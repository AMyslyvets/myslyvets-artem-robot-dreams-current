using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace StateMachineSystem
{
    public class DebugCanvasController : MonoBehaviour
    {
        [SerializeField] private bool displayRuntimeUI;

        private void Update()
        {
            displayRuntimeUI = DebugManager.instance.displayRuntimeUI;
        }

        [ContextMenu("Display RuntimeUI")]
        private void DisplayRuntimeUI()
        {
            DebugManager.instance.displayRuntimeUI = true;
        }
        
        [ContextMenu("Hide RuntimeUI")]
        private void HideRuntimeUI()
        {
            DebugManager.instance.displayRuntimeUI = false;
        }
    }
}