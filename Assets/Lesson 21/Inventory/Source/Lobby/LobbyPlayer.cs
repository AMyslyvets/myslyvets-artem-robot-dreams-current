using StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;

namespace Inventory.Lobby
{
    public class LobbyPlayer : MonoBehaviour
    {
        [SerializeField] private CharacterController _controller;
        [SerializeField] private Transform _transform;
        /*[SerializeField] private InputAction _horizontal;
        [SerializeField] private InputAction _vertical;*/
        [SerializeField] private float _speed;

        private StateMachineSystem.InputController _inputController;
        
        private void Start()
        {
            _inputController = ServiceLocator.Instance.GetService<StateMachineSystem.InputController>();
            
            /*_horizontal.Enable();
            _vertical.Enable();*/
        }
        
        private void FixedUpdate()
        {
            Vector2 input = _inputController.MoveAction.ReadValue<Vector2>();
            
            Vector3 direction = Vector3.zero;
            direction.x = input.x;
            direction.z = input.y;
            direction.Normalize();
            
            Vector3 move = direction.z * _transform.forward + direction.x * _transform.right;
            _controller.SimpleMove(move * _speed);
        }
    }
}