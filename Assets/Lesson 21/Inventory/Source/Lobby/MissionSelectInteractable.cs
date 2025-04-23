using DefendFlag;
using Inventory.UserInterface;
using UnityEngine;

namespace Inventory.Lobby
{
    public class MissionSelectInteractable : InteractableBase
    {
        public override InteractableType Type => InteractableType.Activate;
        
        [SerializeField] private MissionSelectView _missionSelectView;

        private bool _active = false;
        
        public override void Interact()
        {
            _active = !_active;
            if (_active)
            {
                _missionSelectView.Show();
            }
            else
            {
                _missionSelectView.Hide();
            }
            tooltip.gameObject.SetActive(!_active);
        }

        public override void Highlight(bool active)
        {
            if (active)
            {
                base.Highlight(active);
            }
            else
            {
                _active = false;
                _missionSelectView.Hide();
                tooltip.gameObject.SetActive(false);
            }
        }
    }
}