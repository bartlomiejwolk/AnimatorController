using UnityEngine;
using System.Collections;
using UnityEditor;
using Rotorz.ReorderableList;

namespace AnimatorControllerEx {

	[CustomEditor(typeof(AnimatorController))]
	public sealed class AnimatorControllerEditor: Editor {

		private SerializedProperty _animator;
		private SerializedProperty _animatorParams;

		private void OnEnable() {
			_animator = serializedObject.FindProperty("_animator");
			_animatorParams = serializedObject.FindProperty("_animatorParams");
		}

		public override void OnInspectorGUI() {
			serializedObject.Update();

			EditorGUILayout.PropertyField(_animator);

			ReorderableListGUI.Title("Animator Params");
			ReorderableListGUI.ListField(_animatorParams);

			serializedObject.ApplyModifiedProperties();
		}
	}
}
