using UnityEngine;

namespace AnimatorControllerEx {

    /// Data needed for updating a single animator property.
    ///
    /// See 'AnimatorParamDrawer'.
    // todo make fields private. Create properties.
    [System.Serializable]
    public sealed class AnimatorParam {

        /// Source type.
        [SerializeField]
        public SourceTypes _sourceType;

        // Message type.
        [SerializeField]
        public MessageTypes _messageType;
            
        /// Animator parameter to update.
        // TODO Rename to '_paramName'.
        [SerializeField]
        public string _param;

        /// If animator param. is a trigger.
        [SerializeField]
        public bool _trigger;

        /// Animator param hash.
        public int _paramHash;

        /// Source component.
        [SerializeField]
        public Component _sourceCo;

        /// Index of the selected source property in property dropdown.
        ///
        /// Property array contains names of all properties
        /// found in the source game object.
        [SerializeField]
        private int _sourcePropIndex = 0;

        /// Name of the selected source component property.
        [SerializeField]
        public string _sourcePropertyName;
    }

}