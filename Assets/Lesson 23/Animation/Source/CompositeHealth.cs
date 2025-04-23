using System;
using StateMachineSystem;
using StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;

namespace Animation
{
    public class CompositeHealth : MonoBehaviour, ICompositeHealth
    {
        [Serializable]
        public struct ColliderData
        {
            public Collider collider;
            public float damageMultiplier;
        }
        
        public event Action OnDeath;
        public event Action<int> OnTakeDamage;
        public event Action<int> OnHealthChanged;
        public event Action<float> OnHealthChanged01;

        [SerializeField] private HealthData _healthData;
        [SerializeField] private ColliderData[] _colliderData;

        private bool _isAlive;
        private int _healthValue;
        
        private IHealthService _healthService;
        private IHitCollider[] _hitColliders;

        public int HealthValue
        {
            get => _healthValue;
            private set
            {
                _healthValue = value;
                OnHealthChanged?.Invoke(_healthValue);
                OnHealthChanged01?.Invoke(HealthValue01);
            }
        }

        public bool IsAlive
        {
            get => _isAlive;
            private set
            {
                _isAlive = value;
                if (!_isAlive)
                    Death();
            }
        }
        
        public float HealthValue01 => HealthValue / (float)MaxHealthValue;
        public int MaxHealthValue { get; private set; }

        private void Awake()
        {
            _healthService = ServiceLocator.Instance.GetService<IHealthService>();
            
            MaxHealthValue = _healthData.MaxHealth;
            SetHealth(_healthData.MaxHealth);

            _hitColliders = new IHitCollider[_colliderData.Length];
            for (int i = 0; i < _colliderData.Length; ++i)
            {
                ColliderData colliderData = _colliderData[i];
                IHitCollider hitCollider = new HitCollider(colliderData.collider, this, colliderData.damageMultiplier);
                _healthService.RegisterHitCollider(hitCollider);
                _hitColliders[i] = hitCollider;
            }
        }

        public void TakeDamage(int damage)
        {
            if (!IsAlive)
                return;
            
            HealthValue = Mathf.Clamp(HealthValue - damage, 0, MaxHealthValue);
            
            IsAlive = HealthValue > 0;
            
            OnTakeDamage?.Invoke(damage);
        }

        public void Heal(int heal)
        {
            if (!IsAlive)
                return;
            
            HealthValue = Mathf.Clamp(HealthValue + heal, 0, MaxHealthValue);
            
            IsAlive = HealthValue > 0;
        }

        public void SetHealth(int health)
        {
            HealthValue = health;
            IsAlive = HealthValue > 0;
        }

        private void Death()
        {
            OnDeath?.Invoke();

            for (int i = 0; i < _hitColliders.Length; ++i)
            {
                _healthService.RemoveHitCollider(_hitColliders[i]);
            }
        }
    }
}