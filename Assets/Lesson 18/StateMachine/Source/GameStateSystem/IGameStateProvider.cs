using System;
using StateMachineSystem.ServiceLocatorSystem;

namespace StateMachineSystem.GameStateSystem
{
    public interface IGameStateProvider : IService
    {
        event Action<GameState> OnGameStateChanged;
        
        GameState GameState { get; }
        
        void SetGameState(GameState gameState);
    }
}