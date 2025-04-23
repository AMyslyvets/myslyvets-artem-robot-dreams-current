using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AudioSystem.Editor
{
    /// <summary>
    /// Editor window that helps analyze current returned Alphamap content of TerrainData
    /// </summary>
    public class TerrainInspectorWindow : EditorWindow
    {
        [MenuItem("Tools/Terrain Inspector")]
        public static void ShowWindow()
        {
            GetWindow(typeof(TerrainInspectorWindow), false, "Terrain Inspector").Show();
        }
        
        [SerializeField] private Terrain _terrain;
        
        private UnityEditor.Editor _selfEditor;
        private Vector2 _scrollPosition;

        private void OnEnable()
        {
            _selfEditor = UnityEditor.Editor.CreateEditor(this);
        }

        private void OnDisable()
        {
            DestroyImmediate(_selfEditor);
        }

        private void OnGUI()
        {
            _selfEditor.DrawDefaultInspector();
            if (_terrain == null)
                return;
            
            EditorGUILayout.LabelField("Alphamap resolution", _terrain.terrainData.alphamapResolution.ToString());
            
            Texture2D[] alphaMaps = _terrain.terrainData.alphamapTextures;
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

            for (int i = 0; i < alphaMaps.Length; ++i)
            {
                Texture2D alphaMap = alphaMaps[i];
                Rect controlRect = EditorGUILayout.GetControlRect(false, 256, GUILayout.Width(256));
                //EditorGUI.DrawTextureTransparent(controlRect, alphaMap, ScaleMode.ScaleToFit);
                EditorGUI.DrawPreviewTexture(controlRect, alphaMap);
            }
            
            EditorGUILayout.EndScrollView();
        }
    }
}