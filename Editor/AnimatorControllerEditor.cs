using UnityEngine;
using System.Collections;
using UnityEditor;
using Rotorz.ReorderableList;

namespace AnimatorControllerEx {

    [CustomEditor(typeof(AnimatorController))]
    public sealed class AnimatorControllerEditor: Editor {

        private SerializedProperty _animator;
        private SerializedProperty _animatorParams;
        private SerializedProperty description;

        private void OnEnable() {
            _animator = serializedObject.FindProperty("_animator");
            _animatorParams = serializedObject.FindProperty("_animatorParams");
            description = serializedObject.FindProperty("description");
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();

            DrawVersionLabel();
            DrawDescriptionTextArea();

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(_animator);

            ReorderableListGUI.Title("Animator Params");
            ReorderableListGUI.ListField(_animatorParams);

            serializedObject.ApplyModifiedProperties();
        }

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

        [MenuItem("Component/MyNamespace/AnimatorController")]
        private static void AddAnimatorControllerComponent() {
            if (Selection.activeGameObject != null) {
                Selection.activeGameObject.AddComponent(typeof(AnimatorController));
            }
        }

        #endregion METHODS
    }
}
