// Copyright (c) 2015 Bartlomiej Wolk (bartlomiejwolk@gmail.com)
// 
// This file is part of the AnimatorController extension for Unity.
// Licensed under the MIT license. See LICENSE file in the project root folder.

using UnityEditor;
using UnityEngine;

namespace AnimatorControllerEx {

    [CustomPropertyDrawer(typeof (AnimatorParam))]
    public sealed class AnimatorParamDrawer : GameComponentPropertyDrawer {

        #region CONSTANTS

        // Hight of a single property.
        private const int PropHeight = 16;

        // Margin between properties.
        private const int PropMargin = 4;

        // Space between rows.
        private const int RowSpace = 8;

        // Number of rows.
        private const int Rows = 4;

        #endregion

        #region FIELDS

        private string[] sourcePropNames;

        #endregion

        #region UNITY MESSAGES

        public override float GetPropertyHeight(
            SerializedProperty property,
            GUIContent label) {
            return base.GetPropertyHeight(property, label)
                   * Rows // Each row is 16 px high.
                   + (Rows - 1) * RowSpace;
        }

        public override void OnGUI(
            Rect pos,
            SerializedProperty prop,
            GUIContent label) {

            var sourceType =
                prop.FindPropertyRelative("sourceType");
            var messageType =
                prop.FindPropertyRelative("messageType");
            var param =
                prop.FindPropertyRelative("paramName");
            var sourceCo =
                prop.FindPropertyRelative("sourceCo");
            var sourcePropIndex =
                prop.FindPropertyRelative("sourcePropIndex");
            var sourcePropertyName =
                prop.FindPropertyRelative("sourcePropertyName");
            var trigger =
                prop.FindPropertyRelative("trigger");

            DrawSourceTypeDropdown(pos, sourceType);

            // Handle selected source type.
            switch (sourceType.enumValueIndex) {
                case (int) SourceTypes.Property:
                    // todo crate fields instead of passing multiple params.
                    DrawInspectorForPropertySourceType(
                        pos,
                        param,
                        trigger,
                        sourceCo,
                        sourcePropIndex,
                        sourcePropertyName);
                    break;
                case (int) SourceTypes.Trigger:
                    DrawInspectorForTriggerSourceType(
                        pos,
                        trigger,
                        sourceType,
                        messageType,
                        param);
                    break;
            }
        }

        #endregion

        #region DRAW METHODS

        private void DrawInspectorForTriggerSourceType(
            Rect pos,
            SerializedProperty trigger,
            SerializedProperty sourceType,
            SerializedProperty messageType,
            SerializedProperty param) {

            // Message type source is always a trigger.
            trigger.boolValue = true;

            DrawMessageTypeDropdown(pos, messageType);

            // Draw 'param' field.
            EditorGUIUtility.labelWidth = 50;

            DrawParamField(pos, param, 2);

            // Draw 'trigger' field.
            EditorGUIUtility.labelWidth = 50;

            DrawTriggerField(pos, trigger, SourceTypes.Trigger, 2);
        }

        private void DrawInspectorForPropertySourceType(
            Rect pos,
            SerializedProperty param,
            SerializedProperty trigger,
            SerializedProperty sourceCo,
            SerializedProperty sourcePropIndex,
            SerializedProperty sourcePropertyName) {

            EditorGUIUtility.labelWidth = 50;

            DrawParamField(pos, param, 1);

            EditorGUIUtility.labelWidth = 50;

            DrawTriggerField(pos, trigger, SourceTypes.Property, 1);

            EditorGUIUtility.labelWidth = 50;

            DrawSourceComponentField(pos, sourceCo);

            EditorGUIUtility.labelWidth = 0;

            FindComponentProperties(sourceCo);

            EditorGUIUtility.labelWidth = 80;

            DrawSourcePropertyField(
                pos,
                sourceCo,
                sourcePropIndex,
                sourcePropertyName);
        }
        private void DrawSourcePropertyField(
            Rect pos,
            SerializedProperty sourceCo,
            SerializedProperty sourcePropIndex,
            SerializedProperty sourcePropertyName) {

            if (sourceCo.objectReferenceValue == null) return;

            sourcePropIndex.intValue = EditorGUI.Popup(
                new Rect(
                    pos.x,
                    pos.y + (3 * 20),
                    // rows * (row height + empty space)
                    pos.width,
                    16),
                "Property",
                sourcePropIndex.intValue,
                sourcePropNames);

            // Save selected property name.
            sourcePropertyName.stringValue =
                sourcePropNames[sourcePropIndex.intValue];
        }

        private void DrawMessageTypeDropdown(
            Rect pos,
            SerializedProperty messageType) {

            EditorGUI.PropertyField(
                new Rect(
                    pos.x,
                    pos.y + 20,
                    pos.width,
                    16),
                messageType,
                new GUIContent(
                    "Message Type",
                    "When to update the animator trigger field."));
        }

        private void DrawSourceComponentField(
            Rect pos,
            SerializedProperty sourceCo) {

            EditorGUI.PropertyField(
                new Rect(
                    pos.x,
                    pos.y + 2 * (PropHeight + PropMargin),
                    pos.width,
                    16),
                sourceCo,
                new GUIContent(
                    "Source",
                    "Component that contains the property used to update " +
                    "the animator field."));
        }

        private void DrawTriggerField(
            Rect pos,
            SerializedProperty trigger,
            SourceTypes sourceType,
            int row) {

            var disable = sourceType == SourceTypes.Trigger;

            EditorGUI.BeginDisabledGroup(disable);

            EditorGUI.PropertyField(
                new Rect(
                    pos.x + pos.width * 0.5f + 3,
                    pos.y + row * (PropHeight + PropMargin),
                    pos.width * 0.5f,
                    16),
                trigger,
                new GUIContent(
                    "Trigger",
                    "If the animator param. is a trigger. Every change of " +
                    "source property will trigger the animator param."));

            EditorGUI.EndDisabledGroup();
        }

        private void DrawParamField(
            Rect pos,
            SerializedProperty param,
            int row) {

            EditorGUI.PropertyField(
                new Rect(
                    pos.x,
                    pos.y + row * (PropHeight + PropMargin),
                    pos.width * 0.5f,
                    16),
                param,
                new GUIContent(
                    "Param",
                    "Animator parameter name to update."));
        }

        private void DrawSourceTypeDropdown(
            Rect pos,
            SerializedProperty sourceType) {

            EditorGUI.PropertyField(
                new Rect(
                    pos.x,
                    pos.y + 0 * (PropHeight + PropMargin),
                    pos.width,
                    PropHeight),
                sourceType,
                new GUIContent(
                    "Source Type",
                    "Select `Property` to update animator parameter with " +
                    "other component's property value. Select `Trigger` " +
                    "to update animator trigger field."));
        }

        #endregion

        #region METHODS
        private void FindComponentProperties(
                    SerializedProperty sourceCo) {

            // Find component properties in a selected component and display
            // as dropdown.
            if (!sourceCo.objectReferenceValue) return;

            // Get all properties from source component.
            var _sourceProperties =
                sourceCo.objectReferenceValue.GetType().GetProperties();
            // Initialize array.
            sourcePropNames = new string[_sourceProperties.Length];
            // Fill array with property names.
            for (var i = 0; i < _sourceProperties.Length; i++) {
                sourcePropNames[i] = _sourceProperties[i].Name;
            }
        }

        #endregion

    }

}
