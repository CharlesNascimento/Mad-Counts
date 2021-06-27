using System;
using UnityEngine;

namespace KansusGames.KansusAnimator.Animation
{
    /// <summary>
    /// Model that represents a scale out animation.
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "ScaleOut", menuName = "Kansus Games/K-Animator/Scale Out", order = 3)]
    public class ScaleOutAnimation : Base.Animation
    {
        #region Fields

        private Vector3 startScale = new Vector3(1f, 1f, 1f);

        [Header("Scale")]

        [SerializeField]
        private Vector3 endScale = new Vector3(0f, 0f, 0f);

        #endregion

        #region Properties

        /// <summary>
        /// The initial scale of this animation.
        /// </summary>
        public Vector3 StartScale
        {
            get { return startScale; }
            set { startScale = value; }
        }

        /// <summary>
        /// The final scale of this animation.
        /// </summary>
        public Vector3 EndScale
        {
            get { return endScale; }
            set { endScale = value; }
        }

        #endregion
    }
}