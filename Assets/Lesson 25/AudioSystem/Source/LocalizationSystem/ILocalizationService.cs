using System;
using System.Collections.Generic;
using StateMachineSystem.ServiceLocatorSystem;

namespace AudioSystem.LocalizationSystem
{
    /// <summary>
    /// Abstraction of a service, which you can set language, get current one, get list of registered languages
    /// and get value for specific term in current language, as well as subscribe to event of language change
    /// </summary>
    public interface ILocalizationService : IService
    {
        event Action OnLanguageChanged;
        void SetLanguage(string language);
        string GetCurrentLanguage();
        List<string> GetSupportedLanguages();
        string GetTermValue(string term);
    }
}