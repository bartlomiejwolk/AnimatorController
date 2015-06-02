// Copyright (c) 2015 Bartlomiej Wolk (bartlomiejwolk@gmail.com)
// 
// This file is part of the AnimatorController extension for Unity. Licensed
// under the MIT license. See LICENSE file in the project root folder.

using AnimatorControllerEx.ReorderableList;
using UnityEditor;

namespace AnimatorControllerEx {

    [CustomEditor(typeof (AnimatorController))]
    public sealed class AnimatorControllerEditor : Editor {
        #region SERIALIZED PROPERTIES

        private SerializedProperty animator;
        private SerializedProperty animatorParams;
        private SerializedProperty description;

        #endregion SERIALIZED PROPERTIES

        #region UNITY MESSAGES

        public override void OnInspectorGUI() {
            serializedObject.Update();

            DrawVersionLabel();
            DrawDescriptionTextArea();

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(animator);

            ReorderableListGUI.Title("Animator Params");
            ReorderableListGUI.ListField(animatorParams);

            serializedObject.ApplyModifiedProperties();
        }

        private void OnEnable() {
            animator = serializedObject.FindProperty("animator");
            animatorParams = serializedObject.FindProperty("animatorParams");
            description = serializedObject.FindProperty("description");
        }

        #endregion UNITY MESSAGES

        #region INSPECTOR CONTROLS

        private void DrawDescriptionTextArea() {
            description.stringValue = EditorGUILayout.TextArea(
                description.stringValue);
        }

        private void DrawVersionLabel() {
            EditorGUILayout.LabelField(
                string.Format(
                    "{0} ({1})",
                    AnimatorController.Version,
                    AnimatorController.Extension));
        }

        #endregion INSPECTOR CONTROLS

        #region METHODS

        [MenuItem("Component/AnimatorController")]
        private static void AddAnimatorControllerComponent() {
            if (Selection.activeGameObject != null) {
                Selection.activeGameObject.AddComponent(
                    typeof (AnimatorController));
            }
        }

        #endregion METHODS
    }

}