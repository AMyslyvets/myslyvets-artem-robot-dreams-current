using System;

namespace StateMachineSystem.ServiceLocatorSystem
{
    /// <summary>
    /// An abstraction of service for service locator
    /// The Type field holds a type each service wants to be recognised as
    /// </summary>
    public interface IService
    {
        Type Type { get; }
    }
}