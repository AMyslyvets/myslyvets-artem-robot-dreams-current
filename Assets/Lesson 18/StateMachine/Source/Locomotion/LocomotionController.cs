using System;
using UnityEngine;

namespace StateMachineSystem.Locomotion
{
    public class LocomotionController : MonoBehaviour, ISpeedProvider
    {
        public event Action<LocomotionState> OnStateChanged;
        
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private GunAimer _gunAimer;
        [SerializeField] private float _speed;
        [SerializeField] private float _aimSpeed;
        [SerializeField] private float _drag;
        [SerializeField] private Vector2 _jumpSpeed;
        
        private float _currentSpeed;
        
        private StateMachine _stateMachine;

        public float Speed => _currentSpeed;
        
        public string CurrentState => _stateMachine == null ? "[NULL]" : _stateMachine.CurrentState.GetType().Name;
        public LocomotionState LocomotionState => (LocomotionState)_stateMachine.CurrentState.StateId;

        public StateMachine StateMachine => _stateMachine;
        
        public CharacterController CharacterController => _characterController;
        
        private void Start()
        {
            _stateMachine = new StateMachine();
            _stateMachine.AddState((byte)LocomotionState.Idle,
                new IdleState(_stateMachine, (byte)LocomotionState.Idle, _characterController));
            
            _stateMachine.AddState((byte)LocomotionState.Movement,
                new MovementState(_stateMachine, (byte)LocomotionState.Movement, _characterController,
                    _characterController.transform, this));
            
            _stateMachine.AddState((byte)LocomotionState.Fall,
                new FallState(_stateMachine, (byte)LocomotionState.Fall, _characterController, _drag));
            
            _stateMachine.AddState((byte)LocomotionState.Jump,
                new JumpState(_stateMachine, (byte)LocomotionState.Jump, _characterController,
                    _drag, _jumpSpeed.y, _jumpSpeed.x));
            
            _stateMachine.InitState((byte)LocomotionState.Idle);

            _stateMachine.OnStateChange += StateChangeHandler;
        }

        private void FixedUpdate()
        {
            _currentSpeed = Mathf.Lerp(_speed, _aimSpeed, _gunAimer.AimValue);
            
            _stateMachine.Update(Time.fixedDeltaTime);
        }

        private void OnDestroy()
        {
            _stateMachine?.Dispose();
        }

        private void StateChangeHandler(byte stateId)
        {
            OnStateChanged?.Invoke((LocomotionState)stateId);
        }
    }
}