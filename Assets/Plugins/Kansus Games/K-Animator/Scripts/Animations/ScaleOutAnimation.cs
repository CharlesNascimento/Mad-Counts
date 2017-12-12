using System;
using UnityEngine;

namespace KansusAnimator.Animations
{
    /// <summary>
    /// Model that represents a "scale out" animation.
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "ScaleOutAnimation", menuName = "Kansus Games/K-Animator/Scale Out Animation", order = 3)]
    public class ScaleOutAnimation : Animation
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