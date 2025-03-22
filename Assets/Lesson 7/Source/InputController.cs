using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Lesson7
{
    public class InputController : MonoBehaviour
    {
        public static event Action<Vector2> OnMoveInput;
        public static event Action<Vector2> OnLookInput;
        public static event Action<bool> OnCameraLock;

        [SerializeField] private InputActionAsset _inputActionAsset;
        [SerializeField] private string _mapName;
        [SerializeField] private string _moveName;
        [SerializeField] private string _lookAroundName;
        [SerializeField] private string _cameraLockName;
        [SerializeField] private CursorLockMode _enabledCursorMode;
        [SerializeField] private CursorLockMode _disabledCursorMode;
        [SerializeField] private string _primaryFireName;//dell
        [SerializeField] private string _secondaryFireName;//dell
        
        
        public static event Action OnPrimaryInput;
        public static event Action<bool> OnSecondaryInput;
        

        private InputAction _moveAction;
        private InputAction _lookAroundAction;
        private InputAction _cameraLockAction;
        private InputAction _primaryFireAction;//dell
        private InputAction _secondaryFireAction;//dell

        private bool _inputUpdated;
        
        private InputActionMap _actionMap; //dell

        private void OnEnable()
        {
            Cursor.visible = false;
            Cursor.lockState = _enabledCursorMode;
            
            _inputActionAsset.Enable();
            InputActionMap actionMap = _inputActionAsset.FindActionMap(_mapName);
            _moveAction = actionMap[_moveName];
            _lookAroundAction = actionMap[_lookAroundName];
            _cameraLockAction = actionMap[_cameraLockName];

            _moveAction.performed += MovePerformedHandler;
            _moveAction.canceled += MoveCanceledHandler;

            _lookAroundAction.performed += LookPerformedHandler;
            _lookAroundAction.canceled += LookCanceledHandler;
            
            _cameraLockAction.performed += CameraLockPerformedHandler;
            _cameraLockAction.canceled += CameraLockCanceledHandler;
            
            _actionMap = _inputActionAsset.FindActionMap(_mapName);//dell
            _primaryFireAction = _actionMap[_primaryFireName];// dell
            _secondaryFireAction = _actionMap[_secondaryFireName]; //dell
        }

        private void OnDisable()
        {
            _inputActionAsset.Disable();
            
            Cursor.visible = true;
            Cursor.lockState = _disabledCursorMode;
        }

        private void OnDestroy()
        {
            OnMoveInput = null;
            OnLookInput = null;
            
            _primaryFireAction.performed -= PrimaryFirePerformedHandler;//dell
            
            _secondaryFireAction.performed -= SecondaryFirePerformedHandler;//dell
            _secondaryFireAction.canceled -= SecondaryFireCanceledHandler;//dell
            
            OnPrimaryInput = null;//
            OnSecondaryInput = null;//
            
        }
        

        private void MovePerformedHandler(InputAction.CallbackContext context)
        {
            OnMoveInput?.Invoke(context.ReadValue<Vector2>());
        }

        private void MoveCanceledHandler(InputAction.CallbackContext context)
        {
            OnMoveInput?.Invoke(context.ReadValue<Vector2>());
        }

        private void LookPerformedHandler(InputAction.CallbackContext context)
        {
            OnLookInput?.Invoke(context.ReadValue<Vector2>());
        }
        
        private void LookCanceledHandler(InputAction.CallbackContext context)
        {
            OnLookInput?.Invoke(context.ReadValue<Vector2>());
        }

        private void CameraLockPerformedHandler(InputAction.CallbackContext context)
        {
            OnCameraLock?.Invoke(true);
        }

        private void CameraLockCanceledHandler(InputAction.CallbackContext context)
        {
            OnCameraLock?.Invoke(false);
        }
        private void PrimaryFirePerformedHandler(InputAction.CallbackContext context)//dell
        {
            OnPrimaryInput?.Invoke();
        }
        
        private void SecondaryFirePerformedHandler(InputAction.CallbackContext context)//dell
        {
            OnSecondaryInput?.Invoke(true);
        }
        
        private void SecondaryFireCanceledHandler(InputAction.CallbackContext context)//dell
        {
            OnSecondaryInput?.Invoke(false);
        }
    }
}