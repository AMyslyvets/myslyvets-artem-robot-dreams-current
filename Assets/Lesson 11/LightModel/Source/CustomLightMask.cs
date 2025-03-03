using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class CustomLightMask : MonoBehaviour
{
    [SerializeField] private string _mapName;
    [SerializeField] private string _tilingName;
    [SerializeField] private Texture _map;
    [SerializeField] private Vector4 _tiling;

    private void OnEnable()
    {
        Shader.SetGlobalTexture(_mapName, _map);
        Shader.SetGlobalVector(_tilingName, _tiling);
    }
}
