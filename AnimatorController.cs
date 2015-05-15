using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace AnimatorControllerEx {

	/// Update animator parameters with component property values.
	///
	/// Make sure that passed animator property and selected component
	/// property type are of the same type.
	///
	/// When 'Trigger' checkbox is selected, make sure that the animator
	/// parameter is of trigger type.
	public class AnimatorController : MonoBehaviour {

		public enum SourceTypes { Component, Message }

		public enum MessageTypes { OnTriggerEnter, OnTriggerExit }

	    /// Animator component.
		[SerializeField]
		Animator _animator;

		/// List of animator component params.
		public List<AnimatorParam> _animatorParams = new List<AnimatorParam>();

		/// Metadata of the selected source properties.
		private PropertyInfo[] _sourcePropInfo;

		/// Previous values of the component property.
		/// Necessery for the 'Trigger' option.
		private object[] _prevPropValues;

		private void Awake() {
			if (!_animator) {
				Utilities.MissingReference(this, "_animator");
			}

			// Initialize arrays.
			_sourcePropInfo = new PropertyInfo[_animatorParams.Count];
			_prevPropValues = new object[_animatorParams.Count];
			// Array must be initialized with values. Otherwise there'll be
			// a null reference exception.
			for (int i = 0; i < _animatorParams.Count; i++) {
				_prevPropValues[i] = null;
			}

			// Get data for all AnimatorParam class objects.
			for (int i = 0; i < _animatorParams.Count; i++) {
				// For different source types there're different things to do.
				switch (_animatorParams[i]._sourceType) {
					case SourceTypes.Component:
						// Get metadata of the selected source property.
						_sourcePropInfo[i] = _animatorParams[i]._sourceCo.GetType()
							.GetProperty(_animatorParams[i]._sourcePropertyName);
						break;
					case SourceTypes.Message:
						break;
				}
				// Get animator parameter hash.
				_animatorParams[i]._paramHash =
					Animator.StringToHash(_animatorParams[i]._param);
			}
		}

		private void Update () {
			// Type of the source property value.
			Type[] sourceType;
			// Value of the source property.
			object[] sourceValue;

			// Initialize arrays.
			sourceType = new Type[_animatorParams.Count];
			sourceValue = new object[_animatorParams.Count];

			// Update animator params for each AnimatorParams class object.
			for (int i = 0; i < _animatorParams.Count; i++) {
				// Do that only when a components is used as a source.
				if (_animatorParams[i]._sourceType == SourceTypes.Component) {
					sourceType[i] = _sourcePropInfo[i].PropertyType;
					sourceValue[i] = _sourcePropInfo[i].GetValue(
							_animatorParams[i]._sourceCo,
							null);
					UpdateAnimatorParams(
							_animatorParams[i]._paramHash,
							_animatorParams[i]._trigger,
							sourceType[i],
							sourceValue[i],
							// Pass by value so that it can be updated.
							ref _prevPropValues[i]);
				}
			}
		}

		/// Update animator parameters for 'Component' source type.
		///
		/// \param paramHash Parameter hash.
		/// \param trigger If animator parameter is a trigger.
		/// \param propType Type of the component property.
		/// \param propValue Value of the component property.
		/// \param prevPropValue Last remembered component property value.
		private void UpdateAnimatorParams(
				int paramHash,
				bool trigger,
				Type propType,
				object propValue,
				ref object prevPropValue) {

			switch (propType.ToString()) {
				case "System.Int32":
					// Handle parameter of trigger type.
					if (trigger) {
						if (prevPropValue == null) {
							// Update previous property value;
							prevPropValue = propValue;
							break;
						}
						// Check if property value changed since last parameter update.
						else if ((int)propValue != (int)prevPropValue) {
							_animator.SetTrigger(paramHash);
						}
						prevPropValue = propValue;
					}
					else {
						_animator.SetInteger(paramHash, (int)propValue);
					}
					break;
				case "System.Single":
					// Handle parameter of trigger type.
					if (trigger) {
						// If 'prevPropValue' does not hold a value, don't
						// check condition below.
						if (prevPropValue == null) {
							// Update previous property value;
							prevPropValue = propValue;
							break;
						}
						// Check if property value changed since last parameter update.
						else if ((float)propValue != (float)prevPropValue) {
							_animator.SetTrigger(paramHash);
						}
						prevPropValue = propValue;
					}
					else {
						_animator.SetFloat(paramHash, (float)propValue);
					}
					break;
				case "System.Boolean":
					// Handle parameter of trigger type.
					if (trigger) {
						// If 'prevPropValue' does not hold a value, don't
						// check condition below.
						if (prevPropValue == null) {
							// Update previous property value;
							prevPropValue = propValue;
							break;
						}
						// Check if property value changed since last parameter update.
						else if ((bool)propValue != (bool)prevPropValue) {
							_animator.SetTrigger(paramHash);
						}
						prevPropValue = propValue;
					}
					else {
						_animator.SetBool(paramHash, (bool)propValue);
					}
					break;
			}
		}

		void OnTriggerEnter() {
			for (int i = 0; i < _animatorParams.Count; i++) {
				// Handle OnTriggerEnter message type.
				if (_animatorParams[i]._messageType ==
						MessageTypes.OnTriggerEnter) {
					// Send trigger to Animator param.
					_animator.SetTrigger(_animatorParams[i]._paramHash);
				}
			}
		}

		void OnTriggerExit() {
			for (int i = 0; i < _animatorParams.Count; i++) {
				// Handle OnTriggerExit message type.
				if (_animatorParams[i]._messageType ==
						MessageTypes.OnTriggerExit) {
					// Send trigger to Animator param.
					_animator.SetTrigger(_animatorParams[i]._paramHash);
				}
			}
		}
	}

}
