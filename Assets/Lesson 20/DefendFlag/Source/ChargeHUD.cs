using TMPro;
using UnityEngine;

namespace DefendFlag
{
    public class ChargeHUD : MonoBehaviour
    {
        [SerializeField] private HitScanGunClips _gun;
        [SerializeField] private TextMeshProUGUI _chargeValue;
        [SerializeField] private TextMeshProUGUI _maxCharge;

        private void Start()
        {
            _chargeValue.text = _gun.MaxCharge.ToString();
            _maxCharge.text = _gun.MaxCharge.ToString();
            _gun.OnChargeChanged += ChargeHandler;
        }

        private void ChargeHandler(int charge)
        {
            _chargeValue.text = charge.ToString();
        }
    }
}