using UnityEngine;
using System.Collections;
using UnityEditor;
using Rotorz.ReorderableList;

namespace OneDayGame {

	[CustomEditor(typeof(AnimatorController))]
	public class AnimatorControllerEditor: GameComponentEditor {

		private SerializedProperty _animator;
		private SerializedProperty _animatorParams;

		public override void OnEnable() {
			base.OnEnable();

			_animator = serializedObject.FindProperty("_animator");
			_animatorParams = serializedObject.FindProperty("_animatorParams");
		}

		public override void OnInspectorGUI() {
			base.OnInspectorGUI();
			//AnimatorController script = (AnimatorController)target;
			serializedObject.Update();

			EditorGUILayout.PropertyField(_animator);
			ReorderableListGUI.Title("Animator Params");
			ReorderableListGUI.ListField(_animatorParams);

			serializedObject.ApplyModifiedProperties();
			// Save changes
			/*if (GUI.changed) {
				EditorUtility.SetDirty(script);
			}*/
		}
	}
}
