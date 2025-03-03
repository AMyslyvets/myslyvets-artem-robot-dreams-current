using System;
using UnityEngine;

namespace LightEquation.Source
{
    public class BlinnPhongGizmos : MonoBehaviour
    {
        [SerializeField] private LightSystem _lightSystem;
        [SerializeField] private EyeGizmos _eyeGizmos;
        [SerializeField] private MeshFilter _lightMeshFilter;
        [SerializeField] private MeshFilter _viewMeshFilter;
        [SerializeField] private MeshFilter _halfWayMeshFilter;
        [SerializeField] private MeshFilter[] _meshFilters;
        [SerializeField] private float _halfWayExtend;
        [SerializeField] private Color _halfWayColor;
        [SerializeField] private float _shininess;

        private Mesh _lightMesh;
        private Mesh _viewMesh;
        private Mesh _halfWayMesh;

        private int _vertexCount;
        
        private Vector3[] _lightVertices;
        private Color[] _lightColors;
        private int[] _lightIndices;
        
        private Vector3[] _viewVertices;
        private Color[] _viewColors;
        private int[] _viewIndices;
        
        private Vector3[] _halfWayVertices;
        private Color[] _halfWayColors;
        private int[] _halfWayIndices;
        
        private void OnEnable()
        {
            if (_lightMesh == null)
            {
                _lightMesh = new Mesh();
                _lightMeshFilter.sharedMesh = _lightMesh;
            }

            _lightMesh.Clear();
            
            if (_viewMesh == null)
            {
                _viewMesh = new Mesh();
                _viewMeshFilter.sharedMesh = _viewMesh;
            }

            _viewMesh.Clear();
            
            if (_halfWayMesh == null)
            {
                _halfWayMesh = new Mesh();
                _halfWayMeshFilter.sharedMesh = _halfWayMesh;
            }

            _halfWayMesh.Clear();
            
            _vertexCount = 1;
            
            for (int i = 0; i < _meshFilters.Length; ++i)
                _vertexCount += _meshFilters[i].sharedMesh.vertexCount;
            
            _lightVertices = new Vector3[_vertexCount];
            _viewVertices = new Vector3[_vertexCount];
            _halfWayVertices = new Vector3[(_vertexCount - 1) * 2];

            Vector3 lightPosition = _lightSystem.transform.position;
            _lightVertices[0] = lightPosition;
            _viewVertices[0] = _eyeGizmos.EyePosition;
            
            _lightColors = new Color[_vertexCount];
            _viewColors = new Color[_vertexCount];
            _halfWayColors = new Color[(_vertexCount - 1) * 2];
            
            Color lightColor = _lightSystem.LightColor;
            lightColor.a = .5f;
            _lightColors[0] = lightColor;
            Color eyeColor = _eyeGizmos.EyeColor;
            eyeColor.a = .5f;
            _viewColors[0] = eyeColor;
            
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
                    int vertexIndex = vertexOffset + j;
                    
                    Vector3 vertex = matrix.MultiplyPoint(vertices[j]);
                    
                    _lightVertices[vertexIndex] = vertex;
                    _viewVertices[vertexIndex] = vertex;
                    
                    Vector3 normal = normalMatrix.MultiplyVector(normals[j]).normalized;

                    Vector3 viewVector = (_eyeGizmos.EyePosition - vertex).normalized;
                    Vector3 lightVector = (lightPosition - vertex).normalized;
                    Vector3 halfWay = (viewVector + lightVector).normalized;

                    float specular = Mathf.Pow(Mathf.Max(Vector3.Dot(halfWay, normal), 0f), _shininess);

                    Color color = _lightSystem.LightColor * specular;
                    color.a = specular >= 0f ? .5f : .125f;
                    eyeColor.a = color.a;

                    _lightColors[vertexIndex] = color;
                    _viewColors[vertexIndex] = eyeColor;
                    
                    _halfWayVertices[vertexIndex - 1] = vertex;
                    _halfWayVertices[(_vertexCount - 1) + (vertexIndex - 1)] = vertex + halfWay * _halfWayExtend;
                    
                    _halfWayColors[vertexIndex - 1] = _halfWayColor;
                    _halfWayColors[(_vertexCount - 1) + (vertexIndex - 1)] = _halfWayColor;
                }

                vertexOffset += meshFilter.sharedMesh.vertexCount;
            }
            
            _lightMesh.vertices = _lightVertices;
            _lightMesh.colors = _lightColors;
            
            _viewMesh.vertices = _viewVertices;
            _viewMesh.colors = _viewColors;
            
