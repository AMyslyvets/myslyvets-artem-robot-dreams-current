using System;
using UnityEngine;

namespace Inventory.ItemSystem
{
    [CreateAssetMenu(fileName = "Sample", menuName = "Data/Items/Sample", order = 0)]
    public class Sample : ItemBase
    {
        public override Type ItemType { get; } = typeof(Sample);
    }
}