using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Lesson22
{
    public class ZoomController : MonoBehaviour
    {
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private Vector2 _zoomRange;
        [SerializeField] private float _zoomSensetivity;
        [SerializeField] private InputAction _zoomAction;

        private float _zoom;

        private void Start()
        {
            _zoomAction.Enable();
            _zoom = _zoomRange.x;
        }

        private void Update()
        {
            _zoom = Mathf.Clamp(_zoom + _zoomAction.ReadValue<float>() * _zoomSensetivity, _zoomRange.x, _zoomRange.y);
            _cameraTransform.localPosition = new Vector3(0f, 0f, -_zoom);
        }
    }
}