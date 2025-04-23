using System;
using StateMachineSystem.ServiceLocatorSystem;

namespace DefendFlag
{
    public interface IModeService : IService
    {
        event Action<bool> OnComplete;
        
        void Begin();
    }
}