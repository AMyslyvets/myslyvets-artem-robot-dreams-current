using System;
using StateMachineSystem.ServiceLocatorSystem;
using TMPro;
using UnityEngine;

namespace AudioSystem.LocalizationSystem
{
    /// <summary>
    /// Script that controls TextMeshProUGUI component
    /// by storing a term name for text, setting appropriate value on Start, and subscribing to changes of language
    /// in order to update data on language change
    /// </summary>
    public class LocalizedTextMeshProUGUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textMeshProUGUI;
        [SerializeField, LocalizationTerm] private string _term;

        private ILocalizationService _localizationService;

        private void Start()
        {
            // Cache localization service and get term needed in current language
            _localizationService = ServiceLocator.Instance.GetService<ILocalizationService>();
            _textMeshProUGUI.text = _localizationService.GetTermValue(_term);

            // Subscribe to changes of language
            _localizationService.OnLanguageChanged += LanguageHandler;
        }

        private void LanguageHandler()
        {
            // On language change, simply get term value once more and apply to text
            _textMeshProUGUI.text = _localizationService.GetTermValue(_term);
        }

        private void OnDestroy()
        {
            // Unsubscribe from persistant service in order to avoid dangling handlers
            _localizationService.OnLanguageChanged -= LanguageHandler;
        }
    }
}