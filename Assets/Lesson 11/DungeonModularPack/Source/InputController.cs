using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    public Action<Vector2> OnMove;
    public Action<Vector2> OnLook;
    
    [SerializeField] private InputActionAsset _inputActionAsset;
    [SerializeField] private string _mapName;
    [SerializeField] private string _moveName;
    [SerializeField] private string _lookName;

    private IEnumerator Start()
    {
        yield return null;
        enabled = false;
        
        InputActionMap map = _inputActionAsset.FindActionMap(_mapName);
        InputAction moveAction = map[_moveName];
        InputAction lookAction = map[_lookName];

        moveAction.performed += MoveHandler;
        moveAction.canceled += MoveHandler;
        
        lookAction.performed += LookHandler;
    }

    private void OnEnable()
    {
        _inputActionAsset.Enable();
    }

    private void OnDisable()
    {
        _inputActionAsset.Disable();
    }

    private void MoveHandler(InputAction.CallbackContext context)
    {
        OnMove?.Invoke(context.ReadValue<Vector2>());
    }

    private void LookHandler(InputAction.CallbackContext context)
    {
        OnLook?.Invoke(context.ReadValue<Vector2>());
    }
}
