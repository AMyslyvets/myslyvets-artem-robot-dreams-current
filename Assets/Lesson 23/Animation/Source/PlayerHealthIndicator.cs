using System.Collections;
using BehaviourTreeSystem;
using StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;

namespace Animation
{
    public class PlayerHealthIndicator : MonoBehaviour
    {
        [SerializeField] private RectTransform _healthValue;
        [SerializeField] private RectTransform _damageValue;
        [SerializeField] private Vector2 _referenceSize;
        [SerializeField] private float _damageDecaySpeed;
        [SerializeField] private float _regenerationSpeed;
        
        private float _targetHealth;
        private float _displayedHealth;
        private float _displayedDamage;

        private ICompositeHealth _health;
        
        private IEnumerator Start()
        {
            yield return null;
            _health = ServiceLocator.Instance.GetService<IPlayerService>().Player.GetComponent<ICompositeHealth>();
            
            ForceHealth(_health.HealthValue01);
            _health.OnHealthChanged01 += HealthChangedHandler;
        }

        private void Update()
        {
            if (_targetHealth < _displayedHealth)
                _displayedHealth = _targetHealth;
            else
            {
                _displayedHealth =
                    Mathf.MoveTowards(_displayedHealth, _targetHealth,
                        _regenerationSpeed * Time.deltaTime);
            }

            if (_displayedDamage > _displayedHealth)
            {
                _displayedDamage =
                    Mathf.MoveTowards(_displayedDamage, _displayedHealth,
                        _damageDecaySpeed * Time.deltaTime);
            }
            else
                _displayedDamage = _displayedHealth;
            
            _healthValue.sizeDelta = new Vector2(_referenceSize.x * _displayedHealth, _referenceSize.y);
            _damageValue.sizeDelta = new Vector2(_referenceSize.x * _displayedDamage, _referenceSize.y);
        }

        private void HealthChangedHandler(float health) => SetHealth(health);

        private void SetHealth(float health)
        {
            _targetHealth = health;
        }

        private void ForceHealth(float health)
        {
            _displayedDamage = _displayedHealth = _targetHealth = health;
        }
    }
}