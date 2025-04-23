using System;
using System.Collections.Generic;
using StateMachineSystem.ServiceLocatorSystem;
using TMPro;
using UnityEngine;

namespace AudioSystem.LocalizationSystem
{
    /// <summary>
    /// Script that has options as languages registered in localization data
    /// Gets options from ILocalizationService abstraction, and on user's selection sends command to change language
    /// </summary>
    public class LocalizationDropdown : MonoBehaviour
    {
        // Reference to a dropdown
        [SerializeField] private TMP_Dropdown _dropdown;

        // Cached localization service
        private ILocalizationService _localizationService;
        // Cached localization keys
        private List<string> _localizationKeys;
        
        private void Start()
        {
            // At start, localization service needs to be received from service locator
            // In order to get list of languages
            // In order to set options to dropdown
            _localizationService = ServiceLocator.Instance.GetService<ILocalizationService>();
            _localizationKeys = _localizationService.GetSupportedLanguages();
            _dropdown.ClearOptions();
            _dropdown.AddOptions(_localizationKeys);
            // current language opeytion is set as current dropdown value without raising any events
            _dropdown.SetValueWithoutNotify(_localizationKeys.IndexOf(_localizationService.GetCurrentLanguage()));
            // on change of selected option, handler is subscribed
            _dropdown.onValueChanged.AddListener(DropdownHandler);
        }

        private void DropdownHandler(int option)
        {
            // On change of selected option in dropdown, issue a command to localization service to set new language
            _localizationService.SetLanguage(_localizationKeys[option]);
        }
    }
}