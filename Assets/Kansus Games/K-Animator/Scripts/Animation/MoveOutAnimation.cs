using KansusGames.KansusAnimator.Core;
using System;
using UnityEngine;

namespace KansusGames.KansusAnimator.Animation
{
    /// <summary>
    /// Model that represents a move out animation.
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "MoveOut", menuName = "Kansus Games/K-Animator/Move Out", order = 1)]
    public class MoveOutAnimation : Base.Animation
    {
        #region Fields

        private Vector3 beginPosition;

        private Vector3 endPosition;

        [Header("Position")]

        [SerializeField]
        private PresetPosition moveTo = PresetPosition.UpperScreenEdge;

        [SerializeField]
        private Vector3 position;

        #endregion

        #region Properties

        /// <summary>
        /// The initial position of this animation.
        /// </summary>
        public Vector3 StartPosition
        {
            get { return beginPosition; }
            set { beginPosition = value; }
        }

        /// <summary>
        /// The final position of this animation.
        /// </summary>
        public Vector3 EndPosition
        {
            get { return endPosition; }
            set { endPosition = value; }
        }

        /// <summary>
        /// The preset position to where this animation will go.
        /// </summary>
        public PresetPosition MoveTo
        {
            get { return moveTo; }
            set { moveTo = value; }
        }

        /// <summary>
        /// The current position of this animation.
        /// </summary>
        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        #endregion
    }
}