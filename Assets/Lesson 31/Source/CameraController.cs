using UnityEngine;
using UnityEngine.InputSystem;

namespace Lesson32
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private InputAction _moveCamera;
        [SerializeField] private Transform _cursorTransform;
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private float _sendetivity;
        [SerializeField] private float _offset;

        private void Start()
        {
            _moveCamera.Enable();
        }

        private void LateUpdate()
        {
            Vector3 translation;
            Vector2 input = _moveCamera.ReadValue<Vector2>();
            translation = new Vector3(input.x, 0f, input.y) * _sendetivity;
            _cursorTransform.position += translation * Time.deltaTime;

            translation = _cursorTransform.position - _playerTransform.position;
            translation.y = 0f;
            translation.Normalize();

            _cameraTransform.position = _playerTransform.position + translation * _offset + Vector3.up * 10f;
        }
    }
}