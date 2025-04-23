using StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace LoadingScreen
{
    public interface ILoadingScreenService : IService
    {
        void BeginLoading(AsyncOperation asyncOperation);
        void BeginLoading(AsyncOperationHandle asyncOperation);
    }
}