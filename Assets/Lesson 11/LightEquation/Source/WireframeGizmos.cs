using System;
using UnityEngine;

namespace LightEquation.Source
{
    public class WireframeGizmos : MonoBehaviour
    {
        [SerializeField] private MeshFilter _originMeshFilter;
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private float _push;
        [SerializeField] private float _normalsExtend;

        private Mesh _mesh;
        private Vector3[] _vertices;
        private Color[] _colors;
        private int[] _wireframeIndices;
        private int[] _normalIndices;
        
        private void OnEnable()
        {
            Mesh originMesh = _originMeshFilter.sharedMesh;

            if (_mesh == null)
            {
                _mesh = new Mesh();
                _meshFilter.sharedMesh = _mesh;
            }

            _mesh.Clear(false);
            _mesh.subMeshCount = 2;

            //_vertices = new Vector3[originMesh.vertexCount * 2];
            
            int originVertexCount = originMesh.vertexCount;
            
            Vector3[] originVertices = originMesh.vertices;
            _vertices = new Vector3[originMesh.vertexCount * 2];
            _colors = new Color[_vertices.Length];

            Vector3[] originNormals = originMesh.normals;
            Vector3[] normals = new Vector3[_vertices.Length];
            
            for (int i = 0; i < originNormals.Length; ++i)
                normals[i] = originNormals[i];

            for (int i = 0; i < originVertexCount; ++i)
            {
                Vector3 normal = normals[i];
                _vertices[i] = originVertices[i] + normal * _push;
                normal = (normal + Vector3.one) * .5f;
                _colors[i] = new Color(normal.x, normal.y, normal.z, 1f);
            }

            for (int i = 0; i < originVertexCount; ++i)
            {
                Vector3 normal = normals[i];
                _vertices[i + originVertexCount] = originVertices[i] + normal * _normalsExtend;
                normal = (normal + Vector3.one) * .5f;
                _colors[i + originVertexCount] = new Color(normal.x, normal.y, normal.z, 1f);
            }

            _mesh.vertices = _vertices;
            _mesh.normals = normals;
            _mesh.colors = _colors;
            
            int[] triangles = originMesh.triangles;

            _wireframeIndices = new int[triangles.Length * 2];
            _normalIndices = new int[originVertexCount * 2];
            
            int wireframeIndexCount = triangles.Length;
            int triangleCount = wireframeIndexCount / 3;

            for (int i = 0; i < triangleCount; ++i)
            {
                _wireframeIndices[i * 6] = triangles[i * 3];
                _wireframeIndices[i * 6 + 1] = triangles[i * 3 + 1];
                _wireframeIndices[i * 6 + 2] = triangles[i * 3 + 1];
                _wireframeIndices[i * 6 + 3] = triangles[i * 3 + 2];
                _wireframeIndices[i * 6 + 4] = triangles[i * 3 + 2];
                _wireframeIndices[i * 6 + 5] = triangles[i * 3];
            }
            
            for (int i = 0; i < originVertexCount; ++i)
            {
                _normalIndices[i * 2] = i;
                _normalIndices[i * 2 + 1] = i + originVertexCount;
            }
            
            _mesh.SetIndices(_wireframeIndices, MeshTopology.Lines, 0);
            _mesh.SetIndices(_normalIndices, MeshTopology.Lines, 1);
            
            _mesh.RecalculateBounds();
            _mesh.UploadMeshData(false);
        }

        private void OnDisable()
        {
            if (_mesh != null)
                Destroy(_mesh);
            
            _meshFilter.sharedMesh = null;
        }

        private void Update()
        {
            
        }
    }
}