using System.Collections;
using System.Collections.Generic;
using StateMachineSystem.ServiceLocatorSystem;
using TMPro;
using UnityEngine;

namespace Lesson26
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _playerName;
        [SerializeField] private TextMeshProUGUI _cameraName;

        private CameraManager _cameraManager;
        private PlayerManager _playerManager;
        
        private void Start()
        {
            _cameraManager = ServiceLocator.Instance.GetService<CameraManager>();
            _playerManager = ServiceLocator.Instance.GetService<PlayerManager>();
            
            _playerName.SetText(_playerManager.Player.name);
            _cameraName.SetText(_cameraManager.Camera.gameObject.name);
        }
    }
}