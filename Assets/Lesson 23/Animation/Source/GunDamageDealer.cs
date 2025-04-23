using System;
using Dummies;
using StateMachineSystem;
using StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;

namespace Animation
{
    public class GunDamageDealer : MonoBehaviour
    {
        public event Action<int> OnHit;
        
        [SerializeField] private HitScanGunBase _gun;
        //[SerializeField] private int _damage;

        [SerializeField] private WeaponData _data;
        
        private IHealthService _healthService;
        
        public HitScanGunBase Gun => _gun;
        
        private void Start()
        {
            _healthService = ServiceLocator.Instance.GetService<IHealthService>();
            _gun.OnHit += GunHitHandler;
        }

        private void GunHitHandler(Collider collider)
        {
            //Debug.Log($"Gun {gameObject.name} hit: {collider.gameObject.name}");
            if (_healthService.GetHitCollider(collider, out IHitCollider health))
            {
                health.TakeDamage(_data.Damage);
                OnHit?.Invoke(1);
                return;
            }

            OnHit?.Invoke(0);
        }
    }
}