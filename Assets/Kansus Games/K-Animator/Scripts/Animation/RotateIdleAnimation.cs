using KansusGames.KansusAnimator.Animation.Base;
using System;
using UnityEngine;

namespace KansusGames.KansusAnimator.Animation
{
    /// <summary>
    /// Model that represents a rotate idle animation.
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "RotateIdle", menuName = "Kansus Games/K-Animator/Rotate Idle", order = 2)]
    public class RotateIdleAnimation : IdleAnimation
    {
        #region Fields

        private Vector3 rotation;

        [Header("Rotation")]

        [SerializeField]
        private Vector3 startRotation;

        [SerializeField]
        private Vector3 endRotation;

        #endregion

        #region Properties

        /// <summary>
        /// The initial rotation of the idle animation.
        /// </summary>
        public Vector3 StartRotation
        {
            get { return startRotation; }
            set { startRotation = value; }
        }

        /// <summary>
        /// The final rotation of the idle animation.
        /// </summary>
        public Vector3 EndRotation
        {
            get { return endRotation; }
            set { endRotation = value; }
        }

        /// <summary>
        /// The current rotation of the idle animation.
        /// </summary>
        public Vector3 Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        #endregion
    }
}