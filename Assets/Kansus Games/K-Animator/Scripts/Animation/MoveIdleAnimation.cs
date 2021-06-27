using KansusGames.KansusAnimator.Animation.Base;
using KansusGames.KansusAnimator.Core;
using System;
using UnityEngine;

namespace KansusGames.KansusAnimator.Animation
{
    /// <summary>
    /// Model that represents an idle move animation.
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "MoveIdle", menuName = "Kansus Games/K-Animator/Move Idle", order = 1)]
    public class MoveIdleAnimation : IdleAnimation
    {
        #region Fields

        private Vector3 position;

        [Header("Position")]

        [SerializeField]
        private Vector3 startPosition = Vector3.zero;

        [SerializeField]
        private Vector3 endPosition = Vector3.zero;

        [SerializeField]
        private MoveContext movePositionSpace = MoveContext.Self;

        #endregion

        #region Properties

        /// <summary>
        /// The current position of the idle animation.
        /// </summary>
        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
        /// The initial position of the idle animation.
        /// </summary>
        public Vector3 StartPosition
        {
            get { return startPosition; }
            set { startPosition = value; }
        }

        /// <summary>
        /// The final position of the idle animation.
        /// </summary>
        public Vector3 EndPosition
        {
            get { return endPosition; }
            set { endPosition = value; }
        }

        /// <summary>
        /// The space of the idle move animation.
        /// </summary>
        public MoveContext MovePositionSpace
        {
            get { return movePositionSpace; }
            set { movePositionSpace = value; }
        }

        #endregion
    }
}