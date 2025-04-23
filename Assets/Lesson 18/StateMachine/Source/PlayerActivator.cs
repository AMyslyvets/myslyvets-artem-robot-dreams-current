using System;
using StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;

namespace StateMachineSystem
{
    public class PlayerActivator : MonoBehaviour
    {
        [SerializeField] private GameObject _player;
        
        private void Start()
        {
            _player.SetActive(true);
            ServiceLocator.Instance.GetService<InputController>().enabled = true;
        }
    }
}