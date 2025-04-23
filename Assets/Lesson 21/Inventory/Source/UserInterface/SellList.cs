using System;
using System.Collections.Generic;
using Inventory.ItemSystem;
using StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;

namespace Inventory.UserInterface
{
    public class SellList : MonoBehaviour
    {
        [SerializeField] private TradeListEntry _tradeListEntryPrefab;
        [SerializeField] private Transform _content;
        
        private readonly List<TradeListEntry> _buyListEntries = new();

        private TradeTable _tradeTable;
        private Inventory.ItemSystem.Inventory _merchantInventory;
        private Inventory.ItemSystem.Inventory _buyerInventory;
        
        private IInventoryService _inventoryService;

        private void Start()
        {
            _inventoryService = ServiceLocator.Instance.GetService<IInventoryService>();
        }

        public void Open(TradeTable tradeTable,
            Inventory.ItemSystem.Inventory merchantInventory, Inventory.ItemSystem.Inventory buyerInventory)
        {
            _tradeTable = tradeTable;
            _merchantInventory = merchantInventory;
            _buyerInventory = buyerInventory;
            
            UpdateList();
        }

        private void UpdateList()
        {
            for (int i = 0; i < _buyListEntries.Count; ++i)
            {
                Destroy(_buyListEntries[i].gameObject);
            }
            _buyListEntries.Clear();
            
            for (int i = 0; i < _merchantInventory.Count; ++i)
            {
                ItemEntry item = _merchantInventory[i];
                if (_tradeTable.TryGetBuyTrade(item.Item, out TradeTable.TradeEntry tradeEntry))
                {
                    TradeListEntry tradeListEntry = Instantiate(_tradeListEntryPrefab, _content);
                    tradeListEntry.gameObject.SetActive(true);
                    tradeListEntry.SetTrade(tradeEntry, item);

                    if (!_inventoryService.ItemLibrary.TryGetItem(tradeEntry.paymentId, out IItem payment))
                    {
                        Debug.LogError($"Payment item {tradeEntry.paymentId} not found");
                        tradeListEntry.SetEnabled(false);
                    }
                    else
                    {
                        if (_buyerInventory.TryGetItemEntry(payment, out List<ItemEntry> paymentEntryList))
                        {
                            int total = 0;
                            for (int j = 0; j < paymentEntryList.Count; ++j)
                            {
                                total += paymentEntryList[j].Count;
                            }
                            tradeListEntry.SetEnabled(total >= tradeEntry.paymentAmount);
                        }
                        else
                        {
                            tradeListEntry.SetEnabled(false);
                        }
                    }
                    
                    _buyListEntries.Add(tradeListEntry);
                }
            }
        }

        private void CompleteTrade(TradeTable.TradeEntry trade)
        {
            //ItemEntry product = _merchantInventory.Find(item => FindProductPredicate(trade, item));
            //ItemEntry payment = _buyerInventory.Find(item => BuyPredicate(trade, item));
        }
        
        public bool FindProductPredicate(TradeTable.TradeEntry trade, ItemEntry item)
        {
            return trade.productId == item.Item.Id;
        }

        public bool HasItemPredicate(ItemEntry entry, IItem item)
        {
            return entry.Item == item;
        }
    }
}