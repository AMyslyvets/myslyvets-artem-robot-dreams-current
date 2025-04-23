using UnityEngine;
using UnityEngine.InputSystem;

namespace Lesson22
{
    public class SimplexCamera : MonoBehaviour
    {
        [SerializeField] private InputAction _lookAction;
        [SerializeField] private Transform _yawAnchor;
        [SerializeField] private Transform _pitchAnchor;
        [SerializeField] private Vector2 _sensetivity;
        [SerializeField] private Vector2 _pitchBounds;
        
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private Vector2 _zoomRange;
        [SerializeField] private float _zoomSensetivity;
        [SerializeField] private InputAction _zoomAction;

        private float _yaw;
        private float _pitch;
        private float _zoom;

        private Vector2 _lookInput;
    
        public ref float Pitch => ref _pitch;
        public ref Vector2 PitchBounds => ref _pitchBounds;
        
        private void Awake()
        {
            _yaw = _yawAnchor.localEulerAngles.y;
            _pitch = _pitchAnchor.localEulerAngles.x;
            _lookAction.Enable();
            _lookAction.performed += LookHandler;
            _lookAction.canceled += LookHandler;
            
            _zoomAction.Enable();
            _zoom = _zoomRange.x;
        }

        private void LateUpdate()
        {
            _yaw += _sensetivity.x * _lookInput.x;
            _yawAnchor.localRotation = Quaternion.Euler(0f, _yaw, 0f);
        
            _pitch = Mathf.Clamp(_pitch + _sensetivity.y * _lookInput.y, _pitchBounds.x, _pitchBounds.y);
            _pitchAnchor.localRotation = Quaternion.Euler(_pitch, 0f, 0f);

            _lookInput = Vector2.zero;
            
            _zoom = Mathf.Clamp(_zoom + _zoomAction.ReadValue<float>() * _zoomSensetivity, _zoomRange.x, _zoomRange.y);
            _cameraTransform.localPosition = new Vector3(0f, 0f, -_zoom);
        }

        private void LookHandler(InputAction.CallbackContext context)
        {
            _lookInput = context.ReadValue<Vector2>();
        }
    }
}