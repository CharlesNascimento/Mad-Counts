using UnityEngine;

namespace KansusAnimator.Animations
{
    /// <summary>
    /// Model that represents a ping pong animation.
    /// </summary>
    public class PingPongAnimation : BaseAnimation
    {
        #region Fields
        
        [Header("Audio")]
        [SerializeField]
        private PingPongAnimationAudio sounds;

        #endregion

        #region Properties
        
        /// <summary>
        /// The sounds of this animation.
        /// </summary>
        public PingPongAnimationAudio Sound
        {
            get { return sounds; }
            set { sounds = value; }
        }

        #endregion
    }
}