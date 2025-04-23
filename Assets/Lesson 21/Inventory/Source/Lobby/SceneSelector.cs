using System;
using StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;

namespace Inventory.Lobby
{
    public class SceneSelector : MonoServiceBase, ISceneSelector
    {
        public override Type Type { get; } = typeof(ISceneSelector);
        public string SelectedScene { get; private set; }
        
        public void SelectScene(string sceneName)
        {
            SelectedScene = sceneName;
        }
    }
}