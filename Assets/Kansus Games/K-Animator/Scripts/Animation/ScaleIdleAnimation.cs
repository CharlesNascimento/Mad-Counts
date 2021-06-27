using KansusGames.KansusAnimator.Animation.Base;
using System;
using UnityEngine;

namespace KansusGames.KansusAnimator.Animation
{
    /// <summary>
    /// Model that represents a scale idle animation.
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "ScaleIdle", menuName = "Kansus Games/K-Animator/Scale Idle", order = 4)]
    public class ScaleIdleAnimation : IdleAnimation
    {
        #region Fields

        private Vector3 scale;

        [Header("Scale")]

        [SerializeField]
        private Vector3 startScale = new Vector3(1f, 1f, 1f);

        [SerializeField]
        private Vector3 endScale = new Vector3(1.2f, 1.2f, 1.2f);

        #endregion

        #region Properties

        /// <summary>
        /// The initial scale of the idle animation.
        /// </summary>
        public Vector3 StartScale
        {
            get { return startScale; }
            set { startScale = value; }
        }

        /// <summary>
        /// The final scale of the idle animation.
        /// </summary>
        public Vector3 EndScale
        {
            get { return endScale; }
            set { endScale = value; }
        }

        /// <summary>
        /// The current scale of the idle animation.
        /// </summary>
        public Vector3 Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        #endregion
    }
}