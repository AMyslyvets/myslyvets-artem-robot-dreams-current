using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Lesson26
{
    public class SimplexEnemySpawner : MonoBehaviour
    {
        [SerializeField] private Vector2 _bounds;
        [SerializeField] private Vector2 _periodBounds;
        [SerializeField] private Vector2Int _countBounds;
        [SerializeField] private Vector2 _speedBounds;
        [SerializeField] private int _poolCapacity;
        [SerializeField] private Transform _poolContainer;
        [SerializeField] private Transform _spawnContainer;
        
        [SerializeField] private SimplexEnemy _enemy;

        #region Pool

        private Queue<SimplexEnemy> _pool = new();

        private void InitPool()
        {
            for (int i = 0; i < _poolCapacity; ++i)
            {
                SimplexEnemy enemy = Instantiate(_enemy, _poolContainer);
                enemy.gameObject.SetActive(false);
                enemy.onArrived += EnemyArrivedHandler;
                _pool.Enqueue(enemy);
            }
        }

        private void ResizePool()
        {
            int newCapacity = _poolCapacity * 2;
            for (int i = _poolCapacity; i < newCapacity; ++i)
            {
                SimplexEnemy enemy = Instantiate(_enemy, _poolContainer);
                enemy.gameObject.SetActive(false);
                enemy.onArrived += EnemyArrivedHandler;
                _pool.Enqueue(enemy);
            }
            _poolCapacity = newCapacity;
        }
        
        private SimplexEnemy GetEnemy()
        {
            if (_pool.Count <= 0)
            {
                ResizePool();
            }
            SimplexEnemy enemy = _pool.Dequeue();
            enemy.gameObject.SetActive(true);
            enemy.transform.SetParent(_spawnContainer);
            return enemy;
        }

        private void ReturnEnemy(SimplexEnemy enemy)
        {
            enemy.gameObject.SetActive(false);
            enemy.transform.SetParent(_poolContainer);
            _pool.Enqueue(enemy);
        }

        #endregion
        
        private IEnumerator Start()
        {
            InitPool();
            
            while (enabled)
            {
                int count = Random.Range(_countBounds.x, _countBounds.y + 1);
                SpawnEnemies(count);
                float delay = Random.Range(_periodBounds.x, _periodBounds.y);
                float time = 0f;
                while (time < delay)
                {
                    yield return null;
                    time += Time.deltaTime;
                }
            }
        }
        
        private void SpawnEnemies(int count)
        {
            for (int i = 0; i < count; ++i)
            {
                Vector3 position = GetRandomInBounds();
                
                //SimplexEnemy enemyInstance = Instantiate(_enemy, position, Quaternion.identity);
                SimplexEnemy enemyInstance = GetEnemy();
                
                enemyInstance.transform.position = position;
                enemyInstance.SetTarget(GetRandomInBounds());
                //enemyInstance.onArrived += EnemyArrivedHandler;
                float speed = Random.Range(_speedBounds.x, _speedBounds.y);
                enemyInstance.SetSpeed(speed);
            }
        }
        
        private void EnemyArrivedHandler(SimplexEnemy enemyInstance)
        {
            //Destroy(enemyInstance.gameObject);
            ReturnEnemy(enemyInstance);
        }

        private Vector3 GetRandomInBounds()
        {
            Vector3 center = transform.position;
            float x = Random.Range(-_bounds.x, _bounds.x);
            float z = Random.Range(-_bounds.y, _bounds.y);
            return new Vector3(center.x + x, center.y, center.z + z);
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Vector3 center = transform.position;
            Vector3[] corners = new Vector3[4];
            corners[0] = new Vector3(center.x + _bounds.x, center.y, center.z + _bounds.y);
            corners[1] = new Vector3(center.x + _bounds.x, center.y, center.z - _bounds.y);
            corners[2] = new Vector3(center.x - _bounds.x, center.y, center.z - _bounds.y);
            corners[3] = new Vector3(center.x - _bounds.x, center.y, center.z + _bounds.y);
            Gizmos.DrawLineStrip(corners, true);
        }
    }
}