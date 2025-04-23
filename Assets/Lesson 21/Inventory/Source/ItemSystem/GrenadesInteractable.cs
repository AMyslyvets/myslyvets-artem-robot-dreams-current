using BehaviourTreeSystem;
using DefendFlag;
using StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;

namespace Inventory.ItemSystem
{
    public class GrenadesInteractable : InteractableBase
    {
        [SerializeField] private int _grenades;
        
        public override void Interact()
        {
            GrenadeAction grenades = ServiceLocator.Instance.GetService<IPlayerService>().Player.GetComponent<GrenadeAction>();
            grenades.AddGrenades(_grenades);
            base.Interact();
        }
    }
}