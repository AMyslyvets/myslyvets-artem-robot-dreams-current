using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace XRSetup
{
    public class XRPlayerController : MonoBehaviour
    {
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private Transform _characterTransform;
        [SerializeField] private InputAction _lookAround;
        [SerializeField] private InputAction _cameraMove;
        [SerializeField] private InputAction _break;
        
        private void Start()
        {
            _lookAround.Enable();
            _cameraMove.Enable();
            _break.Enable();

            _break.performed += BreakHandler;
        }

        private void LateUpdate()
        {
            Quaternion cameraRotationOffset = _lookAround.ReadValue<Quaternion>();
            //_cameraTransform.localRotation = cameraRotationOffset;
            Vector3 cameraPosition = _cameraMove.ReadValue<Vector3>();
            //_cameraTransform.localPosition = cameraPosition;
            _cameraTransform.SetLocalPositionAndRotation(cameraPosition, cameraRotationOffset);
        }

        private void BreakHandler(InputAction.CallbackContext context)
        {
            Debug.Break();
        }
    }
}