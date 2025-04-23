using System.Collections.Generic;
using AudioSystem.LocalizationSystem;
using Inventory.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace AudioSystem.Editor
{
    /// <summary>
    /// This class will be used to draw in inspector any property with LocalizationTerm Attribute
    /// </summary>
    [CustomPropertyDrawer(typeof(LocalizationTermAttribute))]
    public class LocalizationTermAttributeDrawer : PropertyDrawer
    {
        /// <summary>
        /// Method that draws serialized field with LocalizationTerm attribute, called by UnityEditor
        /// </summary>
        /// <param name="position"></param>
        /// <param name="property"></param>
        /// <param name="label"></param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Only if property is string, draw dropdown, otherwise, default drawing
            if (property.propertyType == SerializedPropertyType.String)
            {
                // Get options from localization data
                List<string> options = LocalizationEditorSettings.instance.LocalizationData.Terms;
                // Select index of current value in options collection
                int selectedIndex = Mathf.Max(0, options.IndexOf(property.stringValue));
            
                // Draw a popup, if user selects new value, selectedIndex will change
                selectedIndex = EditorGUI.Popup(position, label.text, selectedIndex, options.ToArray());
                // Put into string property corresponding value
                property.stringValue = options[selectedIndex];
            }
            else
            {
                // if property is not a string, default property field will be drawn
                EditorGUI.PropertyField(position, property, label);
            }
        }
    }
}