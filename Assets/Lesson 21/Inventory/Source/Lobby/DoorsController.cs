using System;
using System.Collections.Generic;
using Inventory.GameStateSystem;
using StateMachineSystem.GameStateSystem;
using StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Inventory.Lobby
{
    public class DoorsController : MonoBehaviour
    {
        [Serializable]
        private struct DoorEntry
        {
            [SceneDropdown] public string sceneName;
            public DoorInteractable door;
        }
        
        [SerializeField] private DoorInteractable _exit;
        [SerializeField] private DoorEntry[] _doors;
        [SerializeField] private DoorInteractable _missionDoor;

        private ISceneSelector _sceneSelector;
        private IGameStateProvider _gameStateProvider;
        
        private readonly Dictionary<DoorInteractable, string> _doorToScene = new();
        
        private void Awake()
        {
            _exit.OnDoorInteract += ExitHandler;
            _missionDoor.OnDoorInteract += MissionDoorHandler;
            
            for (int i = 0; i < _doors.Length; ++i)
            {
                DoorEntry entry = _doors[i];
                _doorToScene.Add(entry.door, entry.sceneName);
                entry.door.OnDoorInteract += DoorHandler;
            }
        }

        private void Start()
        {
            _sceneSelector = ServiceLocator.Instance.GetService<ISceneSelector>();
            _gameStateProvider = ServiceLocator.Instance.GetService<IGameStateProvider>();
        }

        private void ExitHandler(DoorInteractable door)
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void DoorHandler(DoorInteractable door)
        {
            _sceneSelector.SelectScene(_doorToScene[door]);
            _gameStateProvider.SetGameState(GameState.Gameplay);
        }

        private void MissionDoorHandler(DoorInteractable door)
        {
            _gameStateProvider.SetGameState(GameState.Gameplay);
        }
    }
}