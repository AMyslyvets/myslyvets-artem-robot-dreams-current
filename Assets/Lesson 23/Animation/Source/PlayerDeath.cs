using System.Collections;
using BehaviourTreeSystem;
using BehaviourTreeSystem.VisualEffects;
using StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;

namespace Animation
{
    public class PlayerDeath : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private string _deathName;
        [SerializeField] private float _crossFadeTime;
        [SerializeField] private HandsIK _handsIK;
        [SerializeField] private float _healthBarDelayTime;
        [SerializeField] private GameObject _logicalPlayer;
        
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
            _logicalPlayer.SetActive(false);
            yield return null;
            
            _animator.CrossFadeInFixedTime(_deathName, _crossFadeTime);
            _handsIK.DisableIK();
            
            yield return new WaitForSeconds(_healthBarDelayTime);
            
            ServiceLocator.Instance.GetService<ISaturationService>().SetDeathSaturation();
        }
    }
}