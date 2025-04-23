using System;
using System.Collections;
using StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace LoadingScreen
{
    public class DummyLoadController : MonoBehaviour
    {
        [SerializeField] private DummyLoad _dummyLoad;
        [SerializeField] private AssetReference _addressableLoad;

        private void Awake()
        {
            Debug.Log($"Dummy load: {_dummyLoad.Size} bytes");
        }

        private IEnumerator Start()
        {
            AsyncOperationHandle<DummyLoad> asyncOperationHandle = _addressableLoad.LoadAssetAsync<DummyLoad>();
            ServiceLocator.Instance.GetService<ILoadingScreenService>().BeginLoading(asyncOperationHandle);
            yield return asyncOperationHandle;
            Debug.Log($"Addressable Dummy load: {(_addressableLoad.Asset as DummyLoad)?.Size.ToString() ?? "[NULL]"} bytes");
        }

        private void OnDestroy()
        {
            _addressableLoad.ReleaseAsset();
        }
    }
}