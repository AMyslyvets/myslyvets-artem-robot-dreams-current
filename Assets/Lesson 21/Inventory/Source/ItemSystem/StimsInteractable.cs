using BehaviourTreeSystem;
using DefendFlag;
using StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;

namespace Inventory.ItemSystem
{
    public sealed class StimsInteractable : InteractableBase
    {
        [SerializeField] private int _stims;
        
        public override void Interact()
        {
            StimsControllerBase stims = ServiceLocator.Instance.GetService<IPlayerService>().Player.GetComponent<StimsControllerBase>();
            stims.AddStims(_stims);
            base.Interact();
        }
    }
}