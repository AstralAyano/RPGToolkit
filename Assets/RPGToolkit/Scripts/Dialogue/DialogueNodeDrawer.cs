using UnityEditor;
using UnityEngine;

namespace RPGToolkit
{
    [CustomPropertyDrawer(typeof(DialogueInfoSO.DialogueNode))]
    public class DialogueNodeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // Save and set indent level
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // Get serialized properties
            SerializedProperty speakerSpriteProperty = property.FindPropertyRelative("speakerSprite");
            SerializedProperty speakerProperty = property.FindPropertyRelative("speaker");
            SerializedProperty textLinesProperty = property.FindPropertyRelative("textLines");
            SerializedProperty optionsProperty = property.FindPropertyRelative("options");
            SerializedProperty questStartOrEndProperty = property.FindPropertyRelative("questStartOrEnd");
            SerializedProperty questInfoProperty = property.FindPropertyRelative("questInfo");

            // Draw properties
            float y = position.y;

            // Speaker Sprite
            EditorGUI.PropertyField(new Rect(position.x, y, position.width, EditorGUIUtility.singleLineHeight), speakerSpriteProperty);
            y += EditorGUIUtility.singleLineHeight;

            // Speaker Name
            EditorGUI.PropertyField(new Rect(position.x, y, position.width, EditorGUIUtility.singleLineHeight), speakerProperty);
            y += EditorGUIUtility.singleLineHeight;

            // Text Lines
            float textLinesHeight = EditorGUI.GetPropertyHeight(textLinesProperty, true);
            EditorGUI.PropertyField(new Rect(position.x, y, position.width, textLinesHeight), textLinesProperty, true);
            y += textLinesHeight;

            // Options
            float optionsHeight = EditorGUI.GetPropertyHeight(optionsProperty, true);
            EditorGUI.PropertyField(new Rect(position.x, y, position.width, optionsHeight), optionsProperty, true);
            y += optionsHeight;

            // Quest Start/End
            EditorGUI.PropertyField(new Rect(position.x, y, position.width, EditorGUIUtility.singleLineHeight), questStartOrEndProperty);
            y += EditorGUIUtility.singleLineHeight;

            // Quest Info
            EditorGUI.PropertyField(new Rect(position.x, y, position.width, EditorGUIUtility.singleLineHeight), questInfoProperty);

            // Restore original indent level
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = EditorGUIUtility.singleLineHeight * 5; // Fixed height for properties
            height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("textLines"), true);
            height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("options"), true);
            return height + 20; // Extra spacing
        }
    }
}