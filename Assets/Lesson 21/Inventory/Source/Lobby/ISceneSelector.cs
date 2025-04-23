using StateMachineSystem.ServiceLocatorSystem;

namespace Inventory.Lobby
{
    public interface ISceneSelector : IService
    {
        string SelectedScene { get; }
        void SelectScene(string sceneName);
    }
}