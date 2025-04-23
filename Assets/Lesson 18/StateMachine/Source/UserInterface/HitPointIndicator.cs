using System;
using Dummies;
using UnityEngine;

namespace StateMachineSystem.UserInterface
{
    public class HitPointIndicator : MonoBehaviour
    {
        [SerializeField] private Transform _indicator;
        [SerializeField] private GunAimer _gunAimer;
        [SerializeField] private CameraSystem _cameraSystem;
        
        private void Start()
        {
            
        }

        private void Update()
        {
            _indicator.position = _cameraSystem.Camera.WorldToScreenPoint(_gunAimer.CollisionPoint);
        }
    }
}