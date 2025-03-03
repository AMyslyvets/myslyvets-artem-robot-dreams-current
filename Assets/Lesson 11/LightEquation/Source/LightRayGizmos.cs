using System;
using UnityEngine;

namespace LightEquation.Source
{
    public class LightRayGizmos : MonoBehaviour
    {
        [SerializeField] private LightSystem _lightSystem;
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private MeshFilter[] _meshFilters;

        private Mesh _mesh;
        
        private Vector3[] _vertices;
        private Color[] _colors;
        private int[] _indices;

        private void OnEnable()
        {
            if (_mesh == null)
            {
                _mesh = new Mesh();
                _meshFilter.sharedMesh = _mesh;
            }

            _mesh.Clear();
            
            int vertexCount = 1;
            
            for (int i = 0; i < _meshFilters.Length; ++i)
                vertexCount += _meshFilters[i].sharedMesh.vertexCount;
            
            _vertices = new Vector3[vertexCount];

            Vector3 lightPosition = _lightSystem.transform.position;
            _vertices[0] = lightPosition;
            
            _colors = new Color[vertexCount];
            _colors[0] = _lightSystem.LightColor;
            
            int vertexOffset = 1;

            for (int i = 0; i < _meshFilters.Length; ++i)
            {
                MeshFilter meshFilter = _meshFilters[i];
                Matrix4x4 matrix = meshFilter.transform.localToWorldMatrix;
                Matrix4x4 normalMatrix = matrix.inverse.transpose;
                Vector3[] vertices = meshFilter.sharedMesh.vertices;
                Vector3[] normals = meshFilter.sharedMesh.normals;
                
                for (int j = 0; j < vertices.Length; ++j)
                {
                    Vector3 vertex = matrix.MultiplyPoint(vertices[j]);
                    
                    _vertices[j + vertexOffset] = vertex;
                    
                    float NdL = Vector3.Dot(normalMatrix.MultiplyVector(normals[j]).normalized, (lightPosition - vertex).normalized);

                    Color color = _lightSystem.LightColor * Mathf.Max(0f, NdL);
                    color.a = NdL >= 0f ? 1f : .33f;

                    _colors[j + vertexOffset] = color;
                }

                vertexOffset += meshFilter.sharedMesh.vertexCount;
            }
            
            _mesh.vertices = _vertices;
            _mesh.colors = _colors;
            
            _indices = new int[(vertexCount - 1) * 2];

            for (int i = 1; i < vertexCount; ++i)
            {
                _indices[(i - 1) * 2] = 0;
                _indices[(i - 1) * 2 + 1] = i;
            }
            
            _mesh.SetIndices(_indices, MeshTopology.Lines, 0);
            
            _mesh.RecalculateBounds();
            _mesh.UploadMeshData(false);
        }

        private void Update()
        {
            Vector3 lightPosition = _lightSystem.transform.position;
            _vertices[0] = lightPosition;
            
            _colors[0] = _lightSystem.LightColor;
            
            int vertexOffset = 1;

            for (int i = 0; i < _meshFilters.Length; ++i)
            {
                MeshFilter meshFilter = _meshFilters[i];
                Matrix4x4 matrix = meshFilter.transform.localToWorldMatrix;
                Matrix4x4 normalMatrix = matrix.inverse.transpose;
                Vector3[] vertices = meshFilter.sharedMesh.vertices;
                Vector3[] normals = meshFilter.sharedMesh.normals;
                
                for (int j = 0; j < vertices.Length; ++j)
                {
                    Vector3 vertex = matrix.MultiplyPoint(vertices[j]);
                    
                    _vertices[j + vertexOffset] = vertex;
                    
                    float NdL = Vector3.Dot(normalMatrix.MultiplyVector(normals[j]).normalized, (lightPosition - vertex).normalized);

                    Color color = _lightSystem.LightColor * Mathf.Max(0f, NdL);
                    color.a = NdL >= 0f ? 1f : .33f;

                    _colors[j + vertexOffset] = color;
                }

                vertexOffset += meshFilter.sharedMesh.vertexCount;
            }
            
            _mesh.vertices = _vertices;
            _mesh.colors = _colors;
            
            _mesh.RecalculateBounds();
            _mesh.UploadMeshData(false);
        }

        private void OnDisable()
        {
            if (_mesh != null)
                Destroy(_mesh);
            
            _meshFilter.sharedMesh = null;
        }
    }
}