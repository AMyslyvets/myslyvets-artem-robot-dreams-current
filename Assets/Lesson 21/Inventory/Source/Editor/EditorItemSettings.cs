using Inventory.ItemSystem;
using UnityEditor;
using UnityEngine;

namespace Inventory.Editor
{
    [FilePath("Inventory/Data/EditorItemSettings.asset", FilePathAttribute.Location.ProjectFolder)]
    public class EditorItemSettings : ScriptableSingleton<EditorItemSettings>
    {
        [SerializeField] private ItemLibrary _itemLibrary;

        public ItemLibrary ItemLibrary
        {
            get => _itemLibrary;
            set
            {
                if (_itemLibrary == value)
                    return;
                _itemLibrary = value;
                EditorUtility.SetDirty(this);
                Save(true);
            }
        }
        
        public string[] ItemIds()
        {
            return _itemLibrary?.ItemIds ?? new []{"None"};
        }
    }
}