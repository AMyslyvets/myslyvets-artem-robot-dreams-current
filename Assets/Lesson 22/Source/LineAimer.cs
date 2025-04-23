using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Lesson22
{
    public class LineAimer : MonoBehaviour
    {
        [SerializeField] private Transform _muzzle;
        [SerializeField] private float _muzzleSpeed;
        [SerializeField] private LineRenderer _aimRenderer;
        [SerializeField] private float _simulationMultilplier;
        [SerializeField] private SimplexCamera _simplexCamera;
        [SerializeField] private Vector2 _aimBounds;

        private float _muzzlePitch;

        private List<Vector3> _aimPoints = new();

        private void Start()
        {
            _aimRenderer.positionCount = 2;
            _aimRenderer.SetPosition(0, _muzzle.position);
            _muzzlePitch = _aimBounds.x;
        }

        private void FixedUpdate()
        {
            float normalizedPitch = Mathf.InverseLerp(_simplexCamera.PitchBounds.x, _simplexCamera.PitchBounds.y, _simplexCamera.Pitch);
            _muzzlePitch = Mathf.Lerp(_aimBounds.x, _aimBounds.y, 1f - normalizedPitch);
            //_muzzlePitch = _simplexCamera.Pitch;
            
            _muzzle.localRotation = Quaternion.Euler(_muzzlePitch, 0, 0);

            _aimPoints.Clear();

            RaycastHit hit;

            Vector3 virtualProjectilePosition = _muzzle.position;
            Vector3 virtualProjectileVelocity = _muzzle.forward * _muzzleSpeed;

            _aimPoints.Add(virtualProjectilePosition);

            while (!Physics.Raycast(virtualProjectilePosition, virtualProjectileVelocity.normalized, out hit,
                       virtualProjectileVelocity.magnitude * Time.fixedDeltaTime * _simulationMultilplier) &&
                   virtualProjectilePosition.y > 0f)
            {
                virtualProjectilePosition += virtualProjectileVelocity * Time.fixedDeltaTime * _simulationMultilplier;
                virtualProjectileVelocity += Physics.gravity * Time.fixedDeltaTime * _simulationMultilplier;
                _aimPoints.Add(virtualProjectilePosition);
            }

            _aimRenderer.positionCount = _aimPoints.Count;
            _aimRenderer.SetPositions(_aimPoints.ToArray());
        }
    }
}