using System;
using System.Collections;
using Inventory.Lobby;
using StateMachineSystem;
using StateMachineSystem.GameStateSystem;
using StateMachineSystem.SceneManagement;
using StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

namespace Inventory.GameStateSystem
{
    public class GameStateController : GlobalMonoServiceBase, IGameStateProvider
    {
        public event Action<GameState> OnGameStateChanged; 
        
        private StateMachine _stateMachine;
        
        public GameState GameState
        {
            get => (GameState)_stateMachine.CurrentState.StateId;
            set
            {
                _stateMachine.SetState((byte)value);
                OnGameStateChanged?.Invoke(value);
            }
        }
        
        public override Type Type { get; } = typeof(IGameStateProvider);

        private void Start()
        {
            ISceneService sceneService = ServiceLocator.Instance.GetService<ISceneService>();
            
            _stateMachine = new StateMachine();
            _stateMachine.AddState((byte)GameState.MainMenu, new MainMenu(_stateMachine, (byte)GameState.MainMenu, sceneService));
            _stateMachine.AddState((byte)GameState.Gameplay, new Gameplay(_stateMachine, (byte)GameState.Gameplay, sceneService));
            _stateMachine.AddState((byte)GameState.Paused, new Pause(_stateMachine, (byte)GameState.Paused, sceneService));
        }

        public void SetGameState(GameState gameState)
        {
            GameState = gameState;
        }
    }
}