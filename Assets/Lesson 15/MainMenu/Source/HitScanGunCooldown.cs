using System;
using System.Collections;
using Shooting;
using UnityEngine;

namespace MainMenu
{
    public class HitScanGunCooldown : HitScanGun
    {
        public event Action<int> OnChargeChanged;
        public event Action<bool> OnReload;
        
        [SerializeField] private float _cooldownTime;
        [SerializeField] private int _maxCharge;
        [SerializeField] private int _chargePerShot;
        [SerializeField] private float _reloadTime;
        
        private Cooldown _cooldown;
        private Cooldown _reload;

        private int _currentCharge;

        public int MaxCharge => _maxCharge;
        public int CurrentCharge => _currentCharge;
        
        public Cooldown Cooldown => _cooldown;
        public Cooldown Reload => _reload;
        
        private void Awake()
        {
            _cooldown = new Cooldown(_cooldownTime);
            _reload = new Cooldown(_reloadTime);
            _currentCharge = _maxCharge;
        }
        
        protected override void Start()
        {
            PhysX.InputController.OnReload += ReloadHandler;
            
            base.Start();
        }

        protected override void PrimaryInputHandler()
        {
            if (_cooldown.IsOnCooldown || _reload.IsOnCooldown)
                return;
            base.PrimaryInputHandler();
            StartCoroutine(_cooldown.Begin());
            _currentCharge -= _chargePerShot;
            if (_currentCharge <= 0)
            {
                _currentCharge = 0;
                StartCoroutine(ReloadRoutine());
            }
            OnChargeChanged?.Invoke(_currentCharge);
        }

        private IEnumerator ReloadRoutine()
        {
            OnReload?.Invoke(true);
            yield return _reload.Begin();
            _currentCharge = _maxCharge;
            OnReload?.Invoke(false);
            OnChargeChanged?.Invoke(_currentCharge);
        }

        private void ReloadHandler()
        {
            if(_currentCharge == _maxCharge)
                return;
            StartCoroutine(ReloadRoutine());
        }
    }
}