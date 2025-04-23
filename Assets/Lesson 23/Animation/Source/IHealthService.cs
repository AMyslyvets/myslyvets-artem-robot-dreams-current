using StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;

namespace Animation
{
    public interface IHealthService : IService
    {
        void RegisterHitCollider(IHitCollider collider);
        bool RemoveHitCollider(IHitCollider collider);
        
        bool GetHitCollider(Collider collider, out IHitCollider hitCollider);
    }
}