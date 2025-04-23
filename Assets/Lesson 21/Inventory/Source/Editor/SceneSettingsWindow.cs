using System;
using StateMachineSystem.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Inventory.Editor
{
    public class SceneSettingsWindow : EditorWindow
    {
        [MenuItem("Tools/Scene Settings")]
        private static void ShowWindow()
        {
            var window = GetWindow<SceneSettingsWindow>();
            window.titleContent = new GUIContent("Scene Settings");
            window.Show();
        }

        private static void OnPlayModeChanged(PlayModeStateChange state)
        {
            if (state != PlayModeStateChange.EnteredEditMode)
                return;
            //EditorApplication.playModeStateChanged -= OnPlayModeChanged;
            string lastOpenedScene = EditorPrefs.GetString("LastOpenedScene");
            if (string.IsNullOrEmpty(lastOpenedScene))
                return;
            EditorPrefs.SetString("LastOpenedScene", "");
            EditorSceneManager.OpenScene(lastOpenedScene);
        }

        private UnityEditor.Editor _editor;
        
        private void OnEnable()
        {
            _editor = UnityEditor.Editor.CreateEditor(EditorSceneSettings.instance);
            EditorApplication.playModeStateChanged += OnPlayModeChanged;
        }

        private void OnDisable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeChanged;
        }

        private void OnGUI()
        {
            if (_editor == null)
                return;
            EditorGUI.BeginChangeCheck();
            _editor.DrawDefaultInspector();
            if (EditorGUI.EndChangeCheck())
                EditorSceneSettings.instance.Save();
            
            if (GUILayout.Button("Play Boot"))
            {
                PlayScene(EditorSceneSettings.instance.BootScene());
            }
        }
        
        private void PlayScene(string scenePath)
        {
            if (!EditorApplication.isPlaying)
            {
                if (!string.IsNullOrEmpty(scenePath))
                {
                    EditorPrefs.SetString("LastOpenedScene", EditorSceneManager.GetActiveScene().path);
                    EditorSceneManager.OpenScene(scenePath);
                    EditorApplication.EnterPlaymode();
                }
                else
                {
                    Debug.LogError($"Scene '{scenePath}' not found in build settings!");
                }
            }
            else
            {
                EditorApplication.ExitPlaymode();
            }
        }

    }
}