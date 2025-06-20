using UnityEngine;
using UnityEngine.InputSystem;

public class InputLog : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }

    [ContextMenu("Log")]
    private void Log()
    {
        string msg = "Devices:";
        foreach (var device in InputSystem.devices)
        {
            msg += $"\n{device.displayName}";
        }
        Debug.Log(msg);
    }
}
