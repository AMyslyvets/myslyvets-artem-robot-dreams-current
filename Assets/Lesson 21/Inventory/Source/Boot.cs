using System.Collections;
using Inventory.GameStateSystem;
using StateMachineSystem.GameStateSystem;
using StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;

namespace Inventory.Source
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