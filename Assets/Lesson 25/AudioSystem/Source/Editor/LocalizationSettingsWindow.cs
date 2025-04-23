using AudioSystem.LocalizationSystem;
using UnityEditor;
using UnityEngine;

namespace AudioSystem.Editor
{
    /// <summary>
    /// Editor Window, that gives UI to change current LocalizationData
    /// </summary>
    public class LocalizationSettingsWindow : EditorWindow
    {
        // Attribute MenuItem adds an item in menu bar
        [MenuItem("Tools/Localization Settings")]
        private static void ShowWindow()
        {
            // Get window, either creating new or activating persistant, set title, and Show
            var window = GetWindow<LocalizationSettingsWindow>();
            window.titleContent = new GUIContent("Localization Settings");
            window.Show();
        }

        /// <summary>
        /// Method to draw a window, called by UnityEditor
        /// </summary>
        private void OnGUI()
        {
            // Get current LocalizationData reference from singleton
            LocalizationData localizationData = LocalizationEditorSettings.instance.LocalizationData;
            EditorGUI.BeginChangeCheck();
            // Create a property field, so new LocalizationData can be dragged and dropped
            localizationData = EditorGUILayout.ObjectField(localizationData, typeof(LocalizationData), false) as LocalizationData;
            // if change was made, apply it to singleton, so it will save the change
            if (EditorGUI.EndChangeCheck())
                LocalizationEditorSettings.instance.LocalizationData = localizationData;
        }
    }
}