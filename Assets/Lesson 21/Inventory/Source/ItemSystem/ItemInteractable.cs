using DefendFlag;
using StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;

namespace Inventory.ItemSystem
{
    public sealed class ItemInteractable : InteractableBase
    {
        [SerializeField, ItemId] private string itemId;
        [SerializeField] private int itemAmount;
        
        public override void Interact()
        {
            ServiceLocator.Instance.GetService<IInventoryService>().Inventory.Add(itemId, itemAmount);
            
            base.Interact();
        }
    }
}