using System;
using UnityEngine;

namespace KansusGames.KansusAnimator.Animation
{
    /// <summary>
    /// Model that represents a rotate out animation.
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "RotateOut", menuName = "Kansus Games/K-Animator/Rotate Out", order = 2)]
    public class RotateOutAnimation : Base.Animation
    {
        #region Fields

        private Vector3 startRotation;

        private Vector3 endRotation;

        [Header("Rotation")]

        [SerializeField]
        private Vector3 rotation;

        #endregion

        #region Properties

        /// <summary>
        /// The initial rotation of this animation.
        /// </summary>
        public Vector3 StartRotation
        {
            get { return startRotation; }
            set { startRotation = value; }
        }

        /// <summary>
        /// The final rotation of this animation.
        /// </summary>
        public Vector3 EndRotation
        {
            get { return endRotation; }
            set { endRotation = value; }
        }

        /// <summary>
        /// The current rotation of this animation.
        /// </summary>
        public Vector3 Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        #endregion
    }
}