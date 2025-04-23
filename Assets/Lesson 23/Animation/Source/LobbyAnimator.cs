using System;
using DefendFlag;
using Inventory.Lobby;
using StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;

namespace Animation
{
    public class LobbyAnimator : MonoBehaviour
    {
        [SerializeField] private LobbyInteractor _interactor;
        [SerializeField] private Animator _animator;
        [SerializeField] private string _horizontalName;
        [SerializeField] private string _verticalName;
        [SerializeField] private string _interactTriggerName;
        [SerializeField] private string _interactTypeName;
        [SerializeField] private float _dampTime;

        private int _horizontalId;
        private int _verticalId;
        private int _interactTriggerId;
        private int _interactTypeId;

        private StateMachineSystem.InputController _inputController;
        
        private void Awake()
        {
            _horizontalId = Animator.StringToHash(_horizontalName);
            _verticalId = Animator.StringToHash(_verticalName);
            _interactTriggerId = Animator.StringToHash(_interactTriggerName);
            _interactTypeId = Animator.StringToHash(_interactTypeName);

            _interactor.OnInteract += InteractHandler;
        }

        private void Start()
        {
            _inputController = ServiceLocator.Instance.GetService<StateMachineSystem.InputController>();
        }

        private void FixedUpdate()
        {
            Vector2 moveInput = _inputController.MoveAction.ReadValue<Vector2>();
            
            _animator.SetFloat(_horizontalId, moveInput.x, _dampTime, Time.fixedDeltaTime);
            _animator.SetFloat(_verticalId, moveInput.y, _dampTime, Time.fixedDeltaTime);
        }

        private void InteractHandler(IInteractable interactable)
        {
            _animator.SetInteger(_interactTypeId, (int)interactable.Type);
            _animator.SetTrigger(_interactTriggerId);
        }
    }
}