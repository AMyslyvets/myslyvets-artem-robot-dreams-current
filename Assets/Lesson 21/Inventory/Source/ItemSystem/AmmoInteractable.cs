using BehaviourTreeSystem;
using DefendFlag;
using StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;

namespace Inventory.ItemSystem
{
    public class AmmoInteractable : InteractableBase
    {
        [SerializeField] private int _clips;
        
        public override void Interact()
        {
            HitScanGunClips gun = ServiceLocator.Instance.GetService<IPlayerService>().Player.GetComponent<HitScanGunClips>();
            gun.AddClips(_clips);
            base.Interact();
        }
    }
}