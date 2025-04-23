using TMPro;
using UnityEngine;

namespace DefendFlag
{
    public class StimsHUD : MonoBehaviour
    {
        [SerializeField] private StimsControllerBase _stims;
        [SerializeField] private TextMeshProUGUI _stimsValue;
        [SerializeField] private TextMeshProUGUI _maxStims;

        private void Start()
        {
            _stimsValue.text = _stims.MaxStims.ToString();
            _maxStims.text = _stims.MaxStims.ToString();
            _stims.OnStimUsed += StimUsedHandler;
        }

        private void StimUsedHandler()
        {
            _stimsValue.text = _stims.CurrentStims.ToString();
        }
    }
}