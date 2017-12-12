using System;
using UnityEngine;

namespace KansusAnimator.Animations
{
    /// <summary>
    /// Model that holds the sounds of an ping pong animation.
    /// </summary>
    [Serializable]
    public class PingPongAnimationAudio
    {
        #region Fields

        private AudioSource audioSource;

        [SerializeField]
        private AudioClip audioClip;

        [SerializeField]
        private bool loop;

        #endregion

        #region Properties

        /// <summary>
        /// The audio source that will play this sound
        /// </summary>
        public AudioSource Source
        {
            get { return audioSource; }
            set { audioSource = value; }
        }

        /// <summary>
        /// The audio clip that plays during the ping-pong animation.
        /// </summary>
        public AudioClip Clip
        {
            get { return audioClip; }
            set { audioClip = value; }
        }

        /// <summary>
        /// Indicates whether this sound should play in loop mode during the ping-pong animation.
        /// </summary>
        public bool Loop
        {
            get { return loop; }
            set { loop = value; }
        }

        #endregion
    }
}