using System;
using System.Collections.Generic;
using StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;

namespace DefendFlag
{
    public class Interactor : MonoBehaviour
    {
        public event Action<IInteractable> OnInteract;
        
        [SerializeField] private Transform _transform;
        
        private IInteractable _currentInteractable;
        
        private StateMachineSystem.InputController _inputController;
        private IInteractableService _interactableService;

        private readonly HashSet<IInteractable> _interactables = new();
        
        private void Awake()
        {
            _inputController = ServiceLocator.Instance.GetService<StateMachineSystem.InputController>();
            _inputController.OnInteract += InteractHandler;
            _interactableService = ServiceLocator.Instance.GetService<IInteractableService>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_interactableService.CanInteract(other, out IInteractable interactable))
            {
                _interactables.Add(interactable);
                if (_currentInteractable == null)
                {
                    _currentInteractable = interactable;
                    _currentInteractable.Highlight(true);
                }
                
                if (_currentInteractable != null)
                    _currentInteractable.Highlight(false);
                _currentInteractable = interactable;
                _currentInteractable.Highlight(true);
            }
        }

        private void FixedUpdate()
        {
            IInteractable closest = FindInteractable();
            if (closest != null && closest != _currentInteractable)
            {
                _currentInteractable?.Highlight(false);
                _currentInteractable = closest;
                _currentInteractable.Highlight(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (_interactableService.CanInteract(other, out IInteractable interactable))
            {
                _interactables.Remove(interactable);
                if (interactable == _currentInteractable)
                {
                    _currentInteractable.Highlight(false);
                    _currentInteractable = FindInteractable();
                }
            }
        }

        private IInteractable FindInteractable()
        {
            Vector3 center = _transform.position;
            IInteractable closest = null;
            float minSqrDistance = float.MaxValue;
            foreach (IInteractable interactable in _interactables)
            {
                float sqrDistance = (center - interactable.Position).sqrMagnitude;
                if (sqrDistance < minSqrDistance)
                {
                    minSqrDistance = sqrDistance;
                    closest = interactable;
                }
            }

            return closest;
        }

        private void InteractHandler()
        {
            if (_currentInteractable != null)
            {
                _currentInteractable.onDestroy += InteractableDestroyHandler;
                _currentInteractable.Interact();
                OnInteract?.Invoke(_currentInteractable);
            }
        }

        private void InteractableDestroyHandler(IInteractable interactable)
        {
            _interactables.Remove(interactable);
            if (_currentInteractable == interactable)
            {
                _currentInteractable = null;
            }
        }
    }
}