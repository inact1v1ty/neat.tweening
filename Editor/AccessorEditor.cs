using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace Neat.Tweening.Editor {
    [CustomPropertyDrawer(typeof(Accessor<>))]
    public class AccessorEditor : PropertyDrawer {
        private SerializedProperty accessorProperty;
        private Accessor accessor;
        private SerializedProperty componentTypeNameProperty;
        private SerializedProperty fieldProperty;

        private List<(Component component, FieldInfo[] fields, PropertyInfo[] properties)> components;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) =>
            base.GetPropertyHeight(property, label) * 2;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            accessorProperty = null;
            accessor = null;
            componentTypeNameProperty = null;
            fieldProperty = null;

            if (property.serializedObject.isEditingMultipleObjects) return;
            var behavior = property.serializedObject.targetObject as MonoBehaviour;
            if (behavior == null) return;

            accessorProperty = property;
            accessor = fieldInfo.GetValue(property.serializedObject.targetObject) as Accessor;

            if (accessor == null) return;

            EditorGUI.BeginProperty(position, GUIContent.none, property);

            componentTypeNameProperty = property.FindPropertyRelative("componentTypeName");
            fieldProperty = property.FindPropertyRelative("fieldName");

            Cache(behavior);

            var componentTypeName = componentTypeNameProperty.stringValue;
            var field = fieldProperty.stringValue;

            string componentName = null;

            if (!string.IsNullOrEmpty(componentTypeName)) {
                var componentType = Type.GetType(componentTypeName);
                if (componentType != null) {
                    var component = behavior.GetComponent(componentType);
                    if (component != null) {
                        componentName = component.GetType().Name;
                    }
                }
            }

            var hasAccess = !(string.IsNullOrEmpty(componentName) || string.IsNullOrEmpty(field));

            var labelRect = new Rect(position.x, position.y, position.width, 18);
            var buttonRect = new Rect(position.x, position.y + 18, position.width, 18);

            GUI.Label(labelRect, "Accessor");

            var buttonContent = new GUIContent(hasAccess ? componentName + "." + field : "No field");

            if (GUI.Button(buttonRect, buttonContent)) {
                ShowSelectMenu();
            }

            EditorGUI.EndProperty();
        }

        private void Cache(MonoBehaviour behaviour) {
            components = new List<(Component, FieldInfo[], PropertyInfo[])>();
            foreach (var component in behaviour.gameObject.GetComponents<Component>()) {
                if (component.GetType().IsSubclassOf(typeof(Tween))) continue;

                var type = component.GetType();
                var fields = GetAllFields(type).ToArray();
                var properties = GetAllProperties(type).ToArray();
                if (fields.Length > 0 || properties.Length > 0) {
                    components.Add((component, fields, properties));
                }
            }
        }

        private IEnumerable<FieldInfo> GetAllFields(Type type) {
            if (type == null) {
                return Enumerable.Empty<FieldInfo>();
            }

            const BindingFlags flags = BindingFlags.Public |
                                       BindingFlags.NonPublic |
                                       BindingFlags.Static |
                                       BindingFlags.Instance |
                                       BindingFlags.DeclaredOnly;

            return type.GetFields(flags)
                .Where(field => field.FieldType == accessor.ValueType)
                .Union(GetAllFields(type.BaseType));
        }

        private IEnumerable<PropertyInfo> GetAllProperties(Type type) {
            if (type == null) {
                return Enumerable.Empty<PropertyInfo>();
            }

            const BindingFlags flags = BindingFlags.Public |
                                       BindingFlags.NonPublic |
                                       BindingFlags.Static |
                                       BindingFlags.Instance |
                                       BindingFlags.DeclaredOnly;

            return type.GetProperties(flags)
                .Where(property =>
                    property.PropertyType == accessor.ValueType &&
                    property.CanRead &&
                    property.CanWrite)
                .Union(GetAllProperties(type.BaseType));
        }

        private void ShowSelectMenu() {
            // Now create the menu, add items and show it
            var menu = new GenericMenu();

            menu.AddItem(new GUIContent("No field"), false, OnDeselect);

            if (components.Count > 0) {
                menu.AddSeparator("");
            }

            for (var i = 0; i < components.Count; i++) {
                for (var j = 0; j < components[i].fields.Length; j++) {
                    menu.AddItem(
                        new GUIContent(components[i].component.GetType().Name + "/" + components[i].fields[j].Name),
                        false, OnSelect, (i, j, true));
                }

                if (components[i].fields.Length > 0 && components[i].properties.Length > 0) {
                    menu.AddSeparator(components[i].component.GetType().Name + "/");
                }

                for (var j = 0; j < components[i].properties.Length; j++) {
                    menu.AddItem(
                        new GUIContent(components[i].component.GetType().Name + "/" + components[i].properties[j].Name),
                        false, OnSelect, (i, j, false));
                }
            }

            menu.ShowAsContext();
            Event.current.Use();
        }

        private void OnDeselect() {
            componentTypeNameProperty.stringValue = null;
            fieldProperty.stringValue = null;

            accessorProperty.serializedObject.ApplyModifiedProperties();
        }

        private void OnSelect(object data) {
            var (i, j, isField) = ((int, int, bool)) data;

            var componentType = components[i].component.GetType();
            var componentTypeName = componentType.ToString();
            var assemblyName = componentType.Assembly.GetName().Name;
            var resultName = $"{componentTypeName}, {assemblyName}";
            componentTypeNameProperty.stringValue = resultName;

            fieldProperty.stringValue = isField ? components[i].fields[j].Name : components[i].properties[j].Name;

            accessorProperty.serializedObject.ApplyModifiedProperties();
        }
    }
}
