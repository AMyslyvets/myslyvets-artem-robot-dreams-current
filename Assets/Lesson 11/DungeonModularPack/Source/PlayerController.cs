using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputController _inputController;
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private float _moveSpeed;

    public Vector3 MoveDirection { get; private set; }
    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    private void Start()
    {
        _inputController.OnMove += MoveHandler;
    }

    private void FixedUpdate()
    {
        Vector3 moveDirection = _transform.forward * MoveDirection.z + _transform.right * MoveDirection.x; 
        _characterController.SimpleMove(moveDirection * _moveSpeed);
    }

    private void MoveHandler(Vector2 moveInput)
    {
        MoveDirection = new Vector3(moveInput.x, 0f, moveInput.y);
    }
}
