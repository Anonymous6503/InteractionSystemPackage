using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal; // Required for ReorderableList

namespace InteractionSystem.Editor
{
    [CustomPropertyDrawer(typeof(Runtime.Strategy.InteractionStrategy), true)]
    public class InteractionStrategyDrawer : PropertyDrawer
    {
        private ReorderableList _conditionsList;
        private static List<Type> _conditionTypes;
        
        // A dictionary to cache property finders
        private Dictionary<string, SerializedProperty> _propertyCache = new Dictionary<string, SerializedProperty>();

        // Helper to find properties safely
        private SerializedProperty GetProperty(SerializedProperty property, string name)
        {
            string key = property.propertyPath + name;
            if (_propertyCache.TryGetValue(key, out SerializedProperty foundProp))
            {
                return foundProp;
            }
            
            var prop = property.FindPropertyRelative(name);
            _propertyCache[key] = prop; // Cache it (even if null)
            return prop;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            
            Rect foldoutRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label, true);
            
            float currentY = position.y;
            
            if (property.isExpanded)
            {
                EditorGUI.indentLevel++;
                currentY += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                var hasConditionsProp = GetProperty(property, "hasConditions");
                var conditionsListProp = GetProperty(property, "allConditions"); 
                
                if (_conditionsList == null && conditionsListProp != null)
                {
                    InitializeList(property.serializedObject, conditionsListProp);
                }

                SerializedProperty child = property.Copy();
                var endProperty = property.GetEndProperty();
                child.NextVisible(true); 

                while (!SerializedProperty.EqualContents(child, endProperty))
                {
                    Rect rect;
                    float height;

                    if (SerializedProperty.EqualContents(child, hasConditionsProp))
                    {
                        height = EditorGUI.GetPropertyHeight(child, true);
                        rect = new Rect(position.x, currentY, position.width, height);
                        EditorGUI.PropertyField(rect, child, true);
                        currentY += height + EditorGUIUtility.standardVerticalSpacing;
                    }
                    else if (SerializedProperty.EqualContents(child, conditionsListProp))
                    {
                        if (hasConditionsProp != null && hasConditionsProp.boolValue)
                        {
                            if (_conditionsList != null)
                            {
                                height = _conditionsList.GetHeight();
                                rect = new Rect(position.x, currentY, position.width, height);
                                _conditionsList.DoList(rect);
                                currentY += height + EditorGUIUtility.standardVerticalSpacing;
                            }
                        }
                    }
                    else
                    {
                        height = EditorGUI.GetPropertyHeight(child, true);
                        rect = new Rect(position.x, currentY, position.width, height);
                        EditorGUI.PropertyField(rect, child, true);
                        currentY += height + EditorGUIUtility.standardVerticalSpacing;
                    }

                    if (!child.NextVisible(false)) 
                        break;
                }
                
                EditorGUI.indentLevel--;
            }
            
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float totalHeight = EditorGUIUtility.singleLineHeight;
            if (property.isExpanded)
            {
                totalHeight += EditorGUIUtility.standardVerticalSpacing;

                var hasConditionsProp = GetProperty(property, "hasConditions");
                var conditionsListProp = GetProperty(property, "allConditions");

                if (_conditionsList == null && conditionsListProp != null)
                {
                    InitializeList(property.serializedObject, conditionsListProp);
                }
                
                SerializedProperty child = property.Copy();
                var endProperty = property.GetEndProperty();
                child.NextVisible(true);

                while (!SerializedProperty.EqualContents(child, endProperty))
                {
                    if (SerializedProperty.EqualContents(child, conditionsListProp))
                    {
                        if (hasConditionsProp != null && hasConditionsProp.boolValue && _conditionsList != null)
                        {
                            totalHeight += _conditionsList.GetHeight() + EditorGUIUtility.standardVerticalSpacing;
                        }
                    }
                    else
                    {
                        totalHeight += EditorGUI.GetPropertyHeight(child, true) + EditorGUIUtility.standardVerticalSpacing;
                    }

                    if (!child.NextVisible(false))
                        break;
                }
            }
            return totalHeight;
        }

