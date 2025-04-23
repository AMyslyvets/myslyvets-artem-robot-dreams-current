using StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;

namespace DefendFlag
{
    public interface IDefendFlagModeService : IService
    {
        Vector3 FlagPosition { get; }
    }
}