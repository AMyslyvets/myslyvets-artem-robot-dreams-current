using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Animator _animator;

    [SerializeField] private string _horizontalName;
    [SerializeField] private string _verticalName;
    
    [SerializeField] private float _smoothing;

    private int _horizontalId;
    private int _verticalId;

    private void Awake()
    {
        _horizontalId = Animator.StringToHash(_horizontalName);
        _verticalId = Animator.StringToHash(_verticalName);
    }

    private void Update()
    {
        Vector3 moveDirection = _playerController.MoveDirection;
        
        float deltaTime = Time.deltaTime;
        
        _animator.SetFloat(_horizontalId, moveDirection.x, _smoothing, deltaTime);
        _animator.SetFloat(_verticalId, moveDirection.z, _smoothing, deltaTime);
    }
}
