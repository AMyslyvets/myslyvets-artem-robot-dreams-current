using System;
using UnityEngine;

namespace DefendFlag
{
    public class FlagController : MonoBehaviour
    {
        [Serializable]
        public struct JointData
        {
            public Transform transform;
            public AnimationCurve curve;

            public void Evaluate(float progress)
            {
                transform.localPosition = new Vector3(0f, curve.Evaluate(progress), 0f);
            }
        }
        
        [SerializeField] private JointData[] _joints;
        [SerializeField, Range(0f, 1f)] private float _progress;
        [SerializeField] private Renderer _flagRenderer;
        [SerializeField] private float _flagTime;
        [SerializeField] private ParticleSystem[] _particleSystems;

        public void SetProgress(float progress)
        {
            _progress = progress;
        }

        public void Complete()
        {
            for (int i = 0; i < _particleSystems.Length; ++i)
                _particleSystems[i].Play(true);
        }
        
        private void Update()
        {
            for (int i = 0; i < _joints.Length; ++i)
                _joints[i].Evaluate(_progress);
            _flagRenderer.enabled = _progress >= _flagTime;
        }
    }
}