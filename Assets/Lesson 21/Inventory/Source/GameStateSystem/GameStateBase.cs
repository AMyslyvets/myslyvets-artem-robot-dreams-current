using Inventory.Lobby;
using StateMachineSystem;
using StateMachineSystem.SceneManagement;

namespace Inventory.GameStateSystem
{
    public abstract class GameStateBase : StateBase
    {
        protected readonly ISceneService _sceneService;
        protected string _scene;
        
        protected GameStateBase(StateMachine stateMachine, byte stateId, ISceneService sceneService) : base(stateMachine, stateId)
        {
            _sceneService = sceneService;
        }
    }
}