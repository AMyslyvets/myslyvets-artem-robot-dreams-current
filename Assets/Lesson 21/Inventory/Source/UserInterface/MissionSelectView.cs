using System;
using BehaviourTreeSystem;
using Dummies;
using Inventory.Lobby;
using StateMachineSystem.ServiceLocatorSystem;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Inventory.UserInterface
{
    public class MissionSelectView : MonoBehaviour
    {
        [Serializable]
        public struct MissionData
        {
            public string missionName;
            [SceneDropdown] public string sceneName;
            [TextArea] public string description;
        }
        
        [SerializeField] private MissionSelectItem _missionSelectItem;
        [SerializeField] private MissionData[] _missions;
        [SerializeField] private TextMeshPro _description;
        [SerializeField] private Transform _content;
        [SerializeField] private InputAction _scroll;
        [SerializeField] private GameObject _root;
        [SerializeField] private BillboardBase _billboard;

        private MissionSelectItem[] _missionSelectItems;
        private int _selectedMissionIndex;
        private ISceneSelector _sceneSelector;
        
        private void Awake()
        {
            _missionSelectItems = new MissionSelectItem[_missions.Length];
            for (int i = 0; i < _missions.Length; ++i)
            {
                MissionData mission = _missions[i];
                MissionSelectItem missionSelectItem = Instantiate(_missionSelectItem, _content);
                missionSelectItem.SetName(mission.missionName);
                missionSelectItem.gameObject.SetActive(true);
                missionSelectItem.ForceDeselect();
                _missionSelectItems[i] = missionSelectItem;
            }
        }

        public void Show()
        {
            _scroll.Enable();
            _root.SetActive(true);
        }

        public void Hide()
        {
            _scroll.Disable();
            _root.SetActive(false);
        }

        private void Start()
        {
            _billboard.SetCamera(ServiceLocator.Instance.GetService<ICameraService>().Camera);
            _sceneSelector = ServiceLocator.Instance.GetService<ISceneSelector>();
            
            _selectedMissionIndex = 0;
            ApplySelection(0);
            
            _scroll.performed += ScrollHandler;

            Hide();
        }

        private void ApplySelection(int index, bool force = true)
        {
            if (force)
            {
                _missionSelectItems[_selectedMissionIndex].ForceDeselect();
                _selectedMissionIndex = index;
                _missionSelectItems[_selectedMissionIndex].ForceSelect();
            }
            else
            {
                _missionSelectItems[_selectedMissionIndex].Deselect();
                _selectedMissionIndex = index;
                _missionSelectItems[_selectedMissionIndex].Select();
            }
            
            MissionData selectedMission = _missions[_selectedMissionIndex];
            _sceneSelector.SelectScene(selectedMission.sceneName);
            _description.text = selectedMission.description;
        }

        private void ScrollHandler(InputAction.CallbackContext context)
        {
            //Debug.Log($"[Scroll] {Mathf.Sign(context.ReadValue<float>())}");
            int value = (int)Mathf.Sign(-context.ReadValue<float>());
            int index = (_selectedMissionIndex + value + _missions.Length) % _missions.Length;
            ApplySelection(index, false);
            //_selectedMissionIndex = Mathf.Repeat(_selectedMissionIndex - context.ReadValue<int>(), _missions.Length - 1);
        }
    }
}