            _halfWayMesh.vertices = _halfWayVertices;
            _halfWayMesh.colors = _halfWayColors;
            
            _lightIndices = new int[(_vertexCount - 1) * 2];

            for (int i = 1; i < _vertexCount; ++i)
            {
                _lightIndices[(i - 1) * 2] = 0;
                _lightIndices[(i - 1) * 2 + 1] = i;
            }
            
            _lightMesh.SetIndices(_lightIndices, MeshTopology.Lines, 0);
            
            _lightMesh.RecalculateBounds();
            _lightMesh.UploadMeshData(false);
            
            _viewIndices = new int[(_vertexCount - 1) * 2];

            for (int i = 1; i < _vertexCount; ++i)
            {
                _viewIndices[(i - 1) * 2] = 0;
                _viewIndices[(i - 1) * 2 + 1] = i;
            }
            
            _viewMesh.SetIndices(_viewIndices, MeshTopology.Lines, 0);
            
            _viewMesh.RecalculateBounds();
            _viewMesh.UploadMeshData(false);
            
            _halfWayIndices = new int[(_vertexCount - 1) * 2];

            for (int i = 0; i < (_vertexCount - 1); ++i)
            {
                _halfWayIndices[i * 2] = i;
                _halfWayIndices[i * 2 + 1] = (_vertexCount - 1) + i;
            }
            
            _halfWayMesh.SetIndices(_halfWayIndices, MeshTopology.Lines, 0);
            
            _halfWayMesh.RecalculateBounds();
            _halfWayMesh.UploadMeshData(false);
        }

        private void Update()
        {
            Vector3 lightPosition = _lightSystem.transform.position;
            _lightVertices[0] = lightPosition;
            _viewVertices[0] = _eyeGizmos.EyePosition;

            Color lightColor = _lightSystem.LightColor;
            lightColor.a = .001f;
            _lightColors[0] = lightColor;
            Color eyeColor = _eyeGizmos.EyeColor;
            eyeColor.a = .001f;
            _viewColors[0] = eyeColor;
            
            
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
                    int vertexIndex = vertexOffset + j;
                    
                    Vector3 vertex = matrix.MultiplyPoint(vertices[j]);
                    
                    _lightVertices[vertexIndex] = vertex;
                    _viewVertices[vertexIndex] = vertex;
                    
                    Vector3 normal = normalMatrix.MultiplyVector(normals[j]).normalized;

                    Vector3 viewVector = (_eyeGizmos.EyePosition - vertex).normalized;
                    Vector3 lightVector = (lightPosition - vertex).normalized;
                    Vector3 halfWay = (viewVector + lightVector).normalized;

                    float specular = Mathf.Pow(Mathf.Max(Vector3.Dot(halfWay, normal), 0f), _shininess);

                    Color color = _lightSystem.LightColor * specular;
                    //color.a = specular >= 0f ? .5f : .125f;
                    eyeColor.a = color.a;

                    _lightColors[vertexIndex] = color;
                    _viewColors[vertexIndex] = eyeColor;
                    
                    _halfWayVertices[vertexIndex - 1] = vertex;
                    _halfWayVertices[(_vertexCount - 1) + (vertexIndex - 1)] = vertex + halfWay * _halfWayExtend;
                    
                    _halfWayColors[vertexIndex - 1] = _halfWayColor;
                    _halfWayColors[(_vertexCount - 1) + (vertexIndex - 1)] = _halfWayColor;
                }

                vertexOffset += meshFilter.sharedMesh.vertexCount;
            }
            
            _lightMesh.vertices = _lightVertices;
            _lightMesh.colors = _lightColors;
            
            _lightMesh.RecalculateBounds();
            _lightMesh.UploadMeshData(false);
            
            _viewMesh.vertices = _viewVertices;
            _viewMesh.colors = _viewColors;
            
            _viewMesh.RecalculateBounds();
            _viewMesh.UploadMeshData(false);
            
            _halfWayMesh.vertices = _halfWayVertices;
            _halfWayMesh.colors = _halfWayColors;
            
            _halfWayMesh.RecalculateBounds();
            _halfWayMesh.UploadMeshData(false);
        }

        private void OnDisable()
        {
            if (_lightMesh != null)
                Destroy(_lightMesh);
            if (_viewMesh != null)
                Destroy(_viewMesh);
            if (_halfWayMesh != null)
                Destroy(_halfWayMesh);
            
            _lightMeshFilter.sharedMesh = null;
            _viewMeshFilter.sharedMesh = null;
            _halfWayMeshFilter.sharedMesh = null;
        }
    }
}