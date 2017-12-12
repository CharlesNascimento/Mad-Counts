using System;
using UnityEngine;

namespace KansusAnimator.Animations
{
    /// <summary>
    /// Model that represents a "fade ping-pong" animation.
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "FadePingPongAnimation", menuName = "Kansus Games/K-Animator/Fade Ping-Pong Animation", order = 6)]
    public class FadePingPongAnimation : PingPongAnimation
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
        /// The initial fade of this ping-pong animation.
        /// </summary>
        public float StartFade
        {
            get { return startFade; }
            set { startFade = value; }
        }

        /// <summary>
        /// The final fade of this ping-pong animation.
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