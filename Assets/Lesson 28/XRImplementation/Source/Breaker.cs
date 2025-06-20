using UnityEngine;
using UnityEngine.InputSystem;

namespace XRImplementation
{
    public class Breaker : MonoBehaviour
    {
        [SerializeField] private InputAction _breakInput;

        private void Start()
        {
            _breakInput.Enable();
            _breakInput.performed += BreakHandler;
        }

        private void BreakHandler(InputAction.CallbackContext context)
        {
            Debug.Break();
        }
    }
}