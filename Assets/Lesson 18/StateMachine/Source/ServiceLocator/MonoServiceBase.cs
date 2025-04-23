using System;
using UnityEngine;

namespace StateMachineSystem.ServiceLocatorSystem
{
    /// <summary>
    /// Base class for Scene context services
    /// On Awake adds itself to service locator
    /// On Destroy, removes itself from service locator
    /// </summary>
    [DefaultExecutionOrder(-10)]
    public abstract class MonoServiceBase : MonoBehaviour, IService
    {
        public abstract Type Type { get; }

        protected virtual void Awake()
        {
            ServiceLocator.Instance.AddService(this);
        }

        protected virtual void OnDestroy()
        {
            ServiceLocator.Instance.RemoveService(this);
        }
    }
}