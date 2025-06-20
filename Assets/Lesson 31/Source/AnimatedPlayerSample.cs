using UnityEngine;
using UnityEngine.InputSystem;

public class AnimatedPlayerSample : MonoBehaviour
{
    [SerializeField] InputActionProperty _moveAction;
    [SerializeField] InputActionProperty _lookAction;

    [SerializeField] CharacterController _characterController;
    [SerializeField] Transform _cameraYawAnchor;
    [SerializeField] Transform _cameraPitchAnchor;

    [SerializeField] private float _speed;
    [SerializeField] private Vector2 _sensitivity;

    [SerializeField] private Animator _animator;
    [SerializeField] private string _speedName;
    [SerializeField] private string _horizontalName;
    [SerializeField] private string _verticalName;
    [SerializeField] private float _dampTime;

    private float _yaw;
    private float _pitch;

    private int _speedId;
    private int _horizontalId;
    private int _verticalId;

    private Transform _characterTransform;

    private void Start()
    {
        _moveAction.action.Enable();
        _lookAction.action.Enable();

        _characterTransform = _characterController.transform;

        _speedId = Animator.StringToHash(_speedName);
        _horizontalId = Animator.StringToHash(_horizontalName);
        _verticalId = Animator.StringToHash(_verticalName);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Vector2 input = _moveAction.action.ReadValue<Vector2>();

        Matrix4x4 characterMatrix = _characterTransform.localToWorldMatrix;

        Vector3 characterForward = characterMatrix.MultiplyVector(Vector3.forward);
        Vector3 characterRight = characterMatrix.MultiplyVector(Vector3.right);

        Vector3 velocity = (characterForward * input.y + characterRight * input.x) * _speed;

        _characterController.Move((velocity + Physics.gravity) * Time.fixedDeltaTime);

        _animator.SetFloat(_speedId, input.magnitude, _dampTime, Time.fixedDeltaTime);
        _animator.SetFloat(_horizontalId, input.x, _dampTime, Time.fixedDeltaTime);
        _animator.SetFloat(_verticalId, input.y, _dampTime, Time.fixedDeltaTime);
    }

    private void LateUpdate()
    {
        Vector2 lookInput = _lookAction.action.ReadValue<Vector2>();
        _yaw += lookInput.x * _sensitivity.x;
        _pitch += lookInput.y * _sensitivity.y;

        _cameraYawAnchor.localRotation = Quaternion.Euler(0f, _yaw, 0f);
        _cameraPitchAnchor.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
    }
}
