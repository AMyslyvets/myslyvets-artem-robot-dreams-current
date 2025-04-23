using DefendFlag;
using UnityEngine;

namespace Inventory.Lobby
{
    public class SceneSelectInteractable : InteractableBase
    {
        
        public override void Interact()
        {
            Highlight(false);
        }
    }
}