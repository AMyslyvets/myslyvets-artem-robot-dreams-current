using System;
using UnityEngine;

namespace LightEquation.Source
{
    [ExecuteAlways]
    public class EyeGizmos : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour _lightRayGizmos;
        [SerializeField] private LightSystem _lightSystem;
        [SerializeField] private NormalController _normalController;
        [SerializeField] private Transform _eyeTransform;
        [SerializeField] private MeshFilter _fullMeshFilter;
        [SerializeField] private MeshRenderer _fullMeshRenderer;
        [SerializeField] private MeshFilter _viewRayMeshFilter;
        [SerializeField] private MeshRenderer _viewRayMeshRenderer;
        [SerializeField] private Color _eyeColor;
        [SerializeField] private float _range;
        [SerializeField] private float _normalExtend;
        [SerializeField] private bool _showBlinnPhong;
        [SerializeField] private float _halfWayExtend;
        [SerializeField] private Color _halfWayColor;
        [SerializeField] private float _shininess;
        
        private Mesh _fullMesh;
        private Mesh _viewRayMesh;
        
        private Vector3[] _fullVertices;
        private Color[] _fullColors;
        private int[] _fullIndices;
        
        private Vector3[] _viewRayVertices;
        private Color[] _viewRayColors;
        private int[] _viewRayIndices;
        
        public Vector3 EyePosition => _eyeTransform.position;
        public Color EyeColor => _eyeColor;
        
        private void OnEnable()
        {
            if (_fullMesh == null)
            {
                _fullMesh = new Mesh();
                _fullMeshFilter.sharedMesh = _fullMesh;
            }

            _fullMesh.Clear();
            
            if (_viewRayMesh == null)
            {
                _viewRayMesh = new Mesh();
                _viewRayMeshFilter.sharedMesh = _viewRayMesh;
            }

            _viewRayMesh.Clear();

            _fullVertices = new Vector3[8];
            _fullColors = new Color[8];
            _fullIndices = new int[8];

            _fullColors[2] = _eyeColor;
            _fullColors[3] = _eyeColor;
            _fullColors[6] = _halfWayColor;
            _fullColors[7] = _halfWayColor;
            
            _fullMesh.vertices = _fullVertices;
            _fullMesh.colors = _fullColors;

            _fullIndices[0] = 0;
            _fullIndices[1] = 1;
            _fullIndices[2] = 2;
            _fullIndices[3] = 3;
            _fullIndices[4] = 4;
            _fullIndices[5] = 5;
            _fullIndices[6] = 6;
            _fullIndices[7] = 7;
            
            _fullMesh.SetIndices(_fullIndices, MeshTopology.Lines, 0);
            
            _fullMesh.RecalculateBounds();
            _fullMesh.UploadMeshData(false);
            
            _viewRayVertices = new Vector3[2];
            _viewRayColors = new Color[2];
            _viewRayIndices = new int[2];
            
            _viewRayColors[0] = _eyeColor;
            _viewRayColors[1] = _eyeColor;
            
            _viewRayMesh.vertices = _viewRayVertices;
            _viewRayMesh.colors = _viewRayColors;

            _viewRayIndices[0] = 0;
            _viewRayIndices[1] = 1;
            
            _viewRayMesh.SetIndices(_viewRayIndices, MeshTopology.Lines, 0);
            
            _viewRayMesh.RecalculateBounds();
            _viewRayMesh.UploadMeshData(false);

            _fullMeshRenderer.enabled = _viewRayMeshRenderer.enabled = false;

            _lightRayGizmos.enabled = false;
            _normalController.enabled = false;
        }

        private void Update()
        {
            Ray ray = new Ray(_eyeTransform.position, _eyeTransform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, _range))
            {
                _fullMeshRenderer.enabled = true;
                _viewRayMeshRenderer.enabled = false;

                Vector3 hitPoint = hit.point;
                Vector3 normal = hit.normal;
                
                _fullVertices[0] = _lightSystem.LightPosition;
                _fullVertices[1] = hitPoint;
                _fullVertices[2] = hitPoint;
                _fullVertices[3] = _eyeTransform.position;
                _fullVertices[4] = hitPoint;
                _fullVertices[5] = hitPoint + normal * _normalExtend;

                Vector3 viewVector = (_eyeTransform.position - hitPoint).normalized;
                Vector3 lightVector = (_lightSystem.LightPosition - hitPoint).normalized;

                Color lightColor = Color.black;
                
                if (_showBlinnPhong)
                {
                    Vector3 halfWay = (viewVector + lightVector).normalized;
                    
                    _fullVertices[6] = hitPoint;
                    _fullVertices[7] = hitPoint + halfWay * _halfWayExtend;
                    
                    float specular = Mathf.Pow(Mathf.Max(Vector3.Dot(halfWay, normal), 0f), _shininess);

                    lightColor = _lightSystem.LightColor * specular;
                }
                else
                {
                    _fullVertices[6] = hitPoint;
                    _fullVertices[7] = hitPoint;
                    
                    lightColor = _lightSystem.LightColor * Mathf.Max(0f, 
                        Vector3.Dot(normal, (_lightSystem.LightPosition - hitPoint).normalized));
                }
                
                _fullColors[0] = _lightSystem.LightColor;
                lightColor.a = 1f;
                _fullColors[1] = lightColor;
                
                normal = (normal + Vector3.one) *.5f;
                
                Color normalColor = new Color(normal.x, normal.y, normal.z, 1f);
                _fullColors[4] = normalColor;
                _fullColors[5] = normalColor;
                
                _fullMesh.vertices = _fullVertices;
                _fullMesh.colors = _fullColors;
            
                _fullMesh.RecalculateBounds();
                _fullMesh.UploadMeshData(false);
                
                return;
            }

            _fullMeshRenderer.enabled = false;
            _viewRayMeshRenderer.enabled = true;

            _viewRayVertices[0] = _eyeTransform.position;
            _viewRayVertices[1] = _eyeTransform.position + _eyeTransform.forward * _range;
            
            _viewRayMesh.vertices = _viewRayVertices;
            
            _viewRayMesh.RecalculateBounds();
            _viewRayMesh.UploadMeshData(false);
        }

        private void OnDisable()
        {
            if (_fullMesh != null)
                Destroy(_fullMesh);
            
            if (_viewRayMesh != null)
                Destroy(_viewRayMesh);
            
            _fullMeshRenderer.enabled = _viewRayMeshRenderer.enabled = false;
            
            _lightRayGizmos.enabled = true;
            _normalController.enabled = true;
            
            _fullMeshFilter.sharedMesh = null;
            _viewRayMeshFilter.sharedMesh = null;
        }
    }
}