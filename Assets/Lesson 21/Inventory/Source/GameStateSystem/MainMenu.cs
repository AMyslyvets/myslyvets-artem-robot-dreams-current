using Inventory.Lobby;
using StateMachineSystem;
using StateMachineSystem.ServiceLocatorSystem;

namespace Inventory.GameStateSystem
{
    public class MainMenu : GameStateBase
    {
        private readonly ISceneService _sceneService;
        
        public MainMenu(StateMachine stateMachine, byte stateId, ISceneService sceneService) : base(stateMachine, stateId, sceneService)
        {
            _sceneService = ServiceLocator.Instance.GetService<ISceneService>();
        }

        public override void Enter()
        {
            base.Enter();
            _sceneService.SetLobby();
        }

        public override void Dispose()
        {
        }
    }
}