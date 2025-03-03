using System;
using System.Collections.Generic;
using UnityEngine;

namespace LightEquation.Source
{
    public class NormalController : MonoBehaviour
    {
        [SerializeField] private MeshRenderer[] _meshRenderers;
        [SerializeField] private Material _normalMaterial;

        private void OnEnable()
        {
            for (int i = 0; i < _meshRenderers.Length; ++i)
            {
                List<Material> materials = new();
                _meshRenderers[i].GetSharedMaterials(materials);
                materials.Add(_normalMaterial);
                _meshRenderers[i].SetSharedMaterials(materials);
            }
        }

        private void OnDisable()
        {
            for (int i = 0; i < _meshRenderers.Length; ++i)
            {
                List<Material> materials = new();
                _meshRenderers[i].GetSharedMaterials(materials);
                materials.Remove(_normalMaterial);
                _meshRenderers[i].SetSharedMaterials(materials);
            }
        }
    }
}