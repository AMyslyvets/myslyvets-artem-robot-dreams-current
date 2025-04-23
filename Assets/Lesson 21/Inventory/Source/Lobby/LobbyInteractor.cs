using System;
using System.Collections;
using DefendFlag;
using StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Inventory.Lobby
{
    public class LobbyInteractor : MonoBehaviour
    {
        public event Action<IInteractable> OnInteract; 
        
        [SerializeField] private Transform _transform;
        [SerializeField] private InputAction _interactAction;
        [SerializeField] private float _lockDuration;

        private YieldInstruction _lockDelay;
        
        private IInteractable _currentInteractable;
        private IInteractableService _interactableService;
        private StateMachineSystem.InputController _inputController;
        
        private void Awake()
        {
            _inputController = ServiceLocator.Instance.GetService<StateMachineSystem.InputController>();
            
            _interactAction.Enable();
            _interactAction.performed += InteractHandler;
            _interactableService = ServiceLocator.Instance.GetService<IInteractableService>();
            
            _lockDelay = new WaitForSeconds(_lockDuration);
        }

        private void OnDestroy()
        {
            _interactAction.performed -= InteractHandler;
            _interactAction.Disable();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_interactableService.CanInteract(other, out IInteractable interactable))
            {
                if (_currentInteractable != null)
                    _currentInteractable.Highlight(false);
                _currentInteractable = interactable;
                _currentInteractable.Highlight(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (_currentInteractable != null)
            {
                _currentInteractable.Highlight(false);
            }
        }

        private void InteractHandler(InputAction.CallbackContext context)
        {
            if (_currentInteractable != null)
            {
                OnInteract?.Invoke(_currentInteractable);
                _currentInteractable.Interact();
                StartCoroutine(LockRoutine());
            }
        }

        private IEnumerator LockRoutine()
        {
            _inputController.Lock();
            yield return _lockDelay;
            _inputController.Unlock();
        }
    }
}