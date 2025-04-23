using System;
using System.Collections;
using System.Collections.Generic;
using StateMachineSystem.ServiceLocatorSystem;
using TMPro;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace LoadingScreen
{
    public class LoadingScreenService : GlobalMonoServiceBase, ILoadingScreenService
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private RectTransform _progressValue;
        [SerializeField] private Vector2 _progressSize;
        [SerializeField] private float _progressSpeed;
        [SerializeField] private TextMeshProUGUI _progressText;

        private readonly List<AsyncOperation> _sceneLoadOperations = new();
        private readonly List<AsyncOperationHandle> _addressableLoadOperations = new();

        private Coroutine _loadRoutine;
        private float _currentProgress;
        private bool _loadingScene;
        
        public override Type Type { get; } = typeof(ILoadingScreenService);

        protected override void Awake()
        {
            base.Awake();
            _canvas.enabled = false;
            _loadingScene = true;
            _progressText.text = "Loading Scene...";
        }

        public void BeginLoading(AsyncOperation asyncOperation)
        {
            _sceneLoadOperations.Add(asyncOperation);
            if (_loadRoutine != null)
                return;
            _loadRoutine = StartCoroutine(LoadRoutine());
        }
        
        public void BeginLoading(AsyncOperationHandle asyncOperation)
        {
            _addressableLoadOperations.Add(asyncOperation);
            if (_loadRoutine != null)
                return;
            _loadRoutine = StartCoroutine(LoadRoutine());
        }

        private IEnumerator LoadRoutine()
        {
            Debug.Log("Loading Start");
            _canvas.enabled = true;
            
            bool isDone = false;
            
            while (!isDone)
            {
                isDone = true;
                int sceneOperationIndex = -1;
                for (int i = 0; i < _sceneLoadOperations.Count; ++i)
                {
                    AsyncOperation operation = _sceneLoadOperations[i];
                    if (operation.isDone)
                        continue;
                    isDone = false;
                    sceneOperationIndex = i;
                    break;
                }

                if (!isDone)
                {
                    EvaluateBar(_sceneLoadOperations[sceneOperationIndex].progress, true);
                    yield return null;
                    continue;
                }
                
                isDone = true;
                int addressableOperationIndex = -1;
                for (int i = 0; i < _addressableLoadOperations.Count; ++i)
                {
                    AsyncOperationHandle operation = _addressableLoadOperations[i];
                    if (operation.IsDone)
                        continue;
                    isDone = false;
                    addressableOperationIndex = i;
                    break;
                }

                if (!isDone)
                {
                    EvaluateBar(_addressableLoadOperations[addressableOperationIndex].PercentComplete, false);
                    yield return null;
                    continue;
                }
            }
            
            _sceneLoadOperations.Clear();
            _addressableLoadOperations.Clear();
            
            _canvas.enabled = false;
            _loadRoutine = null;
            
            Debug.Log("Loading Complete");
        }

        private void EvaluateBar(float progress, bool loadingScene)
        {
            if (loadingScene != _loadingScene)
            {
                _loadingScene = loadingScene;
                _progressText.text = _loadingScene ? "Loading Scene..." : "Loading Addressables...";
            }
            
            if (_currentProgress >= progress)
                _currentProgress = progress;
            else
            {
                _currentProgress = Mathf.MoveTowards(_currentProgress, progress, _progressSpeed * Time.deltaTime);
            }
            Vector2 size = _progressSize;
            size.x *= _currentProgress;
            _progressValue.sizeDelta = size;
        }
    }
}