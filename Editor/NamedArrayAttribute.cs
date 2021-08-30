using UnityEngine;
using UnityEditor;

namespace Neat.Tweening.Editor {
    [CustomPropertyDrawer(typeof(NamedArrayAttribute))]
    public class NamedArrayDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            try {
                if (int.TryParse(property.propertyPath.Split('[', ']')[1], out int pos)) {
                    var names = ((NamedArrayAttribute) attribute).Names;
                    if (pos < names.Length) {
                        var name = $"{((NamedArrayAttribute) attribute).BaseName} {pos} ({names[pos]})";
                        EditorGUI.PropertyField(position, property, new GUIContent(name), true);
                    }
                    else {
                        var name = $"{((NamedArrayAttribute) attribute).BaseName} {pos}";
                        EditorGUI.PropertyField(position, property, new GUIContent(name), true);
                    }
                }
                else {
                    EditorGUI.PropertyField(position, property, label, true);
                }
            }
            catch {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) =>
            EditorGUI.GetPropertyHeight(property, label);
    }
}
