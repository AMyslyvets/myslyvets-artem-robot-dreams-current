using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefendFlag
{
    public class ReloadHUD : MonoBehaviour
    {
        [SerializeField] private HitScanGunClips _gun;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _clipsValue;
        [SerializeField] private TextMeshProUGUI _maxClips;

        private void Start()
        {
            _gun.OnReload += ReloadHandler;
            _gun.OnClipsCountChanged += ClipsChangeHandler;
            enabled = false;

            _clipsValue.text = _gun.MaxClips.ToString();
            _maxClips.text = _gun.MaxClips.ToString();
        }

        private void ReloadHandler(bool active)
        {
            _canvasGroup.alpha = active ? 1f : 0f;
            enabled = active;
            _clipsValue.text = _gun.CurrentClips.ToString();
        }

        private void ClipsChangeHandler()
        {
            _clipsValue.text = _gun.CurrentClips.ToString();
        }
        
        private void Update()
        {
            _image.fillAmount = _gun.Reload.Progress;
        }
    }
}