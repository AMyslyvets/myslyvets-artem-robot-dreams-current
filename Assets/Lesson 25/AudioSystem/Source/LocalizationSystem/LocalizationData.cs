using System;
using System.Collections.Generic;
using UnityEngine;

namespace AudioSystem.LocalizationSystem
{
    /// <summary>
    /// Scriptable object that holds collections:
    /// Of languages registered
    /// Terms registered
    /// Table that has as many entries as languages, and each of those entries as many entries as terms
    /// Each value is a value of term in that language, entry of which element current value is
    /// </summary>
    [CreateAssetMenu(fileName = "LocalizationData", menuName = "Data/Localization/Main Asset", order = 0)]
    public class LocalizationData : ScriptableObject
    {
        [Serializable]
        public class LanguageEntry
        {
            // Better name - termValues
            public string[] terms;
        }
        
        [SerializeField, HideInInspector] LanguageEntry[] _languageEntries;
        
        [SerializeField, HideInInspector] private List<string> _terms;
        [SerializeField, HideInInspector] private List<string> _languages;
        
        public List<string> Terms => _terms;
        public List<string> Languages => _languages;
        public LanguageEntry[] LanguageEntries => _languageEntries;
    }
}