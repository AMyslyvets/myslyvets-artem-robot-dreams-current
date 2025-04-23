using UnityEngine;

namespace Animation
{
    public class HitCollider : IHitCollider
    {
        private readonly float _damageMultiplier;
        private readonly Collider _collider;
        private readonly ICompositeHealth _compositeHealth;
        
        public Collider Collider => _collider;
        public ICompositeHealth Health => _compositeHealth;

        public HitCollider(Collider collider, ICompositeHealth compositeHealth, float damageMultiplier)
        {
            _collider = collider;
            _compositeHealth = compositeHealth;
            _damageMultiplier = damageMultiplier;
        }
        
        public void TakeDamage(int damage)
        {
            _compositeHealth.TakeDamage((int)(damage * _damageMultiplier));
        }
    }
}