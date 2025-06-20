using System;
using UnityEngine;

namespace CarSample
{
    public class CarController : MonoBehaviour
    {
        [Serializable]
        public struct WheelRenderer
        {
            public WheelCollider wheel;
            public Transform renderer;
        }

        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private WheelCollider[] _frontWheels;
        [SerializeField] private WheelCollider[] _backWheels;
        [SerializeField] private WheelRenderer[] _wheels;

        [SerializeField] private float _maxTorque;
        [SerializeField] private float _accelerationFactor;
        [SerializeField] private float _maxSteerAngle;
        [SerializeField] private float _steeringFactor;
        [SerializeField] private float _brakeForce;

        private float _torque;
        private float _acceleration;
        private float _steering;
        private float _brake;

        public void SetAcceleration(float acceleration)
        {
            _acceleration = acceleration;
        }

        public void SetSteering(float steering)
        {
            _steering = steering;
        }

        public void SetBrake(float brake)
        {
            _brake = brake;
        }

        private void Update()
        {
            for (int i = 0; i < _wheels.Length; ++i)
            {
                WheelRenderer wheel = _wheels[i];
                wheel.wheel.GetWorldPose(out Vector3 position, out Quaternion rotation);
                wheel.renderer.transform.SetPositionAndRotation(position, rotation);
            }
        }

        private void FixedUpdate()
        {
            float targetTorque = Mathf.LerpUnclamped(0f, _maxTorque, _acceleration);

            if ((targetTorque > _torque && targetTorque > 0f) || (targetTorque < _torque && targetTorque < 0f))
            {
                _torque = Mathf.MoveTowards(_torque, targetTorque, _accelerationFactor * Time.fixedDeltaTime);
            }
            else
            {
                _torque = targetTorque;
            }

            for (int i = 0; i < _backWheels.Length; ++i)
            {
                _backWheels[i].motorTorque = _torque;

                _backWheels[i].brakeTorque = _brakeForce * _brake;
            }

            float steerAngle = Mathf.LerpUnclamped(0f, _maxSteerAngle, _steering);

            for (int i = 0; i < _frontWheels.Length; ++i)
            {
                _frontWheels[i].steerAngle = steerAngle;

                _frontWheels[i].brakeTorque = _brakeForce * _brake;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position + _rigidbody.centerOfMass,
                transform.position + _rigidbody.centerOfMass + _rigidbody.velocity);
        }
    }
}