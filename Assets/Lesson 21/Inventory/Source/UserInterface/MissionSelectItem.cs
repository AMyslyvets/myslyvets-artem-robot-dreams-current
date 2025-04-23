using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Inventory.UserInterface
{
    public class MissionSelectItem : MonoBehaviour
    {
        [Serializable]
        public struct State
        {
            public float scale;
            public Color spriteColor;
            public Color textColor;
            [NonSerialized] public float transitionProgress;

            public void Evaluate(Transform transform, SpriteRenderer spriteRenderer, TextMeshPro textMesh)
            {
                transform.localScale = Vector3.one * scale;
                spriteRenderer.color = spriteColor;
                textMesh.color = textColor;
            }
        }
        
        [SerializeField] private Transform _transform;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private TextMeshPro _textMesh;
        [SerializeField] private State _regular;
        [SerializeField] private State _selected;
        [SerializeField] private float _transitionSpeed;

        private Coroutine _transitionRoutine;

        private State _prevState;
        private State _currentState;
        private State _nextState;

        private void ForceState(State state)
        {
            _currentState = state;
            _currentState.transitionProgress = 1f;
            _currentState.Evaluate(_transform, _spriteRenderer, _textMesh);
        }

        public void ForceSelect()
        {
            ForceState(_selected);
        }
        
        public void ForceDeselect()
        {
            ForceState(_regular);
        }

        public void SetName(string name)
        {
            _textMesh.text = name;
        }

        public void Select()
        {
            BeginTransition(_selected);
        }
        
        public void Deselect()
        {
            BeginTransition(_regular);
        }

        private void BeginTransition(State nextState)
        {
            if (_transitionRoutine != null)
            {
                StopCoroutine(_transitionRoutine);
                _currentState.transitionProgress = 1f - _currentState.transitionProgress;
            }
            else
            {
                _currentState.transitionProgress = 0f;
            }
            _prevState = _currentState;
            _nextState = nextState;
            _transitionRoutine = StartCoroutine(Transition());
        }
        
        private IEnumerator Transition()
        {
            while (_currentState.transitionProgress < 1f)
            {
                _currentState.scale = Mathf.Lerp(_prevState.scale, _nextState.scale, _currentState.transitionProgress);
                _currentState.spriteColor = Color.Lerp(_prevState.spriteColor, _nextState.spriteColor, _currentState.transitionProgress);
                _currentState.textColor = Color.Lerp(_prevState.textColor, _nextState.textColor, _currentState.transitionProgress);
                _currentState.Evaluate(_transform, _spriteRenderer, _textMesh);
                yield return null;
                _currentState.transitionProgress = Mathf.MoveTowards(_currentState.transitionProgress, 1f, _transitionSpeed * Time.deltaTime);
            }
            _currentState.transitionProgress = 1f;
            _currentState.scale = _nextState.scale;
            _currentState.spriteColor = _nextState.spriteColor;
            _currentState.textColor = _nextState.textColor;
            _currentState.Evaluate(_transform, _spriteRenderer, _textMesh);
        }
    }
}