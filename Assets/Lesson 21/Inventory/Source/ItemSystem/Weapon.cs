using System;
using UnityEngine;

namespace Inventory.ItemSystem
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Data/Items/Weapon", order = 0)]
    public class Weapon : ItemBase
    {
        public override Type ItemType { get; } = typeof(Weapon);
    }
}