using StateMachineSystem.ServiceLocatorSystem;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachineSystem.Locomotion
{
    public class IdleState : StateBase
    {
        private readonly CharacterController _characterController;
        private Vector3 _localDirection;

        private readonly bool _grounded;

        private readonly InputController _inputController;

        public IdleState(StateMachine stateMachine,
            byte stateId,
            CharacterController characterController) : base(stateMachine, stateId)
        {
            _characterController = characterController;

            conditions = new List<IStateCondition>
            {
                new BaseCondition((byte)LocomotionState.Movement, IsMoving),
                new BaseCondition((byte)LocomotionState.Fall, IsFalling)
            };

            _inputController = ServiceLocator.Instance.GetService<InputController>();

            _inputController.OnMoveInput += MoveHandler;
        }

        public override void Dispose()
        {
            _inputController.OnMoveInput -= MoveHandler;
        }

        protected override void OnUpdate(float deltaTime)
        {
            //_grounded = _characterController.SimpleMove(Vector3.zero);
            _ = _characterController.Move(Physics.gravity * deltaTime);
        }

        private void MoveHandler(Vector2 input)
        {
            _localDirection = new Vector3(input.x, 0, input.y);
        }

        private bool IsMoving()
        {
            return !Mathf.Approximately(_localDirection.sqrMagnitude, 0f);
        }

        private bool IsFalling()
        {
            return !_characterController.isGrounded;
        }
    }
}