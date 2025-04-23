using StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;

namespace DefendFlag
{
    public interface IInteractableService : IService
    {
        void AddInteractable(Collider collider, IInteractable interactable);
        void RemoveInteractable(Collider collider, IInteractable interactable);
        
        bool CanInteract(Collider collider, out IInteractable interactable);
    }
}