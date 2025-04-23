using System;
using Inventory.Lobby;
using UnityEditor;
using UnityEngine;

namespace Inventory.Editor
{
    [FilePath("Inventory/Data/EditorSceneSettings.asset", FilePathAttribute.Location.ProjectFolder)]
    public class EditorSceneSettings : ScriptableSingleton<EditorSceneSettings>
    {
        [SerializeField] private SceneData _sceneData;

        public void Save()
        {
            Save(true);
        }
        
        
        public string[] SceneNames()
        {
            return _sceneData?.SceneNames ?? new []{"None"};
        }

        public string BootScene()
        {
            return _sceneData?.BootSceneName ?? "";
        }
    }
}