using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Inventory.VisualEffects
{
    public class CCTVCamera : MonoBehaviour
    {
        [SerializeField] private Transform _camera;
        [SerializeField] private Vector2 _rotationBounds;
        [SerializeField] private float _speed;
        
        private Quaternion _minRotation;
        private Quaternion _maxRotation;

        private float _time;
        private float _signal;

        private void Start()
        {
            _time = Random.Range(0f, 1f / _speed);
            
            _minRotation = Quaternion.Euler(0f, _rotationBounds.x, 0f);
            _maxRotation = Quaternion.Euler(0f, _rotationBounds.y, 0f);
        }

        private void Update()
        {
            _signal = Mathf.PingPong(_time, 1f);
            _camera.localRotation = Quaternion.Slerp(_minRotation, _maxRotation, _signal);
            _time += Time.deltaTime * _speed;
        }
    }
}