using PhysX;
using StateMachineSystem;
using UnityEngine;
using HitScanGun = BehaviourTreeSystem.HitScanGun;

namespace DefendFlag
{
    public class TurretController : MonoBehaviour
    {
        public enum State
        {
            Aiming,
            Cooldown,
            Reload,
        }
        
        [SerializeField] private TurretAnimator _turretAnimator;
        [SerializeField] private TurretTargeter _turretTargeter;
        [SerializeField] private WeaponData _weaponData;
        [SerializeField] private HitScanGun _gun;
        [SerializeField] private float _aimTime;

        private ITargetable _target;
        private State _state;
        
        private float _time;
        private int _charge;
        
        private void Awake()
        {
            enabled = false;
            _turretTargeter.Disable();
        }

        private void OnEnable()
        {
            _turretTargeter.Enable();
            StartCoroutine(_turretAnimator.Open());
        }

        private void OnDisable()
        {
            _turretTargeter.Disable();
        }

        private void FixedUpdate()
        {
            if (!_turretTargeter.HasTarget(_target))
                _target = null;
            
            if (_target == null)
            {
                _target = _turretTargeter.GetTarget();
            }

            if (_target == null)
                return;

            switch (_state)
            {
                case State.Aiming:
                    AimingUpdate(Time.fixedDeltaTime);
                    break;
                case State.Cooldown:
                    CooldownUpdate(Time.fixedDeltaTime);
                    break;
                case State.Reload:
                    ReloadUpdate(Time.fixedDeltaTime);
                    break;
            }
        }

        private void AimingUpdate(float deltaTime)
        {
            _time += deltaTime;
            if (_time < _aimTime)
                return;
            
            _charge = _weaponData.MaxCharge;
            Shoot();
        }

        private void CooldownUpdate(float deltaTime)
        {
            _time += deltaTime;
            if (_time < _weaponData.CooldownTime)
                return;
            Shoot();
        }

        private void ReloadUpdate(float deltaTime)
        {
            _time += deltaTime;
            if (_time < _weaponData.ReloadTime)
                return;
            _charge = _weaponData.MaxCharge;
            Shoot();
        }
        
        private void Shoot()
        {
            _gun.Shoot();
            _charge -= _weaponData.ChargePerShot;
            _state = _charge > 0 ? State.Cooldown : State.Reload;
            _time = 0f;
        }
        
        private void Update()
        {
            if (_target == null || _target.TargetPivot == null)
                return;
            _turretAnimator.Aim(_target.TargetPivot.position, Time.deltaTime);
        }
    }
}