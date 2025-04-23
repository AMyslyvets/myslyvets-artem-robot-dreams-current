using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Lesson22
{
    public class CarController : MonoBehaviour
    {
        [SerializeField] private WheelCollider[] _frontWheels;
        [SerializeField] private WheelCollider[] _backWheels;

        [SerializeField] private InputAction _moveAction;
        [SerializeField] private float _maxSteeringAngle;
        [SerializeField] private float _acceleration;
        
        private void Start()
        {
            _moveAction.Enable();
        }

        private void FixedUpdate()
        {
            Vector2 moveInput = _moveAction.ReadValue<Vector2>();
            for (int i = 0; i < _backWheels.Length; ++i)
            {
                WheelCollider wheel = _backWheels[i];
                wheel.motorTorque = _acceleration * moveInput.y;
            }

            for (int i = 0; i < _frontWheels.Length; ++i)
            {
                WheelCollider wheel = _frontWheels[i];
                wheel.steerAngle = _maxSteeringAngle * moveInput.x;
            }
        }
    }
}