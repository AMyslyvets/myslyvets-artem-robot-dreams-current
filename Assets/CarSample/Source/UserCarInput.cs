using UnityEngine;
using UnityEngine.InputSystem;

namespace CarSample
{
    public class UserCarInput : MonoBehaviour
    {
        [SerializeField] private CarController _carController;
        [SerializeField] private InputAction _horizontal;
        [SerializeField] private InputAction _vertical;
        [SerializeField] private InputAction _brake;

        private void OnEnable()
        {
            _horizontal.Enable();
            _vertical.Enable();
            _brake.Enable();
            _brake.performed += BrakePerformedHandler;
            _brake.canceled += BrakeCanceledHandler;
        }

        public void Update()
        {
            _carController.SetAcceleration(_vertical.ReadValue<float>());
            _carController.SetSteering(_horizontal.ReadValue<float>());
        }

        private void BrakePerformedHandler(InputAction.CallbackContext context)
        {
            _carController.SetBrake(1f);
        }

        private void BrakeCanceledHandler(InputAction.CallbackContext context)
        {
            _carController.SetBrake(0f);
        }
    }
}