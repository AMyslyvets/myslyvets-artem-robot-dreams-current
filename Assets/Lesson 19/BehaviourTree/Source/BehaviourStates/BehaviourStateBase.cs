using StateMachineSystem;

namespace BehaviourTreeSystem.BehaviourStates
{
    public abstract class BehaviourStateBase : StateBase
    {
        protected readonly IEnemyController enemyController;
        
        protected BehaviourStateBase(StateMachine stateMachine, byte stateId, IEnemyController enemyController) : base(stateMachine, stateId)
        {
            this.enemyController = enemyController;
        }
    }
}