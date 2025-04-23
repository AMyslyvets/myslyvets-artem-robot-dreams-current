using BehaviourTreeSystem.BehaviourStates;
using DefendFlag;
using StateMachineSystem;
using UnityEngine;

namespace Animation
{
    public class FlagEnemyController : EnemyController
    {
        protected override void InitStateMachine()
        {
            _behaviourMachine = new StateMachine();

            _behaviourMachine.AddState((byte)EnemyBehaviour.Deciding,
                new DecisionBehaviour(_behaviourMachine, (byte)EnemyBehaviour.Deciding, this));
            _behaviourMachine.AddState((byte)EnemyBehaviour.Idle,
                new IdleBehaviour(_behaviourMachine, (byte)EnemyBehaviour.Idle, this));
            _behaviourMachine.AddState((byte)EnemyBehaviour.Patrol,
                new StormBehavior(_behaviourMachine, (byte)EnemyBehaviour.Patrol, this));
            _behaviourMachine.AddState((byte)EnemyBehaviour.Search,
                new SearchBehaviour(_behaviourMachine, (byte)EnemyBehaviour.Search, this));
            
            /*_behaviourMachine.AddState((byte)EnemyBehaviour.Attack,
                new AttackBehaviour(_behaviourMachine, (byte)EnemyBehaviour.Attack, this));*/
            _behaviourMachine.AddState((byte)EnemyBehaviour.Attack,
                new ShootBehaviour(_behaviourMachine, (byte)EnemyBehaviour.Attack, this));
            
            _behaviourMachine.AddState((byte)EnemyBehaviour.Death,
                new DeathBehaviour(_behaviourMachine, (byte)EnemyBehaviour.Death, this));
        }
    }
}