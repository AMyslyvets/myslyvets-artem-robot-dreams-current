using UnityEngine;

namespace Inventory.ItemSystem
{
    public class ItemEntry
    {
        private IItem _item;
        private int _count;
        private int _listPosition;

        public IItem Item => _item;
        public int Count => _count;
        public int ListPosition => _listPosition;
        
        public ItemEntry(IItem item, int count, int listPosition = 0)
        {
            _listPosition = listPosition;
            _item = item;
            _count = Mathf.Clamp(count, 0, _item.MaxStack);
        }

        public void SetCount(int count)
        {
            _count = Mathf.Clamp(count, 0, _item.MaxStack);
        }

        public void SetListPosition(int listPosition)
        {
            _listPosition = listPosition;
        }
    }
}