        private void InitializeList(SerializedObject serializedObject, SerializedProperty conditionsListProp)
        {
            if (_conditionTypes == null)
            {
                var conditionType = typeof(Runtime.InteractionConditions.InteractionCondition);
                _conditionTypes = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(assembly => assembly.GetTypes())
                    .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(conditionType))
                    .ToList();
            }
            
            _conditionsList = new ReorderableList(serializedObject, conditionsListProp, true, true, true, true);

            _conditionsList.drawHeaderCallback = (Rect rect) =>
            {
                EditorGUI.LabelField(rect, "Interaction Conditions");
            };

            // --- THIS IS THE UPDATED DRAW CALLBACK ---
            _conditionsList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                var element = _conditionsList.serializedProperty.GetArrayElementAtIndex(index);
                rect.y += 2;
                
                string label = "Element " + index;
                if (element.managedReferenceValue != null)
                {
                    // Start with the class name as a fallback
                    label = element.managedReferenceValue.GetType().Name.Replace("Condition_", "").Replace("IC_", "");

                    // --- NEW LOGIC ---
                    // Try to find a "name-giving" field
                    // First, try to find *your* "condition" field
                    SerializedProperty nameSourceField = element.FindPropertyRelative("condition");

                    // If that's not it, try to find "FlagToCheck"
                    if (nameSourceField == null)
                    {
                        nameSourceField = element.FindPropertyRelative("FlagToCheck");
                    }
                    
                    // If we found a field and it has an object in it...
                    if (nameSourceField != null && nameSourceField.objectReferenceValue != null)
                    {
                        // ...use the object's name as the label!
                        label = nameSourceField.objectReferenceValue.name;
                    }
                    // --- END NEW LOGIC ---
                }

                // Draw the foldout for the element
                element.isExpanded = EditorGUI.Foldout(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element.isExpanded, label, true);
                
                if (element.isExpanded)
                {
                    float currentY = rect.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    EditorGUI.indentLevel++;
                    
                    SerializedProperty child = element.Copy();
                    var endProperty = element.GetEndProperty();
                    child.NextVisible(true); 

                    while (!SerializedProperty.EqualContents(child, endProperty))
                    {
                        float height = EditorGUI.GetPropertyHeight(child, true);
                        Rect childRect = new Rect(rect.x, currentY, rect.width, height);
                        EditorGUI.PropertyField(childRect, child, true);
                        currentY += height + EditorGUIUtility.standardVerticalSpacing;
                        
                        if (!child.NextVisible(false)) break;
                    }
                    EditorGUI.indentLevel--;
                }
            };

            // (elementHeightCallback is the same as before)
            _conditionsList.elementHeightCallback = (int index) =>
            {
                var element = _conditionsList.serializedProperty.GetArrayElementAtIndex(index);
                float height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; 

                if (element.isExpanded)
                {
                    SerializedProperty child = element.Copy();
                    var endProperty = element.GetEndProperty();
                    child.NextVisible(true);
                    while (!SerializedProperty.EqualContents(child, endProperty))
                    {
                        height += EditorGUI.GetPropertyHeight(child, true) + EditorGUIUtility.standardVerticalSpacing;
                        if (!child.NextVisible(false)) break;
                    }
                }
                return height;
            };

            // (onAddDropdownCallback is the same as before)
            _conditionsList.onAddDropdownCallback = (Rect buttonRect, ReorderableList l) =>
            {
                var menu = new GenericMenu();
                foreach (var type in _conditionTypes)
                {
                    menu.AddItem(new GUIContent(type.Name.Replace("Condition_", "").Replace("IC_","")), false, () =>
                    {
                        var index = l.serializedProperty.arraySize;
                        l.serializedProperty.InsertArrayElementAtIndex(index);
                        var element = l.serializedProperty.GetArrayElementAtIndex(index);
                        element.managedReferenceValue = Activator.CreateInstance(type);
                        serializedObject.ApplyModifiedProperties();
                    });
                }
                menu.ShowAsContext();
            };
        }
    }
}