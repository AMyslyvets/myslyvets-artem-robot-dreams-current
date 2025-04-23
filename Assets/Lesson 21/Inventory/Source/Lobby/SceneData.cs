using System;
using UnityEngine;

namespace Inventory.Lobby
{
    [CreateAssetMenu(fileName = "SceneData", menuName = "Data/Scene Data", order = 0)]
    public class SceneData : ScriptableObject
    {
        [Serializable]
        public struct GameplaySceneEntry
        {
            public string sceneName;
            public string scenePath;
        }
        
        [SerializeField] private string _bootSceneName;
        [SerializeField] private string _lobbySceneName;
        [SerializeField] private GameplaySceneEntry[] _gameplaySceneNames;
        
        private string[] _sceneNames;
        public string BootSceneName => _bootSceneName;
        public string LobbySceneName => _lobbySceneName;
        public GameplaySceneEntry[] GameplaySceneNames => _gameplaySceneNames;
        
        /// <summary>
        /// Editor only data, use GameplaySceneNames in runtime
        /// </summary>
        public string[] SceneNames => _sceneNames;

        private void OnValidate()
        {
            Array.Resize(ref _sceneNames, _gameplaySceneNames.Length);
            for (int i = 0; i < _gameplaySceneNames.Length; ++i)
                _sceneNames[i] = _gameplaySceneNames[i].sceneName;
        }
    }
}