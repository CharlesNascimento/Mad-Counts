using KansusGames.KansusAnimator.Core;
using UnityEngine;

namespace KansusGames.KansusAnimator.Animation.Base
{
    /// <summary>
    /// Base model for an animation.
    /// </summary>
    public abstract class BaseAnimation : ScriptableObject
    {
        #region Fields

        [Header("General")]

        [SerializeField]
        private float delay;

        [SerializeField]
        private float duration = 1f;

        [SerializeField]
        private EaseType easeType = EaseType.linear;

        #endregion

        #region Properties

        /// <summary>
        /// The delay in seconds before the animation starts.
        /// </summary>
        public float Delay { get => delay; set => delay = value; }

        /// <summary>
        /// The duration of the animation in seconds.
        /// </summary>
        public float Duration { get => duration; set => duration = value; }

        /// <summary>
        /// The easing function used to interpolate the animated value.
        /// </summary>
        public EaseType EaseType { get => easeType; set => easeType = value; }

        #endregion
    }
}