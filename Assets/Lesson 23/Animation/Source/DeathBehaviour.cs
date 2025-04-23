using BehaviourTreeSystem;
using BehaviourTreeSystem.BehaviourStates;
using StateMachineSystem;
using UnityEngine;

namespace Animation
{
    public class DeathBehaviour : BehaviourStateBase
    {
        private bool _indicatorHidden;
        private float _time;
        
        public DeathBehaviour(StateMachine stateMachine, byte stateId, IEnemyController enemyController) : base(stateMachine, stateId, enemyController)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _time = 0f;
            _indicatorHidden = false;
        }

        protected override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);

            if (_time < enemyController.Data.HealthBarDelayTime)
            {
                _time += deltaTime;
                return;
            }

            if (!_indicatorHidden)
            {
                _indicatorHidden = true;
                Object.Destroy(enemyController.HealthIndicator);
            }

            if (_time < enemyController.Data.BodyDelayTime)
            {
                _time += deltaTime;
                return;
            }
            
            Object.Destroy(enemyController.RootObject);
        }

        public override void Dispose()
        {
        }
    }
}