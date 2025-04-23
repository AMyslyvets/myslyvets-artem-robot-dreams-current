using System;
using StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;

namespace Lesson26
{
    public class SimplexPlayer : MonoBehaviour
    {
        private void Start()
        {
            Camera camera = ServiceLocator.Instance.GetService<CameraManager>().Camera;
            
            Debug.Log($"Player found camera via Service Locator: {camera.gameObject.name}");
            
            Camera camera2 = GameManager.Instance.CameraManager.Camera;
            Debug.Log($"Player found camera via Game Manager Singleton: {camera2.gameObject.name}");
            
        }
    }
}