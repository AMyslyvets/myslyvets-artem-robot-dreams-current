using System;
using LightEquation.Source;
using UnityEngine;

[ExecuteAlways]
public class LightSystem : MonoBehaviour
{
    [SerializeField] private EyeGizmos _eyeGizmos;
    
    [Header("Shader properties"), SerializeField] private string _lightPositionName;
    [SerializeField] private string _lightDirectionName;
    [SerializeField] private string _lightColorName;
    [SerializeField] private string _ambientName;
    [SerializeField] private string _viewPositionName;
    
    [Space, Header("Light settings"), SerializeField] private Color _lightColor;
    [SerializeField] private float _lightIntensity;
    [SerializeField] private Color _ambientColor;
    
    private int _lightPositionId;
    private int _lightDirectionId;
    private int _lightColorId;
    private int _ambientId;
    private int _viewPositionId;
    
    private Vector3 _lightPosition;
    private Transform _transform;
    
    public Color LightColor => _lightColor;
    
    public Vector3 LightPosition => _lightPosition;
    
    private void OnEnable()
    {
        _lightPositionId = Shader.PropertyToID(_lightPositionName);
        _lightDirectionId = Shader.PropertyToID(_lightDirectionName);
        _lightColorId = Shader.PropertyToID(_lightColorName);
        _ambientId = Shader.PropertyToID(_ambientName);
        _viewPositionId = Shader.PropertyToID(_viewPositionName);
        
        _transform = transform;
    }

    private void Update()
    {
        _lightPosition = _transform.position;
        
        Shader.SetGlobalVector(_lightPositionId, _lightPosition);
        Shader.SetGlobalVector(_lightDirectionId, _transform.forward);
        Shader.SetGlobalVector(_lightColorId, new Vector4(_lightColor.r, _lightColor.g, _lightColor.b, _lightIntensity));
        Shader.SetGlobalVector(_ambientId, _ambientColor);
        Shader.SetGlobalVector(_viewPositionId, _eyeGizmos.EyePosition);
    }
}
