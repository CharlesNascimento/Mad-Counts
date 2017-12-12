using UnityEngine;

namespace KansusAnimator.Animations
{
    /// <summary>
    /// Base model for an animation.
    /// </summary>
    public abstract class BaseAnimation : ScriptableObject
    {
        #region Fields

        private bool hasBegan = false;
        private bool isAnimating = false;
        private bool isDone = false;
        private bool isEnabled = true;

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
        /// Indicates whether this animations has began, but is not necessarily running.
        /// </summary>
        public bool HasBegan
        {
            get { return hasBegan; }
            set { hasBegan = value; }
        }

        /// <summary>
        /// Indicates whether this animations is currently running.
        /// </summary>
        public bool IsAnimating
        {
            get { return isAnimating; }
            set { isAnimating = value; }
        }

        /// <summary>
        /// Indicates whether this animations has finished.
        /// </summary>
        public bool IsDone
        {
            get { return isDone; }
            set { isDone = value; }
        }

        /// <summary>
        /// Indicates whether this animations is enabled.
        /// </summary>
        public bool IsEnabled
        {
            get { return isEnabled; }
        }

        /// <summary>
        /// The delay in seconds before the animation starts.
        /// </summary>
        public float Delay
        {
            get { return delay; }
            set { delay = value; }
        }

        /// <summary>
        /// The duration of the animation in seconds.
        /// </summary>
        public float Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        /// <summary>
        /// The easing function used to interpolate the animated value.
        /// </summary>
        public EaseType EaseType
        {
            get { return easeType; }
            set { easeType = value; }
        }

        #endregion
    }
}