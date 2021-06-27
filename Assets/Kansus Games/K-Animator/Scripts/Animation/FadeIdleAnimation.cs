using KansusGames.KansusAnimator.Animation.Base;
using System;
using UnityEngine;

namespace KansusGames.KansusAnimator.Animation
{
    /// <summary>
    /// Model that represents a fade idle animation.
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "FadeIdle", menuName = "Kansus Games/K-Animator/Fade Animation", order = 6)]
    public class FadeIdleAnimation : IdleAnimation
    {
        #region Fields
        
        private float fade;

        [Header("Fade")]

        [SerializeField]
        private bool fadeChildren;

        [SerializeField]
        private float startFade;

        [SerializeField]
        private float endFade = 1f;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether the child game objects will be affected by this animation.
        /// </summary>
        public bool FadeChildren
        {
            get { return fadeChildren; }
            set { fadeChildren = value; }
        }

        /// <summary>
        /// The initial fade of this idle animation.
        /// </summary>
        public float StartFade
        {
            get { return startFade; }
            set { startFade = value; }
        }

        /// <summary>
        /// The final fade of this idle animation.
        /// </summary>
        public float EndFade
        {
            get { return endFade; }
            set { endFade = value; }
        }

        /// <summary>
        /// The current animation fade.
        /// </summary>
        public float Fade
        {
            get { return fade; }
            set { fade = value; }
        }

        #endregion
    }
}