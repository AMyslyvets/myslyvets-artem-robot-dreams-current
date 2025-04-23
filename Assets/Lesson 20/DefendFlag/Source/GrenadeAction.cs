using System;
using System.Collections;
using System.Collections.Generic;
using StateMachineSystem;
using StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;

namespace DefendFlag
{
    public class GrenadeAction : GrenadeSpawnerBase, ICoroutineExecutable
    {
        public event Action onGrenadesCountChanged;
        
        [SerializeField] private Transform _throwAnchor;
        [SerializeField] private Transform _spawnAnchor;
        
        [SerializeField] private Rigidbody _grenadePrefab;
        [SerializeField] private float _throwForce;
        [SerializeField] private float _throwTorque;
        [SerializeField] private float _fuseTime;
        [SerializeField] private float _explosionDelay;
        [SerializeField] private float _explosionForce;
        [SerializeField] private float _explosionRadius;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private ExplosionController _explosionParticles;
        [SerializeField] private bool _breakOnSpawn;
        [SerializeField] private int _maxGrenades;
        
        private HashSet<Grenade> _grenades = new();
        
        private Vector3 _lastGrenadePosition;
        private float _lastGrenadeRadius;

        private int _grenadeCount;
        
        private StateMachineSystem.InputController _inputController;
        
        public int MaxGrenades => _maxGrenades;
        public int GrenadeCount => _grenadeCount;
        
        private void Start()
        {
            _explosionParticles.ApplyRadius(_explosionRadius);
            _grenadeCount = _maxGrenades;
        }

        private void OnEnable()
        {
            if (_inputController == null)
                _inputController = ServiceLocator.Instance.GetService<StateMachineSystem.InputController>();
            _inputController.OnGrenadeInput += GrenadeInputHandler;
        }
        
        private void OnDisable()
        {
            _inputController.OnGrenadeInput -= GrenadeInputHandler;
        }

        private void GrenadeInputHandler()
        {
            if (_grenadeCount <= 0)
                return;
            _grenadeCount--;
            
            Grenade grenade = new Grenade(this, _grenadePrefab, _spawnAnchor, _throwAnchor,
                _fuseTime, _explosionDelay, _throwForce, _throwTorque,
                _explosionForce, _explosionRadius, _layerMask, _explosionParticles);
            grenade.OnExplode += GrenadeExplodeHandler;
            _grenades.Add(grenade);
            
            onGrenadesCountChanged?.Invoke();
            InvokeOnGrenadeSpawned(grenade);
            
            if (_breakOnSpawn)
                Debug.Break();
        }

        public void ExecuteCoroutine(IEnumerator routine)
        {
            StartCoroutine(routine);
        }

        private void GrenadeExplodeHandler(Grenade grenade)
        {
            _lastGrenadePosition = grenade.Position;
            _lastGrenadeRadius = grenade.ExplosionRadius;
            _grenades.Remove(grenade);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
            Gizmos.DrawSphere(_lastGrenadePosition, _lastGrenadeRadius);
        }

        public void AddGrenades(int amount)
        {
            _grenadeCount = Mathf.Clamp(_grenadeCount + amount, 0, _maxGrenades);
            onGrenadesCountChanged?.Invoke();
        }
    }
}