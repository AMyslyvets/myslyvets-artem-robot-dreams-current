using System;
using UnityEngine;

namespace StateMachineSystem
{
    public abstract class GrenadeSpawnerBase : MonoBehaviour
    {
        public event Action<Grenade> OnGrenadeSpawned;
        
        protected void InvokeOnGrenadeSpawned(Grenade grenade) => OnGrenadeSpawned?.Invoke(grenade);
    }
}