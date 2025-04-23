using System;
using StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;

namespace StateMachineSystem.SceneManagement
{
    public interface ISceneManager : IService
    {
        event Action<AsyncOperation> onSceneLoad;
        
        void SetScene(Scenes scene);
        void OnSceneLoad(AsyncOperation operation);
    }
}