using System;
using StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Inventory.Lobby
{
    public class CameraController : MonoBehaviour
    {
        /*[SerializeField] private InputAction _horizontal;
        [SerializeField] private InputAction _vertical;*/
        
        [SerializeField] private Transform _pitchAnchor;
        [SerializeField] private Transform _yawAnchor;
        [SerializeField] private float _sensitivity;
    
        private float _pitch = 20f;
        private float _yaw = 0f;
    
        private Vector2 _lookInput;

        private StateMachineSystem.InputController _inputController;
        
        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            _inputController = ServiceLocator.Instance.GetService<StateMachineSystem.InputController>();
            
            /*_horizontal.Enable();
            _vertical.Enable();*/
        }

        private void LateUpdate()
        {
            Vector2 input = _inputController.LookAroundAction.ReadValue<Vector2>();
            _pitch -= input.y * _sensitivity;
            _yaw += input.x * _sensitivity;
            _pitchAnchor.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
            _yawAnchor.localRotation = Quaternion.Euler(0f, _yaw, 0f);
        }
    }
}