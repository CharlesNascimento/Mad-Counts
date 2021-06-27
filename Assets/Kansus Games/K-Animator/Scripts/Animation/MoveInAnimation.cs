using KansusGames.KansusAnimator.Core;
using System;
using UnityEngine;

namespace KansusGames.KansusAnimator.Animation
{
    /// <summary>
    /// Model that represents a move animation.
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "MoveIn", menuName = "Kansus Games/K-Animator/Move In", order = 1)]
    public class MoveInAnimation : Base.Animation
    {
        #region Fields

        private Vector3 beginPosition;

        private Vector3 endPosition;

        [Header("Position")]

        [SerializeField]
        private PresetPosition moveFrom = PresetPosition.UpperScreenEdge;

        [SerializeField]
        private Vector3 position;

        #endregion

        #region Properties

        /// <summary>
        /// The initial value of this animation.
        /// </summary>
        public Vector3 StartPosition
        {
            get { return beginPosition; }
            set { beginPosition = value; }
        }

        /// <summary>
        /// The final value of this animation.
        /// </summary>
        public Vector3 EndPosition
        {
            get { return endPosition; }
            set { endPosition = value; }
        }

        /// <summary>
        /// The preset position from where this animation will start.
        /// </summary>
        public PresetPosition MoveFrom
        {
            get { return moveFrom; }
            set { moveFrom = value; }
        }

        /// <summary>
        /// The current value of this animation. When the MoveFrom property is set to LocalPosition,
        /// this property can be used to specify a local position as the initial position.
        /// </summary>
        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        #endregion
    }
}