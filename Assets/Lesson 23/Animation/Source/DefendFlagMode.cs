using System;
using BehaviourTreeSystem;
using DefendFlag;
using StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;

namespace Animation
{
    public class DefendFlagMode : MonoServiceBase, IDefendFlagModeService, IModeService
    {
        public event Action<bool> OnComplete;
        
        [SerializeField] private HordeSpawner[] _hordeSpawners;
        [SerializeField] private FlagController _flagController;
        [SerializeField] private float _duration;
        [SerializeField] private TurretController[] _turrets;

        private float _time;
        private float _reciprocal;
        
        public override Type Type { get; } = typeof(IModeService);
        public Vector3 FlagPosition => _flagController.transform.position;

        protected override void Awake()
        {
            base.Awake();
            ServiceLocator.Instance.AddServiceExplicit(typeof(IDefendFlagModeService), this);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            ServiceLocator.Instance.RemoveServiceExplicit(typeof(IDefendFlagModeService), this);
        }

        private void Start()
        {
            ServiceLocator.Instance.GetService<IPlayerService>().Player.GetComponent<ICompositeHealth>().OnDeath += PlayerDeathHandler;
            enabled = false;
        }

        public void Begin()
        {
            _time = 0f;
            _reciprocal = 1f / _duration;
            
            for (int i = 0; i < _hordeSpawners.Length; ++i)
                _hordeSpawners[i].enabled = true;

            enabled = true;
        }

        private void Update()
        {
            _time += Time.deltaTime;
            if (_time >= _duration)
            {
                End();
                return;
            }
            _flagController.SetProgress(_time * _reciprocal);
        }

        private void End()
        {
            enabled = false;
            
            _flagController.SetProgress(1f);
            _flagController.Complete();

            for (int i = 0; i < _hordeSpawners.Length; ++i)
            {
                HordeSpawner spawner = _hordeSpawners[i];
                spawner.enabled = false;
                spawner.OnEnemyDeath += SpawnerEnemyDeathHandler;
            }

            for (int i = 0; i < _turrets.Length; ++i)
                _turrets[i].enabled = true;
        }

        private void SpawnerEnemyDeathHandler(int enemyCount)
        {
            int sceneEnemyCount = 0;
            for (int i = 0; i < _hordeSpawners.Length; ++i)
                sceneEnemyCount += _hordeSpawners[i].EnemyCount;

            if (sceneEnemyCount <= 0)
            {
                StateMachineSystem.InputController inputController =
                    ServiceLocator.Instance.GetService<StateMachineSystem.InputController>();
                inputController.enabled = false;
                inputController.DisableEscape();
                OnComplete?.Invoke(true);
            }
        }
        
        private void PlayerDeathHandler()
        {
            enabled = false;
            ServiceLocator.Instance.GetService<StateMachineSystem.InputController>().DisableEscape();
            OnComplete?.Invoke(false);
        }
    }
}