using System;
using UnityEngine;

namespace StateMachineSystem
{
    public class JumpTest : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private CharacterController _controller;

        private void FixedUpdate()
        {
            _controller.SimpleMove(Vector3.up * _speed);
        }
    }
}