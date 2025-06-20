using CarSample;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshCarInput : MonoBehaviour
{
    public enum State
    {
        Idle,
        Drive,
        Brake
    }

    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private CarController _carController;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Vector3 _extends;
    [SerializeField] private float _sampleDistance;
    [SerializeField] private float _angle;
    [SerializeField] private float _steering;
    [SerializeField] private Vector3 _position;
    [SerializeField] private float _horizontalSpeed;

    [SerializeField] private State _state;

    private Vector3 _velocity;
    private Vector3 _localDirection;

    private void OnEnable()
    {
        _agent.updatePosition = false;
        _agent.updateRotation = false;

        _state = State.Idle;
    }

    private void Update()
    {
        switch (_state)
        {
            case State.Idle:
                IdleUpdate();
                break;
            case State.Drive:
                DriveUpdate();
                break;
            case State.Brake:
                BrakeUpdate();
                break;
        }

        _agent.nextPosition = transform.position;
    }

    private void IdleUpdate()
    {
        Vector3 position = new(Random.Range(-_extends.x, _extends.x),
            Random.Range(-_extends.y, _extends.y),
            Random.Range(-_extends.z, _extends.z));
        if (!NavMesh.SamplePosition(position, out NavMeshHit hit, _sampleDistance, _agent.areaMask))
        {
            Debug.LogError($"Agent {_agent.gameObject.name} cannot find point on NavMesh!");
            return;
        }
        _agent.SetDestination(hit.position);
        _state = State.Drive;
    }

    private void DriveUpdate()
    {
        _velocity = _agent.desiredVelocity;
        _localDirection = transform.InverseTransformVector(_velocity).normalized;

        _angle = Vector3.SignedAngle(transform.forward, _velocity.normalized, Vector3.up);

        _steering = Mathf.InverseLerp(-35f, 35f, _angle) * 2f - 1f;

        _carController.SetAcceleration(0.5f);
        _carController.SetSteering(_steering);

        if (_agent.pathPending || _agent.remainingDistance <= _agent.stoppingDistance)
        {
            _carController.SetAcceleration(0f);
            _carController.SetSteering(0f);
            _carController.SetBrake(1f);
            _state = State.Brake;
        }
    }

    private void BrakeUpdate()
    {
        _horizontalSpeed = Vector3.ProjectOnPlane(_rigidbody.velocity, Vector3.up).magnitude;
        if (_horizontalSpeed < 0.0001f)
        {
            _carController.SetBrake(0f);
            _state = State.Idle;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(Vector3.zero, _extends * 2f);

        /* Gizmos.color = Color.red;
         Gizmos.DrawLine(transform.position, transform.position + _velocity);
         Gizmos.color = Color.green;
         Vector3 direction = transform.TransformVector(_localDirection);
         Gizmos.DrawLine(transform.position + transform.loca)*/
    }
}
