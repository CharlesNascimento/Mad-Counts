using System.Collections.Generic;
using UnityEngine;

namespace KansusGames.KansusAnimator.Core
{
    /// <summary>
    /// Model that holds the default configurations of the animators.
    /// </summary>
    [CreateAssetMenu(fileName = "K-Animator Configuration",
        menuName = "Kansus Games/K-Animator/K-Animator Configuration File", order = 0)]
    public class KAnimatorConfiguration : ScriptableObject
    {
        #region Fields

        private static KAnimatorConfiguration instance;

        private static Dictionary<TweenEngine, ITweener> tweenEngineDictionary;

#if LEANTWEEN
        [SerializeField]
        private TweenEngine tweener = TweenEngine.LeanTween;
#elif ITWEEN
        [SerializeField]
        private TweenEngine tweener = TweenEngine.iTween;
#elif DOTWEEN
        [SerializeField]
        private TweenEngine tweener = TweenEngine.DOTween;
#else
		[SerializeField]
		private TweenEngine tweener;
#endif

        [Range(0.5f, 10f)]
        [SerializeField]
        private float animationSpeed = 1f;

        #endregion

        #region Properties

        /// <summary>
        /// The tween engine used by the animators.
        /// </summary>
        public TweenEngine TweenEngine { get => tweener; set => tweener = value; }

        /// <summary>
        /// The reference to the tween engine object used by the animators.
        /// </summary>
        public ITweener Tweener { get => tweenEngineDictionary[tweener]; }

        /// <summary>
        /// The base speed of all animations. This value is multiplied by the specific
        /// animation speed.
        /// </summary>
        public float AnimationSpeed { get => animationSpeed; set => animationSpeed = value; }

        public static KAnimatorConfiguration Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Resources.Load<KAnimatorConfiguration>("K-Animator Configuration");
                }

                return instance;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Static constructor to initialize static fields.
        /// </summary>
        static KAnimatorConfiguration()
        {
            tweenEngineDictionary = new Dictionary<TweenEngine, ITweener>();

#if LEANTWEEN
            tweenEngineDictionary[TweenEngine.LeanTween] = Tweening.Adapter.LeanTweenAdapter.Instance;
#endif

#if ITWEEN
            tweenEngineDictionary[TweenEngine.iTween] = Tweening.Adapter.ITweenAdapter.Instance;
#endif

#if DOTWEEN
			tweenEngineDictionary[TweenEngine.DOTween] = Tweening.Adapter.DOTweenAdapter.Instance;
#endif
        }

        #endregion
    }
}
