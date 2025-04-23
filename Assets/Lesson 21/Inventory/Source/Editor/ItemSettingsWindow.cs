using Inventory.ItemSystem;
using UnityEditor;
using UnityEngine;

namespace Inventory.Editor
{
    public class ItemSettingsWindow : EditorWindow
    {
        [MenuItem("Tools/Item Settings")]
        private static void ShowWindow()
        {
            var window = GetWindow<ItemSettingsWindow>();
            window.titleContent = new GUIContent("Item Settings");
            window.Show();
        }

        private void OnGUI()
        {
            ItemLibrary itemLibrary = EditorItemSettings.instance.ItemLibrary;
            EditorGUI.BeginChangeCheck();
            itemLibrary = EditorGUILayout.ObjectField(itemLibrary, typeof(ItemLibrary), false) as ItemLibrary;
            if (EditorGUI.EndChangeCheck())
                EditorItemSettings.instance.ItemLibrary = itemLibrary;
        }
    }
}