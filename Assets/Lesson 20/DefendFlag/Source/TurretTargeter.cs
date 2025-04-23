using System;
using System.Collections.Generic;
using BehaviourTreeSystem;
using Dummies;
using PhysX;
using StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;

namespace DefendFlag
{
    public class TurretTargeter : MonoBehaviour
    {
        public Action<ITargetable> OnFirstTargetEnter;
        public Action<ITargetable> OnLastTargetExit;
        
        [SerializeField] private Transform _transform;
        [SerializeField] private Collider _collider;
        
        private HashSet<ITargetable> _targets = new();
        
        private ITargetable _currentTarget;
        private IPlayerService _playerService;
        private IHealthService _healthService;

        private void Awake()
        {
            _playerService = ServiceLocator.Instance.GetService<IPlayerService>();
            _healthService = ServiceLocator.Instance.GetService<IHealthService>();
        }

        public void Enable()
        {
            enabled = true;
            _collider.enabled = true;
        }

        public void Disable()
        {
            enabled = false;
            _collider.enabled = false;
            _targets.Clear();
            OnLastTargetExit?.Invoke(null);
        }

        private void OnTriggerEnter(Collider other)
        {
            //Debug.Log($"OnTriggerEnter: {other.gameObject.name}");
            ITargetable target = other.GetComponent<ITargetable>();
            if (target == null || _playerService.IsPlayer(other))
                return;
            _targets.Add(target);

            if (_healthService.GetHealth(other, out Health health))
                health.OnDeath += () => TargetDeathHandler(target);
            
            if (_targets.Count < 2)
                OnFirstTargetEnter?.Invoke(target);
        }

        private void OnTriggerExit(Collider other)
        {
            ITargetable target = other.GetComponent<ITargetable>();
            if (target == null)
                return;
            _targets.Remove(target);
            if (_targets.Count < 1)
                OnLastTargetExit?.Invoke(target);
        }

        public ITargetable GetTarget()
        {
            float minSqrDistance = float.MaxValue;
            ITargetable closest = null;
            foreach (ITargetable target in _targets)
            {
                if (target == null || target.TargetPivot == null)
                    continue;
                float sqrDistance = (target.TargetPivot.position - _transform.position).sqrMagnitude;
                if (sqrDistance >= minSqrDistance)
                    continue;
                closest = target;
                minSqrDistance = sqrDistance;
            }
            return closest;
        }

        public bool HasTarget(ITargetable target)
        {
            if (target == null || target.TargetPivot == null && _targets.Remove(target))
            {
                return false;
            }
            return _targets.Contains(target);
        }
        
        public HashSet<ITargetable> GetTargets() => _targets;

        private void TargetDeathHandler(ITargetable target)
        {
            _targets.Remove(target);
            if (_targets.Count < 1)
                OnLastTargetExit?.Invoke(target);
        }
    }
}