using System;
using StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;

namespace Lesson26
{
    //public class PlayerManager : Singleton<PlayerManager>
    //public class PlayerManager : MonoBehaviour
    public class PlayerManager : MonoServiceBase
    {
        [SerializeField] private GameObject _playerPrefab;

        public GameObject Player { get; private set; }

        /*protected override void Awake()
        {
            base.Awake();
            Player = Instantiate(_playerPrefab);
        }*/

        public override Type Type { get; } = typeof(PlayerManager);

        /*private void Awake()
        {
            Player = Instantiate(_playerPrefab);
        }*/
        
        protected override void Awake()
        {
            base.Awake();
            Player = Instantiate(_playerPrefab);
        }
    }
}