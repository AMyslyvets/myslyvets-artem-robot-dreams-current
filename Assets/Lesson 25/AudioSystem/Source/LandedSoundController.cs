using System;
using StateMachineSystem.Locomotion;
using UnityEngine;

namespace AudioSystem
{
    /// <summary>
    /// Script that plays landed sound on Locomotion's state machine state change
    /// But only if state machine was in falling state and changed it to idle (even with pressed W, state machine will
    /// change state to Idle, and only then to Movement)
    /// </summary>
    [DefaultExecutionOrder(10)]
    public class LandedSoundController : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private LocomotionController _locomotionController;

        private LocomotionState _lastState;
        
        private void Start()
        {
            _lastState = _locomotionController.LocomotionState;
            _locomotionController.OnStateChanged += LocomotionStateChanged;
        }

        private void LocomotionStateChanged(LocomotionState state)
        {
            //Debug.Log($"Locomotion state changed: from [{_lastState}] to [{state}]");
            if (_lastState == LocomotionState.Fall && state == LocomotionState.Idle)
            {
                if (_audioSource.isPlaying)
                    _audioSource.Stop();
                _audioSource.Play();
            }
            _lastState = state;
        }
    }
}