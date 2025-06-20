using System;
using UnityEngine;

namespace XRImplementation
{
    public class UIPokeInteractable : MonoBehaviour
    {
        public event Action onClick;
        
        public void Interact()
        {
            onClick?.Invoke();
        }
    }
}