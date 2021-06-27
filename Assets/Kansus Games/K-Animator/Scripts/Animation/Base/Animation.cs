using KansusGames.KansusAnimator.Animation.Audio;
using UnityEngine;

namespace KansusGames.KansusAnimator.Animation.Base
{
    /// <summary>
    /// Model that represents an animation.
    /// </summary>
    public class Animation : BaseAnimation
    {
        #region Fields

        [SerializeField]
        [Header("Audio")]
        private UIAnimationAudio sounds;

        #endregion

        #region Properties

        /// <summary>
        /// The sounds of this animation.
        /// </summary>
        public UIAnimationAudio Sounds
        {
            get { return sounds; }
            set { sounds = value; }
        }

        #endregion
    }
}