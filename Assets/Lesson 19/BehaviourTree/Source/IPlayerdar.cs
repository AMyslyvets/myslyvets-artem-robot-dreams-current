using PhysX;
using UnityEngine;

namespace BehaviourTreeSystem
{
    public interface IPlayerdar
    {
        TargetableBase CurrentTarget { get; }
        bool HasTarget { get; }
        bool SeesTarget { get; }
        Vector3 LastTargetPosition { get; }

        void LookAround();
    }
}