using System;
using UnityEngine;

namespace KansusAnimator.Animations
{
    /// <summary>
    /// Model that represents a "move ping-pong" animation.
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "MovePingPongAnimation", menuName = "Kansus Games/K-Animator/Move Ping-Pong Animation", order = 1)]
    public class MovePingPongAnimation : PingPongAnimation
    {
        #region Fields

        private Vector3 position;

        [Header("Position")]

        [SerializeField]
        private Vector3 startPosition = Vector3.zero;

        [SerializeField]
        private Vector3 endPosition = Vector3.zero;

        #endregion

        #region Properties

        /// <summary>
        /// The current position of the ping-pong animation.
        /// </summary>
        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
        /// The initial position of the ping-pong animation.
        /// </summary>
        public Vector3 StartPosition
        {
            get { return startPosition; }
            set { startPosition = value; }
        }

        /// <summary>
        /// The final position of the ping-pong animation.
        /// </summary>
        public Vector3 EndPosition
        {
            get { return endPosition; }
            set { endPosition = value; }
        }

        #endregion
    }
}