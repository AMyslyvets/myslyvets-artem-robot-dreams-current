using AudioSystem.LocalizationSystem;
using UnityEditor;
using UnityEngine;

namespace AudioSystem.Editor
{
    /// <summary>
    /// Editor singleton, saved per project
    /// Acts like gateway for attribute drawers, so properties with attributes can read languages
    /// and terms registred in current LocalizationData
    /// </summary>
    [FilePath("Localization/Data/EditorSceneSettings.asset", FilePathAttribute.Location.ProjectFolder)]
    public class LocalizationEditorSettings : ScriptableSingleton<LocalizationEditorSettings>
    {
        // Reference to LocalizationData, that is current one for project
        [SerializeField] private LocalizationData _localizationData;

        /// <summary>
        /// Property that editor window can change
        /// if changed, singleton will be saved
        /// </summary>
        public LocalizationData LocalizationData
        {
            get => _localizationData;
            set
            {
                if (value == _localizationData)
                    return;
                _localizationData = value;
                Save(true);
            }
        }
    }
}