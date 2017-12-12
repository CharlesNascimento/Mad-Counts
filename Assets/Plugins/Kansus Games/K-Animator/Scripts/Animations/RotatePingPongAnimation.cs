using System;
using UnityEngine;

namespace KansusAnimator.Animations
{
    /// <summary>
    /// Model that represents a "rotate ping-pong" animation.
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "RotatePingPongAnimation", menuName = "Kansus Games/K-Animator/Rotate Ping-Pong Animation", order = 2)]
    public class RotatePingPongAnimation : PingPongAnimation
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
        /// The initial rotation of the ping-pong animation.
        /// </summary>
        public Vector3 StartRotation
        {
            get { return startRotation; }
            set { startRotation = value; }
        }

        /// <summary>
        /// The final rotation of the ping-pong animation.
        /// </summary>
        public Vector3 EndRotation
        {
            get { return endRotation; }
            set { endRotation = value; }
        }

        /// <summary>
        /// The current rotation of the ping-pong animation.
        /// </summary>
        public Vector3 Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        #endregion
    }
}