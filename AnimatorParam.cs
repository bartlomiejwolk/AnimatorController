// Copyright (c) 2015 Bartlomiej Wolk (bartlomiejwolk@gmail.com)
// 
// This file is part of the AnimatorController extension for Unity. Licensed
// under the MIT license. See LICENSE file in the project root folder.

using UnityEngine;

namespace AnimatorControllerEx {

    /// Data needed for updating a single animator property.
    /// 
    /// See 'AnimatorParamDrawer'.
    // todo make fields private. Create properties.
    [System.Serializable]
    public sealed class AnimatorParam {

        // Message type.
        [SerializeField]
        public MessageTypes messageType;

        /// Animator param hash.
        public int paramHash;

        /// Animator parameter to update.
        [SerializeField]
        public string paramName;

        /// Source component.
        [SerializeField]
        public Component sourceCo;

        /// Name of the selected source component property.
        [SerializeField]
        public string sourcePropertyName;

        /// Source type.
        [SerializeField]
        public SourceTypes sourceType;

        /// If animator param. is a trigger.
        [SerializeField]
        public bool trigger;

        /// Index of the selected source property in property dropdown.
        /// 
        /// Property array contains names of all properties found in the source
        /// game object.
        [SerializeField]
        private int sourcePropIndex = 0;

    }

}