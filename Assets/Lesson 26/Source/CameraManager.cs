using System;
using StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;

namespace Lesson26
{
    //public class CameraManager : Singleton<CameraManager>
    //public class CameraManager : MonoBehaviour
    public class CameraManager : MonoServiceBase
    {
        [SerializeField] private Camera _camera;
        
        public Camera Camera => _camera;
        public override Type Type { get; } = typeof(CameraManager);
    }
}