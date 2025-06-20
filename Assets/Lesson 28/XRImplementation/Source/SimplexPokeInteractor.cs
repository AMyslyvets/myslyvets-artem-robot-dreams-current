using System;
using UnityEngine;

namespace XRImplementation
{
    public class SimplexPokeInteractor : MonoBehaviour
    {
        [SerializeField] private float _radius;
        [SerializeField] private LayerMask _layerMask;

        private Transform _transform;

        private void Start()
        {
            _transform = transform;
        }

        private void FixedUpdate()
        {
            Collider[] colliders = Physics.OverlapSphere(_transform.position, _radius, _layerMask, QueryTriggerInteraction.Collide);
            for (int i = 0; i < colliders.Length; i++)
            {
                UIPokeInteractable interactable = colliders[i].GetComponent<UIPokeInteractable>();
                if (interactable == null)
                    continue;
                interactable.Interact();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
    }
}