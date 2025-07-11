using StateMachineSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace XRImplementation
{
    public class HandGunController : MonoBehaviour
    {
        [SerializeField] private InputAction _handPosition;
        [SerializeField] private InputAction _handRotation;
        [SerializeField] private InputAction _trigger;
        [SerializeField] private Transform _controllerAnchor;
        [SerializeField] private BonePair[] _bonePairs;
        [SerializeField] private HitScanGunBase _gun;
        
        private void Start()
        {
            _handPosition.Enable();
            _handRotation.Enable();
            _trigger.Enable();
            _trigger.performed += TriggerPerformedHandler;
        }

        private void Update()
        {
            Vector3 handPosition = _handPosition.ReadValue<Vector3>();
            Quaternion handRotation = _handRotation.ReadValue<Quaternion>();
            _controllerAnchor.SetLocalPositionAndRotation(handPosition, handRotation);
            
            for (int i = 0; i < _bonePairs.Length; ++i)
                _bonePairs[i].Update();
        }

        private void TriggerPerformedHandler(InputAction.CallbackContext context)
        {
            _gun.Shoot();
        }
    }
}