using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Animation
{
    public class GraphController : MonoBehaviour
    {
        [SerializeField] private AnimationClip _animationClipPrimary;
        [SerializeField] private AnimationClip _animationClipSecondary;
        [SerializeField] private Animator _animator;
        [SerializeField, Range(0f, 1f)] private float _weight;

        private PlayableGraph _graph;
        private AnimationPlayableOutput _output;
        private AnimationMixerPlayable _animationMixer;
        private AnimationClipPlayable _animationClipPlayablePrimary;
        private AnimationClipPlayable _animationClipPlayableSecondary;
        
        private void Start()
        {
            _graph = PlayableGraph.Create("Custom graph");
            _animationClipPlayablePrimary = AnimationClipPlayable.Create(_graph, _animationClipPrimary);
            _animationClipPlayableSecondary = AnimationClipPlayable.Create(_graph, _animationClipSecondary);
            
            _animationMixer = AnimationMixerPlayable.Create(_graph, 2);
            _animationMixer.ConnectInput(0, _animationClipPlayablePrimary, 0);
            _animationMixer.ConnectInput(1, _animationClipPlayableSecondary, 0);
            
            _output = AnimationPlayableOutput.Create(_graph, "Animation", _animator);
            
            _output.SetSourcePlayable(_animationMixer);
            
            _graph.Play();
        }

        public void SetPrimary(AnimationClip animationClipPrimary)
        {
            _graph.DestroyPlayable(_animationClipPlayablePrimary);
            _animationClipPlayablePrimary = AnimationClipPlayable.Create(_graph, animationClipPrimary);
            _animationMixer.ConnectInput(0, _animationClipPlayablePrimary, 0);
        }
        
        private void Update()
        {
            _animationMixer.SetInputWeight(0, _weight);
            _animationMixer.SetInputWeight(1, 1f - _weight);
        }
    }
}