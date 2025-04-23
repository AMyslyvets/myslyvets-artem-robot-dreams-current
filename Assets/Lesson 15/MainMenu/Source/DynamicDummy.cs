using System;
using System.Collections;
using Dummies;
using UnityEngine;

namespace MainMenu
{
    public class DynamicDummy : MonoBehaviour
    {
        [SerializeField] private Health _health;
        [SerializeField] private HealthIndicator _healthIndicator;
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Transform _characterTransform;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Transform _meshRendererTransform;
        [SerializeField] private Transform _fallMark;
        [SerializeField] private AnimationCurve _fallCurve;
        [SerializeField] private GameObject _rootObject;
        [SerializeField] private float _healthBarDelayTime;
        [SerializeField] private float _speed;

        private Vector3 _fallMarkPosition;
        private Quaternion _fallMarkRotation;
        private DummyTrack _dummyTrack;

        private float _time;
        
        public Health Health => _health;
        public HealthIndicator HealthIndicator => _healthIndicator;

        private void Start()
        {
            _health.OnDeath += DeathHandler;
            
            _fallMark.GetLocalPositionAndRotation(out _fallMarkPosition, out _fallMarkRotation);
        }

        private void FixedUpdate()
        {
            if (!_health.IsAlive)
                return;
            
            Vector3 position = _dummyTrack.Evaluate(_time);
            Vector3 translation = position - _characterTransform.position;
            _characterTransform.rotation = Quaternion.LookRotation(translation, Vector3.up);
            _characterController.Move(translation);
            _time += _speed * Time.fixedDeltaTime;
        }

        public void SetTrack(DummyTrack dummyTrack)
        {
            _dummyTrack = dummyTrack;
        }
        
        private void DeathHandler()
        {
            StartCoroutine(DelayedDestroy());
        }

        private IEnumerator DelayedDestroy()
        {
            float time = 0f;
            float reciprocal = 1f / _healthBarDelayTime;

            while (time < _healthBarDelayTime)
            {
                EvaluateFall(time * reciprocal);
                yield return null;
                time += Time.deltaTime;
            }
            
            EvaluateFall(1f);
            
            Destroy(_rootObject);
        }
        
        private void EvaluateFall(float progress)
        {
            float curveFactor = _fallCurve.Evaluate(progress);
            Vector3 position = Vector3.Lerp(Vector3.zero, _fallMarkPosition, curveFactor);
            Quaternion rotation = Quaternion.Slerp(Quaternion.identity, _fallMarkRotation, curveFactor);
            _meshRendererTransform.SetLocalPositionAndRotation(position, rotation);
        }
    }
}