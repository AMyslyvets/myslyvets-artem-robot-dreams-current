using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Inventory.VisualEffects
{
    public class ColorBlinker : MonoBehaviour
    {
        private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
        [SerializeField] private Vector2 _alphaBounds;
        [SerializeField] private float _blinkPeriod;
        [SerializeField] private Color _baseColor;

        [SerializeField] private Renderer[] _renderers;

        private float _startAlpha;
        private float _alpha;
        private float _targetAlpha;
        
        private float _time;
        private float _reciprocal;

        private void Start()
        {
            _startAlpha = _alpha = Random.Range(_alphaBounds.x, _alphaBounds.y);
            _targetAlpha = Random.Range(_alphaBounds.x, _alphaBounds.y);
            
            _reciprocal = 1f / _blinkPeriod;
            
            ApplyAlpha();
        }

        private void Update()
        {
            _time += Time.deltaTime;
            _alpha = Mathf.Lerp(_startAlpha, _targetAlpha, _time * _reciprocal);
            if (_time >= _blinkPeriod)
            {
                _startAlpha = _targetAlpha;
                _targetAlpha = Random.Range(_alphaBounds.x, _alphaBounds.y);
                _time = 0f;
            }
            
            ApplyAlpha();
        }

        private void ApplyAlpha()
        {
            Color color = _baseColor;
            color.a = _alpha;
            
            for (int i = 0; i < _renderers.Length; ++i)
            {
                _renderers[i].material.SetColor(BaseColor, color);
            }
        }
    }
}