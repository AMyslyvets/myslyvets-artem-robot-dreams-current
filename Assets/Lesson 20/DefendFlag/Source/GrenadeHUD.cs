using StateMachineSystem;
using TMPro;
using UnityEngine;

namespace DefendFlag
{
    public class GrenadeHUD : MonoBehaviour
    {
        [SerializeField] private GrenadeAction _grenadeAction;
        [SerializeField] private TextMeshProUGUI _grenadeValue;
        [SerializeField] private TextMeshProUGUI _maxGrenades;

        private void Start()
        {
            _grenadeValue.text = _grenadeAction.MaxGrenades.ToString();
            _maxGrenades.text = _grenadeAction.MaxGrenades.ToString();
            _grenadeAction.onGrenadesCountChanged += ChargeHandler;
        }

        private void ChargeHandler()
        {
            _grenadeValue.text = _grenadeAction.GrenadeCount.ToString();
        }
    }
}