using System;
using MainMenu;
using StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;

namespace DefendFlag
{
    public class StimsController : StimsControllerBase
    {
        [SerializeField] private int _maxStims;
        [SerializeField] private int _healing;
        [SerializeField] private Health _health;

        private int _currentStims;
        private StateMachineSystem.InputController _inputController;
        
        public override int MaxStims => _maxStims;
        public override int CurrentStims => _currentStims;
        
        private void Start()
        {
            _inputController = ServiceLocator.Instance.GetService<StateMachineSystem.InputController>();
            _inputController.OnHeal += HealHandler;
            _currentStims = _maxStims;
        }

        private void HealHandler()
        {
            if (_currentStims <= 0)
                return;
            _currentStims--;
            
            _health.Heal(_healing);
            
            InvokeOnStimUsed();
        }

        public override void AddStims(int amount)
        {
            _currentStims = Mathf.Clamp(_currentStims + amount, 0, _maxStims);
            InvokeOnStimUsed();
        }
    }
}