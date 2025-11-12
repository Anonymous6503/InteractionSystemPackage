using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace InteractionSystem.Editor
{
    // Use the full namespace
    [CustomEditor(typeof(Runtime.Interactable))] 
    public class InteractableEditor : UnityEditor.Editor
    {
        private SerializedProperty _strategyField;
        private List<Type> _strategyTypes;
        private string[] _strategyTypeNames;
        private int _selectedTypeIndex = -1;

        private void OnEnable()
        {
            // Use the correct variable name from your screenshot
            _strategyField = serializedObject.FindProperty("interactionType");

            var strategyType = typeof(Runtime.Strategy.InteractionStrategy);
            _strategyTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(strategyType))
                .ToList();
            
            _strategyTypeNames = _strategyTypes.Select(t => t.Name).ToArray();
            
            UpdateSelectedTypeIndex();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // --- 1. Set Default (if null) ---
            if (_strategyField.managedReferenceValue == null)
            {
                // Make sure your strategy class is named "InteractionStrategy_Once"
                var defaultType = _strategyTypes.FirstOrDefault(t => t.Name == "InteractionStrategy_Once");
                if (defaultType != null)
                {
                    _strategyField.managedReferenceValue = Activator.CreateInstance(defaultType);
                }
                UpdateSelectedTypeIndex();
            }

            // --- 2. Draw Dropdown ---
            EditorGUILayout.LabelField("Interaction Strategy", EditorStyles.boldLabel);
            int newIndex = EditorGUILayout.Popup(_selectedTypeIndex, _strategyTypeNames);

            if (newIndex != _selectedTypeIndex)
            {
                var newType = _strategyTypes[newIndex];
                _strategyField.managedReferenceValue = Activator.CreateInstance(newType);
                _selectedTypeIndex = newIndex;
            }

            EditorGUILayout.Space(5);

            // --- 3. Draw The Strategy's Properties ---
            // This line will *automatically* find and use our new
            // PropertyDrawer (from Step 2) to draw the field.
            EditorGUILayout.PropertyField(_strategyField, new GUIContent("Strategy Properties"), true);

            serializedObject.ApplyModifiedProperties();
        }
        
        // (UpdateSelectedTypeIndex method stays the same as before)
        private void UpdateSelectedTypeIndex()
        {
             if (_strategyField.managedReferenceValue != null)
            {
                var currentType = _strategyField.managedReferenceValue.GetType();
                _selectedTypeIndex = _strategyTypes.IndexOf(currentType);
            }
            else
            {
                _selectedTypeIndex = -1;
            }
        }
    }
}