using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

[CustomEditor(typeof(Interactable))]
public class InteractableEditor : Editor
{
    private SerializedProperty _strategyField;
    private List<Type> _strategyTypes;
    private string[] _strategyTypeNames;
    private int _selectedTypeIndex = -1;

    private void OnEnable()
    {
        _strategyField = serializedObject.FindProperty("_interactionType");

        var strategyType = typeof(InteractionStrategy);
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

        // Set the OneShotInteraction type as the default value
        if (_strategyField.managedReferenceValue == null)
        {
            _strategyField.managedReferenceValue = Activator.CreateInstance(typeof(OneShotInteraction));
            UpdateSelectedTypeIndex();
        }


        // Draw a label
        EditorGUILayout.LabelField("Interaction Strategy", EditorStyles.boldLabel);

        int newIndex = EditorGUILayout.Popup(_selectedTypeIndex, _strategyTypeNames);

        if (newIndex != _selectedTypeIndex)
        {
            var newType = _strategyTypes[newIndex];
            _strategyField.managedReferenceValue = Activator.CreateInstance(newType);
            _selectedTypeIndex = newIndex;
        }

        EditorGUILayout.PropertyField(_strategyField, true);

        serializedObject.ApplyModifiedProperties();
    }

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