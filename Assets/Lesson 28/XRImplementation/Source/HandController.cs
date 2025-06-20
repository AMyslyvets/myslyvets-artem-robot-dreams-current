using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.UI;

namespace XRImplementation
{
    public class HandController : MonoBehaviour, IUIHoverInteractor
    {
        [SerializeField] private InputAction _handPosition;
        [SerializeField] private InputAction _handRotation;
        [SerializeField] private InputAction _trigger;
        [SerializeField] private Transform _controllerAnchor;
        [SerializeField] private BonePair[] _bonePairs;
        [SerializeField] private Transform _indexTip;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private float _range;
        [SerializeField] XRUIInputModule _inputModule;
        [SerializeField] private LineRenderer _lineRenderer;

        private bool _triggerPressed;
        private PointerEventData _pointerEventData;
        private bool _hovered;
        
        private void Start()
        {
            _handPosition.Enable();
            _handRotation.Enable();
            _trigger.Enable();
            _trigger.performed += TriggerPerformedHandler;
            _trigger.canceled += TriggerCanceledHandler;
            
            _inputModule.RegisterInteractor(this);
            _inputModule.pointerMove += PointerMoveHandler;
            
            _lineRenderer.positionCount = 2;
        }

        private void OnDestroy()
        {
            _inputModule.UnregisterInteractor(this);
        }

        private void FixedUpdate()
        {
            Vector3 handPosition = _handPosition.ReadValue<Vector3>();
            Quaternion handRotation = _handRotation.ReadValue<Quaternion>();
            _controllerAnchor.SetLocalPositionAndRotation(handPosition, handRotation);
            
            for (int i = 0; i < _bonePairs.Length; ++i)
                _bonePairs[i].Update();
        }

        private void LateUpdate()
        {
            _lineRenderer.SetPosition(0, _indexTip.position);
            
            Vector3 hitPoint = Vector3.zero;
            
            if (_hovered && _pointerEventData != null)
            {
                hitPoint = _pointerEventData.pointerCurrentRaycast.worldPosition;
            }
            else
            {
                hitPoint = _controllerAnchor.position + _controllerAnchor.forward * _range;
            }
            
            _lineRenderer.SetPosition(1, hitPoint);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(_controllerAnchor.position, _controllerAnchor.position + _controllerAnchor.forward * _range);
        }
        
        public virtual void UpdateUIModel(ref TrackedDeviceModel model)
        {
            if (!isActiveAndEnabled)
            {
                model.Reset(false);
                return;
            }

            var originTransform = _controllerAnchor;

            model.position = originTransform.position;
            model.orientation = originTransform.rotation;
            model.select = _triggerPressed;
            model.scrollDelta = Vector2.zero;
            model.raycastLayerMask = _layerMask;
            model.interactionType = UIInteractionType.Ray;

            var raycastPoints = model.raycastPoints;
            raycastPoints.Clear();
            raycastPoints.Add(_controllerAnchor.position);
            raycastPoints.Add(_controllerAnchor.position + _controllerAnchor.forward * _range);
        }

        /// <inheritdoc />
        public bool TryGetUIModel(out TrackedDeviceModel model)
        {
            if (_inputModule != null)
            {
                return _inputModule.GetTrackedDeviceModel(this, out model);
            }

            model = TrackedDeviceModel.invalid;
            return false;
        }

        public void OnUIHoverEntered(UIHoverEventArgs args)
        {
            _hovered = true;
            Debug.Log($"Hover Enter {args.uiObject.name}");
        }

        public void OnUIHoverExited(UIHoverEventArgs args)
        {
            _hovered = false;
            Debug.Log($"Hover Exit {args.uiObject.name}");
        }

        public UIHoverEnterEvent uiHoverEntered { get; } = new();
        public UIHoverExitEvent uiHoverExited { get; } = new();

        private void TriggerPerformedHandler(InputAction.CallbackContext context)
        {
            _triggerPressed = true;
        }
        
        private void TriggerCanceledHandler(InputAction.CallbackContext context)
        {
            _triggerPressed = false;
        }

        private void PointerMoveHandler(GameObject moveObject, PointerEventData eventData)
        {
            _pointerEventData = eventData;
        }
    }
}