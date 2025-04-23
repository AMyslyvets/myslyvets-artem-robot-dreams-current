using System;
using StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;

namespace BehaviourTreeSystem
{
    public class CameraService : MonoServiceBase, ICameraService
    {
        [SerializeField] private Camera _camera;
        public override Type Type { get; } = typeof(ICameraService);
        public Camera Camera => _camera;
    }
}