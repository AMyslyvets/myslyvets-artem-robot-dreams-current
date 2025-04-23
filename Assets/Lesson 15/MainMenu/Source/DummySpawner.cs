using System.Collections;
using Dummies;
using UnityEngine;

namespace MainMenu
{
    public class DummySpawner : MonoBehaviour
    {
        [SerializeField] private DynamicHealthSystem _healthSystem;
        [SerializeField] private CameraSystem _cameraSystem;
        [SerializeField] private DynamicDummy _dummyPrefab;
        [SerializeField] private DummyTrack _track;

        [SerializeField] private float _spawnDelay;
        [SerializeField] private float _spawnOffset;

        private Transform _spawnPoint;

        private YieldInstruction _spawnDelayInstruction;
        
        private IEnumerator Start()
        {
            _spawnDelayInstruction = new WaitForSeconds(_spawnDelay);
            _spawnPoint = transform;
            yield return new WaitForSeconds(_spawnOffset);
            Spawn();
        }

        private void Spawn()
        {
            _spawnPoint.GetPositionAndRotation(out Vector3 position, out Quaternion rotation);
            DynamicDummy dummy = Instantiate(_dummyPrefab, position, rotation);
            _healthSystem.AddCharacter(dummy.Health);
            dummy.SetTrack(_track);
            dummy.HealthIndicator.Billboard.SetCamera(_cameraSystem.Camera);
            dummy.Health.OnDeath += () => DummyDeathHandler(dummy);
        }

        private void DummyDeathHandler(DynamicDummy dummy)
        {
            _healthSystem.RemoveCharacter(dummy.Health);
            _ = StartCoroutine(DelayedSpawn());
        }

        private IEnumerator DelayedSpawn()
        {
            yield return _spawnDelayInstruction;
            Spawn();
        }
    }
}