using System;
using StateMachineSystem;
using StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Animation
{
    public class GrenadeDamageDealer : MonoBehaviour
    {
        public event Action<int> OnHit;

        [SerializeField] private int _raysCount;
        [SerializeField] private GrenadeSpawnerBase _grenadeAction;
        [SerializeField] private int _damage;
        [SerializeField] private int _rayDamage;
        [SerializeField] private bool _breakOnHit;
        [SerializeField] private LayerMask _raysMask;
        [SerializeField] private float _raysRange;

        private IHealthService _healthService;
        
        public GrenadeSpawnerBase GrenadeAction => _grenadeAction;
        
        private void Start()
        {
            _healthService = ServiceLocator.Instance.GetService<IHealthService>();
            
            _grenadeAction.OnGrenadeSpawned += GrenadeSpawnedHandler;
        }

        private void GrenadeSpawnedHandler(Grenade grenade)
        {
            grenade.OnExplosionHit += colliders => GrenadeExplosionHitHandler(grenade, colliders);
        }

        private void GrenadeExplosionHitHandler(Grenade grenade, Collider[] colliders)
        {
            if (_breakOnHit)
                Debug.Break();
            
            int hitCount = 0;
            for (int i = 0; i < colliders.Length; ++i)
            {
                Collider collider = colliders[i];
                if (_healthService.GetHitCollider(collider, out IHitCollider hitCollider))
                {
                    Vector3 direction = collider.transform.position - grenade.Position;
                    float falloff = Mathf.Clamp01(1f - direction.magnitude / grenade.ExplosionRadius);
                    hitCollider.TakeDamage((int)(_damage * falloff));
                    hitCount++;
                }
            }

            for (int i = 0; i < _raysCount; ++i)
            {
                Vector3 direction = Random.onUnitSphere;
                Ray ray = new Ray(grenade.Position, direction);
                Vector3 hitPoint = ray.GetPoint(_raysRange);
                if (Physics.Raycast(ray, out RaycastHit hitInfo, _raysRange, _raysMask, QueryTriggerInteraction.Ignore))
                {
                    hitPoint = hitInfo.point;
                    if (_healthService.GetHitCollider(hitInfo.collider, out IHitCollider hitCollider))
                    {
                        Vector3 difference = hitPoint - grenade.Position;
                        float falloff = Mathf.Clamp01(1f - difference.magnitude / _raysRange);
                        hitCollider.TakeDamage((int)(_rayDamage * falloff));
                        hitCount++;
                        Debug.DrawLine(grenade.Position, hitPoint, Color.yellow, 2f);
                    }
                    else
                    {
                        Debug.DrawLine(grenade.Position, hitPoint, new Color(0.75f, 0.25f, 0f, 0.5f), 2f);
                    }
                }
                else
                {
                    Debug.DrawLine(grenade.Position, hitPoint, new Color(0.5f,0f, 0f, 0.5f), 2f);
                }
            }
            OnHit?.Invoke(hitCount);
        }
    }
}