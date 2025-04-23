using System;
using UnityEngine;

namespace StateMachineSystem.ServiceLocatorSystem
{
    /// <summary>
    /// Base class for persistant services
    /// On Awake, calls base Awake, which adds it to service locator
    /// Then, put itself into DontDestroyOnLoad
    /// </summary>
    [DefaultExecutionOrder(-20)]
    public abstract class GlobalMonoServiceBase : MonoServiceBase
    {
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }
    }
}