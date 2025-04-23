using System;
using System.Collections.Generic;
using AudioSystem.SaveSystem;
using StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;

namespace AudioSystem.LocalizationSystem
{
    /// <summary>
    /// Implementation of abstraction of ILocalizationService
    /// Inheritor of GlobalMonoServiceBase, meaning it should be added to a Boot scene, so it will be persistant across scenes
    /// </summary>
    public class LocalizationService : GlobalMonoServiceBase, ILocalizationService
    {
        public event Action OnLanguageChanged;
        
        [SerializeField] private LocalizationData _data;

        private string _currentLanguage;
        
        // In localization data, terms are stored in raw form, in order to edit
        // In service, data will be cached into dictionaries, in order to avoid constant searches
        private Dictionary<string, Dictionary<string, string>> _localizationLookup = new();
        private Dictionary<string, string> _currentTermsLookup;
        
        private ISaveService _saveService;
        
        public override Type Type { get; } = typeof(ILocalizationService);

        protected override void Awake()
        {
            base.Awake();
            
            // Cache ISaveService in order to get saved language preference
            _saveService = ServiceLocator.Instance.GetService<ISaveService>();
            string savedLanguage = _saveService.SaveData.localizationData.language;
            // Ensure that saved language exists in data, otherwise, select first one and save it
            if (!_data.Languages.Contains(savedLanguage))
            {
                savedLanguage = _data.Languages[0];
                _saveService.SaveData.localizationData.language = savedLanguage;
            }

            // Cache data into lookup dictionaries
            for (int i = 0; i < _data.LanguageEntries.Length; ++i)
            {
                LocalizationData.LanguageEntry languageEntry = _data.LanguageEntries[i];
                
                Dictionary<string, string> termLookup = new Dictionary<string, string>();
                
                for (int j = 0; j < languageEntry.terms.Length; ++j)
                {
                    termLookup.Add(_data.Terms[j], languageEntry.terms[j]);
                }
                
                _localizationLookup.Add(_data.Languages[i], termLookup);
            }
            
            // Set current language and reference to current lookup
            _currentLanguage = savedLanguage;
            _currentTermsLookup = _localizationLookup[savedLanguage];
        }

        public string GetCurrentLanguage() => _currentLanguage;
        public List<string> GetSupportedLanguages() => _data.Languages;
        
        public void SetLanguage(string language)
        {
            // In order to set language, not only variable should be changed
            // but reference to current lookup, as well as data applied to SaveData
            // and an event needs to be issued, so subscribers can update their content
            _currentLanguage = language;
            _currentTermsLookup = _localizationLookup[language];
            _saveService.SaveData.localizationData.language = _currentLanguage;
            OnLanguageChanged?.Invoke();
        }

        public string GetTermValue(string term)
        {
            // Simply get the term value from current lookup
            return _currentTermsLookup[term];
        }
    }
}