using Inventory.ItemSystem;
using StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;

namespace Inventory.Lobby
{
    public class InventoryController : MonoBehaviour
    {
        private StateMachineSystem.InputController _inputController;
        private IInventoryService _inventoryService;
        
        private void Start()
        {
            _inputController = ServiceLocator.Instance.GetService<StateMachineSystem.InputController>();
            _inventoryService = ServiceLocator.Instance.GetService<IInventoryService>();

            _inputController.OnInventory += InventoryHandler;
        }

        private void InventoryHandler()
        {
            _inventoryService.ToggleInventory();
        }
    }
}