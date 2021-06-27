using System;
using UnityEngine;

namespace KansusGames.KansusAnimator.Animation.Audio
{
    /// <summary>
    /// Model that holds the sounds of an animation.
    /// </summary>
    [Serializable]
    public class UIAnimationAudio
    {
        #region Fields

        [SerializeField]
        private AudioClip begin;

        [SerializeField]
        private AudioClip afterDelay;

        [SerializeField]
        private AudioClip end;

        #endregion

        #region Properties

        /// <summary>
        /// The audio clip played when the animation begins.
        /// </summary>
        public AudioClip Begin
        {
            get { return begin; }
            set { begin = value; }
        }

        /// <summary>
        /// The audio clip played after the animation delay.
        /// </summary>
        public AudioClip AfterDelay
        {
            get { return afterDelay; }
            set { afterDelay = value; }
        }

        /// <summary>
        /// The audio clip played when the animation finished.
        /// </summary>
        public AudioClip End
        {
            get { return end; }
            set { end = value; }
        }

        #endregion
    }
}