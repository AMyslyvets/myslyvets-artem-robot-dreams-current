using StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;

namespace DefendFlag
{
    public class FlagInteractable : InteractableBase
    {
        public override InteractableType Type => InteractableType.Activate;

        public override void Interact()
        {
            ServiceLocator.Instance.GetService<IModeService>().Begin();
            Destroy(this);
        }
    }
}