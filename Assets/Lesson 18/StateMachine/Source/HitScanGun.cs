using System;
using System.Collections;
using Shooting;
using StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;

namespace StateMachineSystem
{
    public class HitScanGun : HitScanGunBase
    {
        [SerializeField] protected GunAimer _aimer;
        [SerializeField] protected HitscanShotAspect _shotPrefab;
        [SerializeField] protected Transform _muzzleTransform;
        [SerializeField] protected float _decaySpeed;
        [SerializeField] protected Vector3 _shotScale;
        [SerializeField] protected float _shotRadius;
        [SerializeField] protected float _shotVisualDiameter;
        [SerializeField] protected string _tilingName;
        [SerializeField] protected float _range;
        [SerializeField] protected LayerMask _layerMask;

        protected int _tilingId;

        protected InputController _inputController;
        
        protected virtual void Start()
        {
            _tilingId = Shader.PropertyToID(_tilingName);
        }

        protected void OnEnable()
        {
            if (_inputController == null)
                _inputController = ServiceLocator.Instance.GetService<InputController>();
            _inputController.OnPrimaryInput += PrimaryInputHandler;
        }

        protected void OnDisable()
        {
            _inputController.OnPrimaryInput -= PrimaryInputHandler;
        }

        public override void Shoot()
        {
            Vector3 muzzlePosition = _muzzleTransform.position;
            Vector3 muzzleForward = _muzzleTransform.forward;
            Ray ray = new Ray(muzzlePosition, muzzleForward);
            Vector3 hitPoint = muzzlePosition + muzzleForward * _range;
            if (Physics.SphereCast(ray, _shotRadius, out RaycastHit hitInfo, _range, _layerMask))
            {
                Vector3 directVector = hitInfo.point - _muzzleTransform.position;
                Vector3 rayVector = Vector3.Project(directVector, ray.direction);
                hitPoint = muzzlePosition + rayVector;
                
                InvokeHit(hitInfo.collider);
                InvokeHitPrecise(hitInfo);
            }

            HitscanShotAspect shot = Instantiate(_shotPrefab, hitPoint, _muzzleTransform.rotation);
            shot.distance = (hitPoint - _muzzleTransform.position).magnitude;
            shot.outerPropertyBlock = new MaterialPropertyBlock();
            StartCoroutine(ShotRoutine(shot));
            
            InvokeShot();
        }
        
        protected virtual void PrimaryInputHandler()
        {
            Shoot();
        }

        protected IEnumerator ShotRoutine(HitscanShotAspect shot)
        {
            float interval = _decaySpeed * Time.deltaTime;
            while (shot.distance >= interval)
            {
                EvaluateShot(shot);
                yield return null;
                shot.distance -= interval;
                interval = _decaySpeed * Time.deltaTime;
            }

            Destroy(shot.gameObject);
        }

        protected void EvaluateShot(HitscanShotAspect shot)
        {
            shot.Transform.localScale = new Vector3(_shotScale.x, _shotScale.y, shot.distance * 0.5f);
            Vector4 tiling = Vector4.one;
            tiling.y = shot.distance * 0.5f / _shotVisualDiameter;
            shot.outerPropertyBlock.SetVector(_tilingId, tiling);
            shot.Outer.SetPropertyBlock(shot.outerPropertyBlock);
        }
    }
}