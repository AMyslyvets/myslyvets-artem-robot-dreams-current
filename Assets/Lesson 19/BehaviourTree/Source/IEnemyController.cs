using MainMenu;
using StateMachineSystem;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviourTreeSystem
{
    public interface IEnemyController
    {
        float PatrolStamina { get; set; }
        EnemyData Data { get; }
        NavMeshAgent NavMeshAgent { get; }
        INavPointProvider NavPointProvider { get; }
        CharacterController CharacterController { get; }
        Transform CharacterTransform { get; }
        IHealth Health { get; }
        IPlayerdar Playerdar { get; }
        Transform FallMark { get; }
        GameObject RootObject { get; }
        Transform MeshRendererTransform { get; }
        Transform WeaponTransform { get; }
        HitScanGun HitScanGun { get; }
        WeaponData WeaponData { get; }
        GameObject HealthIndicator { get; }

        void ComputeBehaviour();
        void RestorePatrolStamina();
    }
}