using BehaviourTreeSystem;
using StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;

namespace AudioSystem
{
    public class AmbienceZone : MonoBehaviour
    {
        [SerializeField] private AudioClip[] _audioClips;
        [SerializeField] private AudioSource _audioSource;

        private IPlayerService _playerService;

        private void Start()
        {
            _playerService = ServiceLocator.Instance.GetService<IPlayerService>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other != _playerService.Player.CharacterController)
                return;
            
            _audioSource.PlayOneShot(_audioClips[Random.Range(0, _audioClips.Length)]);
        }
    }
}