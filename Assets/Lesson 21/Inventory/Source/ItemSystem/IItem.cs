using System;

namespace Inventory.ItemSystem
{
    public interface IItem
    {
        string Id { get; }
        Type ItemType { get; }
        int MaxStack { get; }
        string Name { get; }
        string Description { get; }
    }
}