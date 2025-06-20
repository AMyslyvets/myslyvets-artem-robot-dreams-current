using Inventory.Lobby;
using StateMachineSystem;
using StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;

namespace Inventory.GameStateSystem
{
    public class Pause : GameStateBase
    {
        private const float FIXED_TIMESTEP = 0.02f;
        private const float REGULAR_TIMESCALE = 1f;
        private const float PAUSE_TIMESCALE = 0.0625f;

        public Pause(StateMachine stateMachine, byte stateId, ISceneService sceneService) : base(stateMachine, stateId, sceneService)
        {
        }

        public override void Enter()
        {
            base.Enter();
            Time.timeScale = PAUSE_TIMESCALE;
            Time.fixedDeltaTime = FIXED_TIMESTEP * PAUSE_TIMESCALE;
            ServiceLocator.Instance.GetService<StateMachineSystem.InputController>().enabled = false;
        }

        public override void Exit()
        {
            base.Exit();
            Time.timeScale = REGULAR_TIMESCALE;
            Time.fixedDeltaTime = FIXED_TIMESTEP * REGULAR_TIMESCALE;
            ServiceLocator.Instance.GetService<StateMachineSystem.InputController>().enabled = true;
        }

        public override void Dispose()
        {
        }
    }
}