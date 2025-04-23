using System;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTreeSystem
{
    public class ReactionController : MonoBehaviour
    {
        public List<Func<bool>> _conditions;
        
        private void Start()
        {
            
        }

        private bool WasDamaged()
        {
            return true;
        }
    }
}