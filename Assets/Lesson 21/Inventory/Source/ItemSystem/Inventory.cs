using System.Collections.Generic;
using StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;

namespace Inventory.ItemSystem
{
    public class Inventory
    {
        private List<ItemEntry> _entries = new();
        private Dictionary<IItem, List<ItemEntry>> _items = new();
        
        private readonly IInventoryService _inventoryService;

        public int Count => _entries.Count;
        
        public ItemEntry this[int index] => _entries[index];
        
        public Inventory()
        {
            _inventoryService = ServiceLocator.Instance.GetService<IInventoryService>();
        }

        public void Add(string itemId, int amount)
        {
            if (!_inventoryService.ItemLibrary.TryGetItem(itemId, out IItem item))
            {
                Debug.LogError($"Item with id: {itemId} does not exist");
                return;
            }

            if (_items.TryGetValue(item, out List<ItemEntry> entries))
            {
                for (int i = 0; i < entries.Count; ++i)
                {
                    ItemEntry entry = entries[i];
                    if (entry.Count + amount > item.MaxStack)
                    {
                        amount -= item.MaxStack - entry.Count;
                        entry.SetCount(item.MaxStack);
                    }
                    else
                    {
                        entry.SetCount(entry.Count + amount);
                        amount = 0;
                        break;
                    }
                }

                if (amount > 0)
                {
                    AddStack(item, amount);
                }
            }
            else
            {
                _items.Add(item, new List<ItemEntry>());
                while (amount > item.MaxStack)
                {
                    AddStack(item, item.MaxStack);
                    amount -= item.MaxStack;
                }
                AddStack(item, amount);
            }
        }

        public bool Remove(string itemId, int amount)
        {
            if (!_inventoryService.ItemLibrary.TryGetItem(itemId, out IItem item))
            {
                Debug.LogError($"Item with id: {itemId} does not exist");
                return false;
            }

            if (!_items.TryGetValue(item, out List<ItemEntry> entries))
            {
                Debug.LogError($"Item with id: {itemId} not in inventory");
                return false;
            }
            
            int total = 0;
            for (int j = 0; j < entries.Count; ++j)
            {
                total += entries[j].Count;
            }

            if (total < amount)
            {
                return false;
            }
            
            List<int> indicesToRemove = new();
            int iterator = entries.Count - 1;
            while (amount > 0)
            {
                ItemEntry entry = entries[iterator];

                if (amount >= entry.Count)
                {
                    indicesToRemove.Add(iterator);
                    amount -= entry.Count;
                }
                else
                {
                    entry.SetCount(entry.Count - amount);
                    amount = 0;
                }

                iterator--;
            }

            for (int i = 0; i < indicesToRemove.Count; ++i)
            {
                int index = indicesToRemove[i];
                ItemEntry entry = entries[index];
                entries.RemoveAt(index);
                _entries.RemoveAt(entry.ListPosition);
            }
            
            for (int i = 0; i < _entries.Count; ++i)
                _entries[i].SetListPosition(i);
            
            return true;
        }
        
        private void AddStack(IItem item, int amount)
        {
            ItemEntry itemEntry = new ItemEntry(item, amount, _entries.Count);
            _entries.Add(itemEntry);
            _items[item].Add(itemEntry);
        }

        public bool Contains(IItem item)
        {
            return _items.ContainsKey(item);
        }
        
        public bool TryGetItemEntry(IItem item, out List<ItemEntry> entry)
        {
            return _items.TryGetValue(item, out entry);
        }
    }
}