using System;
using UnityEngine;

namespace DefendFlag
{
    public abstract class StimsControllerBase : MonoBehaviour
    {
        public event Action OnStimUsed;
        public abstract int MaxStims { get; }
        public abstract int CurrentStims { get; }
        public abstract void AddStims(int stims);
        
        protected void InvokeOnStimUsed() => OnStimUsed?.Invoke();
    }
}