using UnityEngine;
using System.Collections;
using UnityEditor;
using Rotorz.ReorderableList;

namespace AnimatorControllerEx {

    [CustomEditor(typeof(AnimatorController))]
    public sealed class AnimatorControllerEditor: Editor {

        #region SERIALIZED PROPERTIES

        private SerializedProperty animator;
        private SerializedProperty animatorParams;
        private SerializedProperty description;

        #endregion

        #region UNITY MESSAGES

        private void OnEnable() {
            animator = serializedObject.FindProperty("animator");
            animatorParams = serializedObject.FindProperty("animatorParams");
            description = serializedObject.FindProperty("description");
        }

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

        #endregion

        #region INSPECTOR CONTROLS

        private void DrawVersionLabel() {
            EditorGUILayout.LabelField(
                string.Format(
                    "{0} ({1})",
                    AnimatorController.Version,
                    AnimatorController.Extension));
        }

        private void DrawDescriptionTextArea() {
            description.stringValue = EditorGUILayout.TextArea(
                description.stringValue);
        }

        #endregion INSPECTOR

        #region METHODS

        [MenuItem("Component/AnimatorController")]
        private static void AddAnimatorControllerComponent() {
            if (Selection.activeGameObject != null) {
                Selection.activeGameObject.AddComponent(typeof(AnimatorController));
            }
        }

        #endregion METHODS
    }
}
