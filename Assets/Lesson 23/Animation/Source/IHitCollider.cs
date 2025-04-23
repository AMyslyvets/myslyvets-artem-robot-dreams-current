using MainMenu;
using UnityEngine;

namespace Animation
{
    public interface IHitCollider
    {
        Collider Collider { get; }
        ICompositeHealth Health { get; }
        
        void TakeDamage(int damage);
    }
}