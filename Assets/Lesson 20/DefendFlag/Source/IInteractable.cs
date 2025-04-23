using System;
using UnityEngine;

namespace DefendFlag
{
    public interface IInteractable
    {
        event Action<IInteractable> onDestroy;
        InteractableType Type { get; }
        Vector3 Position { get; }
        void Highlight(bool active);
        void Interact();
    }
}