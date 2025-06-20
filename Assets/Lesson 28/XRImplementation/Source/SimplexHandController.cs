using UnityEngine;
using UnityEngine.InputSystem;

namespace XRImplementation
{
    public class SimplexHandController : MonoBehaviour
    {
        [SerializeField] private InputAction _handPosition;
        [SerializeField] private InputAction _handRotation;
        [SerializeField] private Transform _controllerAnchor;
        [SerializeField] private BonePair[] _bonePairs;
        
        private void Start()
        {
            _handPosition.Enable();
            _handRotation.Enable();
        }

        private void Update()
        {
            Vector3 handPosition = _handPosition.ReadValue<Vector3>();
            Quaternion handRotation = _handRotation.ReadValue<Quaternion>();
            _controllerAnchor.SetLocalPositionAndRotation(handPosition, handRotation);
            
            for (int i = 0; i < _bonePairs.Length; ++i)
                _bonePairs[i].Update();
        }
    }
}