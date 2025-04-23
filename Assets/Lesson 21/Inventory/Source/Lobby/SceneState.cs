using System.Collections.Generic;
using StateMachineSystem;
using UnityEngine.SceneManagement;

namespace Inventory.Lobby
{
    public class SceneState : StateBase
    {
        private readonly string _sceneName;
        
        public SceneState(StateMachine stateMachine, byte stateId, string sceneName) : base(stateMachine, stateId)
        {
            _sceneName = sceneName;
        }

        public override void Enter()
        {
            _ = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(_sceneName, LoadSceneMode.Single);
        }

        public override void Dispose()
        {
        }
    }
}