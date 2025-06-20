using System.Collections;
using Animation;
using BehaviourTreeSystem;
using BehaviourTreeSystem.VisualEffects;
using StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;

namespace XRImplementation
{
    /// <summary>
    /// Almost copy of Death controller from animation
    /// Removed handling IK
    /// Added handling death without disabling gameobject
    /// But character controller is still being disabled
    /// </summary>
    public class PlayerDeath : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private string _deathName;
        [SerializeField] private float _crossFadeTime;
        [SerializeField] private float _healthBarDelayTime;
        [SerializeField] private CharacterController _characterController;

        private ICompositeHealth _health;

        private void Start()
        {
            _health = ServiceLocator.Instance.GetService<IPlayerService>().Player.GetComponent<ICompositeHealth>();
            _health.OnDeath += DeathHandler;
        }

        private void DeathHandler()
        {
            StartCoroutine(DelayedDestroy());
        }

        private IEnumerator DelayedDestroy()
        {
            ServiceLocator.Instance.GetService<StateMachineSystem.InputController>().enabled = false;
            yield return null;

            _animator.CrossFadeInFixedTime(_deathName, _crossFadeTime);

            yield return new WaitForSeconds(_healthBarDelayTime);

            _characterController.enabled = false;
            ServiceLocator.Instance.GetService<ISaturationService>().SetDeathSaturation();
        }
    }
}