using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private InputController _inputController;
    [SerializeField] private Transform _yawAnchor;
    [SerializeField] private Transform _pitchAnchor;
    [SerializeField] private Vector2 _sensetivity;

    private float _yaw;
    private float _pitch;

    private Vector2 _lookInput;
    
    private void Awake()
    {
        _yaw = _yawAnchor.localEulerAngles.y;
        _pitch = _pitchAnchor.localEulerAngles.x;

        _inputController.OnLook += LookHandler;
    }

    private void LateUpdate()
    {
        _yaw += _sensetivity.x * _lookInput.x;
        _yawAnchor.localRotation = Quaternion.Euler(0f, _yaw, 0f);
        
        _pitch += _sensetivity.y * _lookInput.y;
        _pitchAnchor.localRotation = Quaternion.Euler(_pitch, 0f, 0f);

        _lookInput = Vector2.zero;
    }

    private void LookHandler(Vector2 lookInput)
    {
        _lookInput = lookInput;
    }
}
