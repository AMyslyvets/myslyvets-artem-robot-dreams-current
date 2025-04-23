using System.Collections.Generic;
using StateMachineSystem.ServiceLocatorSystem;

namespace Inventory.ItemSystem
{
    public interface IInventoryService : IService
    {
        Inventory Inventory { get; }
        void ShowInventory();
        void HideInventory();
        void ToggleInventory();
        ItemLibrary ItemLibrary { get; }
    }
}