using System;
using Inventory.ItemSystem;
using Inventory.Lobby;
using StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;

namespace Inventory.UserInterface
{
    public class MerchantUI : MonoBehaviour
    {
        [SerializeField] private MerchantInteractable _merchantInteractable;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private BuyList _buyList;
        [SerializeField] private BuyList _sellList;

        private IInventoryService _inventoryService;
        
        private void Start()
        {
            _inventoryService = ServiceLocator.Instance.GetService<IInventoryService>();

            _buyList.onInventriesUpdate += UpdateLists;
            _sellList.onInventriesUpdate += UpdateLists;
            
            Hide();
        }

        private void UpdateLists()
        {
            _buyList.UpdateList();
            _sellList.UpdateList();
        }
        
        public void Show()
        {
            _canvas.enabled = true;
            _buyList.Open(_merchantInteractable.TradeTable.BuyTable, _merchantInteractable.Inventory, _inventoryService.Inventory);
            _sellList.Open(_merchantInteractable.TradeTable.SellTable, _inventoryService.Inventory, _merchantInteractable.Inventory);
        }

        public void Hide()
        {
            _canvas.enabled = false;
        }
    }
}