using System;
using UnityEngine;

namespace KansusAnimator.Animations
{
    /// <summary>
    /// Model that represents a "fade" animation.
    /// </summary>
    [Serializable]
    public class FadeAnimation : Animation
    {
        #region Fields

        private float fade;

        [Header("Fade")]

        [SerializeField]
        private bool fadeChildren;

        #endregion

        #region Properties

        /// <summary>
        /// The current animation fade.
        /// </summary>
        public float Fade
        {
            get { return fade; }
            set { fade = value; }
        }

        /// <summary>
        /// Indicates whether the child game objects will be affected by the animation.
        /// </summary>
        public bool FadeChildren
        {
            get { return fadeChildren; }
            set { fadeChildren = value; }
        } 

        #endregion
    }
}