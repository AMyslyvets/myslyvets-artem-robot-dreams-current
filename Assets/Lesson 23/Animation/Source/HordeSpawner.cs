using System;
using System.Collections.Generic;
using BehaviourTreeSystem;
using DefendFlag;
using StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Animation
{
    public class HordeSpawner : MonoBehaviour, INavPointProvider
    {
        public event Action<int> OnEnemyDeath; 
        
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private float _spawnRadius;
        [SerializeField] private Vector2Int _spawnCount;
        [SerializeField] private EnemyController _enemyController;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private Vector2 _periodBounds;
        
        private List<EnemyController> _enemies = new();

        private Vector3 _point;
        private NavMeshHit _hit;

        private ICameraService _cameraSystem;
        private IDefendFlagModeService _defendFlagMode;
        
        private float _time;
        private float _spawnDelay;

        public int EnemyCount => _enemies.Count;
        
        private void Awake()
        {
            enabled = false;
        }

        private void OnEnable()
        {
            _defendFlagMode = ServiceLocator.Instance.GetService<IDefendFlagModeService>();
            _cameraSystem = ServiceLocator.Instance.GetService<ICameraService>();
            
            _time = 0f;
            SpawnEnemies(Random.Range(_spawnCount.x, _spawnCount.y));
            _spawnDelay = Random.Range(_periodBounds.x, _periodBounds.y);
        }

        private void Update()
        {
            if (_time < _spawnDelay)
            {
                _time += Time.deltaTime;
                return;
            }

            _time = 0f;
            SpawnEnemies(Random.Range(_spawnCount.x, _spawnCount.y));
            _spawnDelay = Random.Range(_periodBounds.x, _periodBounds.y);
        }

        private void SpawnEnemies(int count)
        {
            for (int i = 0; i < count; ++i)
                SpawnEnemy();
        }
        
        [ContextMenu("GetPoint")]
        private void GetPointInternal()
        {
            Vector3 center = transform.position + _offset;
            Vector2 randomInCircle = Random.insideUnitCircle * _spawnRadius;
            _point.x = randomInCircle.x + center.x;
            _point.y = center.y;
            _point.z = randomInCircle.y + center.z;
            NavMesh.SamplePosition(_point, out _hit, 1.0f, NavMesh.AllAreas);
        }

        public Vector3 GetPoint()
        {
            GetPointInternal();
            return _hit.position;
        }
        
        [ContextMenu("Spawn Enemy")]
        private void SpawnEnemy()
        {
            GetPointInternal();

            int depth = 0;
            while (!_hit.hit)
            {
                GetPointInternal();
                depth++;
                if (depth > 100000)
                {
                    Debug.LogError("Point sampling reached 100000 iterations, aborting");
                    return;
                }
            }
            
            Vector3 flagDirection = Vector3.ProjectOnPlane((_defendFlagMode.FlagPosition - _hit.position), Vector3.up).normalized;
            EnemyController enemy = Instantiate(_enemyController, _hit.position, Quaternion.LookRotation(flagDirection));
            enemy.Initialize(this, _cameraSystem.Camera);
            
            enemy.GetComponent<ICompositeHealth>().OnDeath += () => EnemyDeathHandler(enemy);
            
            _enemies.Add(enemy);
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Vector3 center = transform.position + _offset;
            
            Gizmos.DrawWireSphere(center, _spawnRadius);
            
            Gizmos.color = _hit.hit ? Color.blue : Color.red;
            
            Gizmos.DrawSphere(_hit.hit ? _hit.position : _point, 0.33f);
        }

        private void EnemyDeathHandler(EnemyController enemy)
        {
            _enemies.Remove(enemy);
            OnEnemyDeath?.Invoke(_enemies.Count);
        }
    }
}