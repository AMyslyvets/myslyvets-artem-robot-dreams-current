using StateMachineSystem.ServiceLocatorSystem;

namespace Inventory.Lobby
{
    public interface ISceneService : IService
    {
        /// <summary>
        /// Method that loads scene registered as Lobby in SceneData
        /// </summary>
        void SetLobby();
        void SetGameplayScene(string scene);
    }
}