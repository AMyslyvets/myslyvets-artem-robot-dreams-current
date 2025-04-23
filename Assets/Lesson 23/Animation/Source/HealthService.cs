using System;
using System.Collections.Generic;
using StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;

namespace Animation
{
    public class HealthService : MonoServiceBase, IHealthService
    {
        private Dictionary<int, IHitCollider> _hitColliders = new();
        
        public override Type Type { get; } = typeof(IHealthService);

        public void RegisterHitCollider(IHitCollider hitCollider)
        {
            _hitColliders.Add(hitCollider.Collider.GetManagedHasCode(), hitCollider);
        }

        public bool RemoveHitCollider(IHitCollider hitCollider)
        {
            return _hitColliders.Remove(hitCollider.Collider.GetManagedHasCode());
        }
        
        public bool GetHitCollider(Collider collider, out IHitCollider hitCollider)
        {
            return _hitColliders.TryGetValue(collider.GetManagedHasCode(), out hitCollider);
        }
    }
}