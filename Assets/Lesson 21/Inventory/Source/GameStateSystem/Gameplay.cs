using Inventory.Lobby;
using StateMachineSystem;
using StateMachineSystem.SceneManagement;
using StateMachineSystem.ServiceLocatorSystem;

namespace Inventory.GameStateSystem
{
    public class Gameplay : GameStateBase
    {
        private readonly ISceneService _sceneService;
        
        public Gameplay(StateMachine stateMachine, byte stateId, ISceneService sceneService) : base(stateMachine, stateId, sceneService)
        {
            _sceneService = ServiceLocator.Instance.GetService<ISceneService>();
        }

        public override void Enter()
        {
            base.Enter();
            ISceneSelector sceneSelector = ServiceLocator.Instance.GetService<ISceneSelector>();
            if (sceneSelector == null)
                return;
            _sceneService.SetGameplayScene(ServiceLocator.Instance.GetService<ISceneSelector>().SelectedScene);
        }

        public override void Dispose()
        {
        }
    }
}