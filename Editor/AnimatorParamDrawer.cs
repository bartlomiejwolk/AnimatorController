using UnityEngine;
using UnityEditor;
using System.Reflection;
using OneDayGame;

namespace AnimatorControllerEx {


    [CustomPropertyDrawer(typeof (AnimatorController.AnimatorParam))]
    public class AnimatorParamDrawer : GameComponentPropertyDrawer {

        private float rows = 4;

        public override float GetPropertyHeight(
            SerializedProperty property,
            GUIContent label) {
            return base.GetPropertyHeight(property, label)
                   * rows // Each row is 16 px high.
                   + (rows - 1) * 4; // Add 4 px for each spece between rows.
        }

        public override void OnGUI(
            Rect pos,
            SerializedProperty prop,
            GUIContent label) {

            /* Serialized properties */
            SerializedProperty sourceType =
                prop.FindPropertyRelative("_sourceType");
            SerializedProperty messageType =
                prop.FindPropertyRelative("_messageType");
            SerializedProperty param =
                prop.FindPropertyRelative("_param");
            SerializedProperty sourceCo =
                prop.FindPropertyRelative("_sourceCo");
            SerializedProperty sourcePropIndex =
                prop.FindPropertyRelative("_sourcePropIndex");
            SerializedProperty sourcePropertyName =
                prop.FindPropertyRelative("_sourcePropertyName");
            SerializedProperty trigger =
                prop.FindPropertyRelative("_trigger");

            // Component properties.
            PropertyInfo[] _sourceProperties;

            // Draw 'sourceType' dropdown.
            EditorGUI.PropertyField(
                new Rect(
                    pos.x,
                    pos.y,
                    pos.width,
                    16),
                sourceType,
                new GUIContent(
                    "Source Type",
                    "Source based on which the Animator parameter will be updated"));

            // Draw properties for 'Component' source type.
            if (sourceType.enumValueIndex
                == (int) AnimatorController.SourceTypes.Component) {
                // Draw 'param' field.
                EditorGUIUtility.labelWidth = 50;
                EditorGUI.PropertyField(
                    new Rect(
                        pos.x,
                        pos.y + 20,
                        pos.width * 0.5f,
                        16),
                    param,
                    new GUIContent(
                        "Param",
                        "Animator parameter name to update."));

                // Draw 'trigger' field.
                EditorGUIUtility.labelWidth = 50;
                EditorGUI.PropertyField(
                    new Rect(
                        pos.x + pos.width * 0.5f + 3,
                        pos.y + 20,
                        pos.width * 0.5f,
                        16),
                    trigger,
                    new GUIContent(
                        "Trigger",
                        "If the animator param. is a trigger."));

                // Draw 'sourceCo' field.
                EditorGUIUtility.labelWidth = 50;
                EditorGUI.PropertyField(
                    new Rect(
                        pos.x,
                        pos.y + 40,
                        pos.width,
                        16),
                    sourceCo,
                    new GUIContent(
                        "Source",
                        "Component which property is used to update " +
                        "selected animator parameter."));
                EditorGUIUtility.labelWidth = 0;
            }

            // Draw properties for 'Message' source type.
            if (sourceType.enumValueIndex
                == (int) AnimatorController.SourceTypes.Message) {
                // Message type source is always a trigger.
                trigger.boolValue = true;

                EditorGUI.PropertyField(
                    new Rect(
                        pos.x,
                        pos.y + 20,
                        pos.width,
                        16),
                    messageType,
                    new GUIContent(
                        "Message Type",
                        "Type of the message"
                        ));

                // Draw 'param' field.
                EditorGUIUtility.labelWidth = 50;
                EditorGUI.PropertyField(
                    new Rect(
                        pos.x,
                        pos.y + 40,
                        pos.width * 0.5f,
                        16),
                    param,
                    new GUIContent(
                        "Param",
                        "Animator parameter name to update."));

                // Draw 'trigger' field.
                EditorGUIUtility.labelWidth = 50;
                EditorGUI.PropertyField(
                    new Rect(
                        pos.x + pos.width * 0.5f + 3,
                        pos.y + 40,
                        pos.width * 0.5f,
                        16),
                    trigger,
                    new GUIContent(
                        "Trigger",
                        "If the animator param. is a trigger. For message " +
                        "source type it's read-only."));
            }

            // Component properties by name.
            string[] sourcePropNames;
            // Find component properties in a selected component and display
            // as dropdown.
            if (
                // Source component is not null.
                sourceCo.objectReferenceValue &&
                // Source type must 'Component'.
                sourceType.enumValueIndex
                == (int) AnimatorController.SourceTypes.Component) {
                // Get all properties from source component.
                _sourceProperties =
                    sourceCo.objectReferenceValue.GetType().GetProperties();
                // Initialize array.
                sourcePropNames = new string[_sourceProperties.Length];
                // Fill array with property names.
                for (int i = 0; i < _sourceProperties.Length; i++) {
                    sourcePropNames[i] = _sourceProperties[i].Name;
                }
                EditorGUIUtility.labelWidth = 80;
                // Display dropdown component property list.
                sourcePropIndex.intValue = EditorGUI.Popup(
                    new Rect(
                        pos.x,
                        pos.y + (3 * 20),
                        // rows * (row height + empty space)
                        pos.width,
                        16),
                    "Source Prop.",
                    sourcePropIndex.intValue,
                    sourcePropNames);

                // Save selected property name.
                sourcePropertyName.stringValue =
                    sourcePropNames[sourcePropIndex.intValue];
            }
        }

    }

}
