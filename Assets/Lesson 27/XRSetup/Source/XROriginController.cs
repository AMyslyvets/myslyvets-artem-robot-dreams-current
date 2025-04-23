using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace XRSetup
{
    public class XROriginController : MonoBehaviour
    {
        [SerializeField] private CharacterController _characterController;

        [SerializeField] private InputAction _moveAction;
        [SerializeField] private InputAction _lookAction;

        [SerializeField] private float _sensetivity;
        [SerializeField] private float _speed;
        
        private Transform _characterTransform;

        private float _yaw;
        
        private void Start()
        {
            _characterTransform = _characterController.transform;
            
            _moveAction.Enable();
            _lookAction.Enable();
            
            _yaw = _characterTransform.eulerAngles.y;
        }

        private void FixedUpdate()
        {
            Vector2 move = _moveAction.ReadValue<Vector2>();
            Vector2 look = _lookAction.ReadValue<Vector2>();
            
            _yaw += look.x * _sensetivity;
            
            _characterTransform.rotation = Quaternion.Euler(0f, _yaw, 0f);
            
            Vector3 movement = (_characterTransform.forward * move.y + _characterController.transform.right * move.x) *
                               (_speed * Time.fixedDeltaTime);
            
            movement += Physics.gravity * Time.fixedDeltaTime;

            _characterController.Move(movement);
        }
    }
}