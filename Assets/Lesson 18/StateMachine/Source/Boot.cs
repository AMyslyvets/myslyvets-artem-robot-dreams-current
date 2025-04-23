using System;
using System.Collections;
using StateMachineSystem.GameStateSystem;
using StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;

namespace StateMachineSystem
{
    public class Boot : MonoBehaviour
    {
        public IEnumerator Start()
        {
            yield return null;
            ServiceLocator.Instance.GetService<IGameStateProvider>().SetGameState(GameState.MainMenu);
        }
    }
}