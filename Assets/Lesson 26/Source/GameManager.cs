using UnityEngine;

namespace Lesson26
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private PlayerManager _playerManager;
        [SerializeField] private CameraManager _cameraManager;
        
        public PlayerManager PlayerManager => _playerManager;
        public CameraManager CameraManager => _cameraManager;
    }
}