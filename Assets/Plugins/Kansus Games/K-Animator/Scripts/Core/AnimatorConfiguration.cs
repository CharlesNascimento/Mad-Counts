using System.Collections.Generic;
using UnityEngine;

namespace KansusAnimator
{
    /// <summary>
    /// Model that holds the default configurations of the animators.
    /// </summary>
    [CreateAssetMenu(fileName = "K-Animator Configuration", menuName = "Kansus Games/K-Animator/K-Animator Configuration File", order = 0)]
    public class AnimatorConfiguration : ScriptableObject
    {
        #region Fields

        private static Dictionary<TweenEngine, ITweener> tweenEngineDictionary;

        [Header("General")]

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

        [SerializeField]
        private bool pingPongAfterInAnimation = true;

        [SerializeField]
        private AnimationInterruptionBehaviour interruptionBehavior = AnimationInterruptionBehaviour.Interrupt;

        [Header("Initial animation")]

        [SerializeField]
        private bool startAutomatically = true;

        [SerializeField]
        private InitialAnimationType initialAnimationType = InitialAnimationType.All;

        [SerializeField]
        private float initialAnimationIdleDuration = 5f;

        #endregion

        #region Properties

        /// <summary>
        /// The tween engine used by the animators.
        /// </summary>
        public TweenEngine TweenEngine
        {
            get
            {
                return tweener;
            }
            set
            {
                tweener = value;
            }
        }

        /// <summary>
        /// The reference to the tween engine object used by the animators.
        /// </summary>
        public ITweener Tweener
        {
            get
            {
                return tweenEngineDictionary[tweener];
            }
        }

        /// <summary>
        /// The base speed of all animations. This value is multiplied by the specific
        /// animation speed.
        /// </summary>
        public float AnimationSpeed
        {
            get
            {
                return animationSpeed;
            }
            set
            {
                animationSpeed = value;
            }
        }

        /// <summary>
        /// Indicates whether the Ping Pong animation should start automatically after
        /// the end of the In animation.
        /// </summary>
        public bool PingPongAfterInAnimation
        {
            get
            {
                return pingPongAfterInAnimation;
            }
            set
            {
                pingPongAfterInAnimation = value;
            }
        }

        /// <summary>
        /// The behavior of the animator if it must start a new animation, but there is another
        /// one currently running in the same game object.
        /// </summary>
        public AnimationInterruptionBehaviour InterruptionBehavior
        {
            get
            {
                return interruptionBehavior;
            }
            set
            {
                interruptionBehavior = value;
            }
        }

        /// <summary>
        /// Indicates whether the animators will start automatically when added to the scene.
        /// </summary>
        public bool StartAutomatically
        {
            get
            {
                return startAutomatically;
            }
            set
            {
                startAutomatically = value;
            }
        }

        /// <summary>
        /// If the animators are set to start automatically, this indicates
        /// the animation types that will run.
        /// </summary>
        public InitialAnimationType InitialAnimationType
        {
            get
            {
                return initialAnimationType;
            }
            set
            {
                initialAnimationType = value;
            }
        }

        /// <summary>
        /// The duration of the initial animation idle state, if the animators
        /// are set to start automatically.
        /// </summary>
        public float InitialAnimationIdleDuration
        {
            get
            {
                return initialAnimationIdleDuration;
            }
            set
            {
                initialAnimationIdleDuration = value;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Static constructor to initialize static fields.
        /// </summary>
        static AnimatorConfiguration()
        {
            tweenEngineDictionary = new Dictionary<TweenEngine, ITweener>();

#if LEANTWEEN
            tweenEngineDictionary[TweenEngine.LeanTween] = Adapters.LeanTweenAdapter.Instance;
#endif

#if ITWEEN
            tweenEngineDictionary[TweenEngine.iTween] = Adapters.ITweenAdapter.Instance;
#endif

#if DOTWEEN
			tweenEngineDictionary[TweenEngine.DOTween] = Adapters.DOTweenAdapter.Instance;
#endif
        }

        #endregion
    }
}