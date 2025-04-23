using System;
using System.Collections.Generic;
using StateMachineSystem;
using StateMachineSystem.SceneManagement;
using StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;

namespace Inventory.Lobby
{
    public class SceneService : GlobalMonoServiceBase, ISceneService
    {
        [SerializeField, Tooltip("Scene data with names and paths")] private SceneData _data;
        
        private StateMachine _stateMachine;
        
        private readonly Dictionary<string, byte> _nameToIndex = new();
        
        public override Type Type { get; } = typeof(ISceneService);

        private void Start()
        {
            _stateMachine = new StateMachine();

            _stateMachine.AddState(0, new SceneState(_stateMachine, 0, _data.LobbySceneName));
            
            for (int i = 0; i < _data.GameplaySceneNames.Length; ++i)
            {
                byte sceneIndex = (byte)(i + 1);
                string sceneName = _data.GameplaySceneNames[i].sceneName;
                string scenePath = _data.GameplaySceneNames[i].scenePath;
                
                _nameToIndex.Add(sceneName, sceneIndex);
                
                _stateMachine.AddState(sceneIndex, new SceneState(_stateMachine, sceneIndex, scenePath));
            }
        }
        
        public void SetLobby()
        {
            _stateMachine.SetState(0);
        }

        public void SetGameplayScene(string scene)
        {
            _stateMachine.SetState(_nameToIndex[scene]);
        }
    }
}