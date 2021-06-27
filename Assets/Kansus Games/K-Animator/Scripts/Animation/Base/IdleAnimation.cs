using KansusGames.KansusAnimator.Animation.Audio;
using UnityEngine;

namespace KansusGames.KansusAnimator.Animation.Base
{
    /// <summary>
    /// Model that represents an idle animation.
    /// </summary>
    public class IdleAnimation : BaseAnimation
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