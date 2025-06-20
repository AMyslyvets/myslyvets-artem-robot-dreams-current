using UnityEngine;
using UnityEngine.AI;

namespace Lesson35
{
    public class SimplexAgent : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private NavMeshAgent _agent;

        [SerializeField] private CharacterController _characterController;
        [SerializeField] private float _speed;

        [ContextMenu("Launch")]
        private void Launch()
        {
            _agent.avoidancePriority = Random.Range(1, 100);
            _agent.SetDestination(_target.position);
            _agent.isStopped = false;
        }

        private void Start()
        {
            _agent.updatePosition = false;
            _agent.updateRotation = false;
        }

        private void Update()
        {
            if (_agent.isStopped)
                return;

            Vector3 direction = _agent.desiredVelocity.normalized;

            _characterController.SimpleMove(direction * _speed);
            _agent.nextPosition = _characterController.transform.position;

            if (_agent.pathPending || _agent.remainingDistance <= _agent.stoppingDistance)
            {
                _agent.avoidancePriority = 0;
                _agent.isStopped = true;
            }
        }
    }
}