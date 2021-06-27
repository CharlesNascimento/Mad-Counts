using System;
using UnityEngine;

namespace KansusGames.KansusAnimator.Animation
{
    /// <summary>
    /// Model that represents a scale in animation.
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "ScaleIn", menuName = "Kansus Games/K-Animator/Scale In", order = 3)]
    public class ScaleInAnimation : Base.Animation
    {
        #region Fields

        [Header("Scale")]

        [SerializeField]
        private Vector3 startScale = new Vector3(0f, 0f, 0f);

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

        #endregion
    }
}