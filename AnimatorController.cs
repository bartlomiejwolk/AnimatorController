// Copyright (c) 2015 Bartlomiej Wolk (bartlomiejwolk@gmail.com)
// 
// This file is part of the AnimatorController extension for Unity. Licensed
// under the MIT license. See LICENSE file in the project root folder.

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace AnimatorControllerEx {

    /// Update animator parameters with component property values.
    /// 
    /// Make sure that passed animator property and selected component property
    /// type are of the same type.
    /// 
    /// When 'Trigger' checkbox is selected, make sure that the animator
    /// parameter is of trigger type.
    public sealed class AnimatorController : MonoBehaviour {
        #region CONSTANTS

        public const string Extension = "AnimatorController";
        public const string Version = "v0.1.0";

        #endregion CONSTANTS

        #region FIELDS

        [SerializeField]
        private string description = "Description";

        /// Previous values of the component property. Necessery for the
        /// 'Trigger' option.
        private object[] prevPropValues;

        /// Metadata of the selected source properties.
        private PropertyInfo[] sourcePropInfo;

        #endregion FIELDS

        #region INSPECTOR FIELDS

        /// List of animator component params.
        // todo make fields private
        public List<AnimatorParam> animatorParams = new List<AnimatorParam>();

        /// Animator component.
        [SerializeField]
        private Animator animator;

        #endregion INSPECTOR FIELDS

        #region SERIALIZED PROPERTIES

        public string Description {
            get { return description; }
            set { description = value; }
        }

        #endregion SERIALIZED PROPERTIES

        #region UNITY MESSAGES

        private void Awake() {
            if (!animator) {
                Utilities.MissingReference(this, "_animator");
            }

            // Initialize arrays.
            sourcePropInfo = new PropertyInfo[animatorParams.Count];
            prevPropValues = new object[animatorParams.Count];
            // Array must be initialized with values. Otherwise there'll be a
            // null reference exception.
            for (var i = 0; i < animatorParams.Count; i++) {
                prevPropValues[i] = null;
            }

            // Get data for all AnimatorParam class objects.
            for (var i = 0; i < animatorParams.Count; i++) {
                // For different source types there're different things to do.
                switch (animatorParams[i].sourceType) {
                    case SourceTypes.Property:
                        // Get metadata of the selected source property.
                        sourcePropInfo[i] = animatorParams[i].sourceCo.GetType()
                            .GetProperty(animatorParams[i].sourcePropertyName);
                        break;

                    case SourceTypes.Trigger:
                        break;
                }
                // Get animator parameter hash.
                animatorParams[i].paramHash =
                    Animator.StringToHash(animatorParams[i].paramName);
            }
        }

        private void OnTriggerEnter() {
            for (var i = 0; i < animatorParams.Count; i++) {
                // Handle OnTriggerEnter message type.
                if (animatorParams[i].messageType ==
                    MessageTypes.OnTriggerEnter) {
                    // Send trigger to Animator param.
                    animator.SetTrigger(animatorParams[i].paramHash);
                }
            }
        }

        private void OnTriggerExit() {
            for (var i = 0; i < animatorParams.Count; i++) {
                // Handle OnTriggerExit message type.
                if (animatorParams[i].messageType ==
                    MessageTypes.OnTriggerExit) {
                    // Send trigger to Animator param.
                    animator.SetTrigger(animatorParams[i].paramHash);
                }
            }
        }

        private void Update() {
            // Type of the source property value.
            Type[] sourceType;
            // Value of the source property.
            object[] sourceValue;

            // Initialize arrays.
            sourceType = new Type[animatorParams.Count];
            sourceValue = new object[animatorParams.Count];

            // Update animator params for each AnimatorParams class object.
            for (var i = 0; i < animatorParams.Count; i++) {
                // Do that only when a components is used as a source.
                if (animatorParams[i].sourceType == SourceTypes.Property) {
                    sourceType[i] = sourcePropInfo[i].PropertyType;
                    sourceValue[i] = sourcePropInfo[i].GetValue(
                        animatorParams[i].sourceCo,
                        null);
                    UpdateAnimatorParams(
                        animatorParams[i].paramHash,
                        animatorParams[i].trigger,
                        sourceType[i],
                        sourceValue[i],
                        // Pass by value so that it can be updated.
                        ref prevPropValues[i]);
                }
            }
        }

        #endregion UNITY MESSAGES

        #region METHODS

        /// Update animator parameters for 'Component' source type.
        /// 
        /// \param paramHash Parameter hash. \param trigger If animator
        /// parameter is a trigger. \param propType Type of the component
        /// property. \param propValue Value of the component property. \param
        /// prevPropValue Last remembered component property value.
        // todo extract methods
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
                            // Check if property value changed since last parameter
                            // update.
                        if ((int) propValue != (int) prevPropValue) {
                            animator.SetTrigger(paramHash);
                        }
                        prevPropValue = propValue;
                    }
                    else {
                        animator.SetInteger(paramHash, (int) propValue);
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
                            // Check if property value changed since last parameter
                            // update.
                        if ((float) propValue != (float) prevPropValue) {
                            animator.SetTrigger(paramHash);
                        }
                        prevPropValue = propValue;
                    }
                    else {
                        animator.SetFloat(paramHash, (float) propValue);
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
                            // Check if property value changed since last parameter
                            // update.
                        if ((bool) propValue != (bool) prevPropValue) {
                            animator.SetTrigger(paramHash);
                        }
                        prevPropValue = propValue;
                    }
                    else {
                        animator.SetBool(paramHash, (bool) propValue);
                    }
                    break;
            }
        }

        #endregion METHODS
    }

}