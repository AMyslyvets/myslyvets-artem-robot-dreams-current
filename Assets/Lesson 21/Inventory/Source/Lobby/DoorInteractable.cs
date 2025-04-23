using System;
using DefendFlag;
using UnityEngine;

namespace Inventory.Lobby
{
    public class DoorInteractable : InteractableBase
    {
        public event Action<DoorInteractable> OnDoorInteract;

        public override InteractableType Type => InteractableType.Activate;

        public override void Interact()
        {
            collider.enabled = false;
            Highlight(false);
            OnDoorInteract?.Invoke(this);
        }
    }
}