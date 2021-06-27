using KansusGames.KansusAnimator.Animation;
using KansusGames.KansusAnimator.Animation.Base;
using KansusGames.KansusAnimator.Attribute;
using KansusGames.KansusAnimator.Core;
using KansusGames.KansusAnimator.Extension;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KansusGames.KansusAnimator.Animator
{
    /// <summary>
    /// Special animator that manages many other animators.
    /// </summary>
    [AddComponentMenu("Kansus Games/K-Animator/K-Animator")]
    [Disallow(typeof(MoveAnimator), typeof(RotateAnimator), typeof(ScaleAnimator), typeof(FadeAnimator))]
    [DisallowMultipleComponent]
    public class KAnimator : AnimatorBehaviour, IAnimationObserver
    {
        #region Fields

        private const AnimationContext defaultContext = AnimationContext.Self;

        private AudioManager audioManager;

        private float initialAnimationEndTime = 0;
        private int inAnimationsPlayingCount = 0;
        private int outAnimationsPlayingCount = 0;
        private bool isInitialized;

        #endregion

        #region Fields - Animators

        private MoveAnimator moveAnimator;
        private RotateAnimator rotateAnimator;
        private ScaleAnimator scaleAnimator;
        private FadeAnimator fadeAnimator;

        #endregion

        #region Fields - Animation

        [SerializeField]
        private MoveInAnimation moveInAnimation;

        [SerializeField]
        private MoveOutAnimation moveOutAnimation;

        [SerializeField]
        private MoveIdleAnimation moveIdleAnimation;

        [SerializeField]
        private RotateInAnimation rotateInAnimation;

        [SerializeField]
        private RotateOutAnimation rotateOutAnimation;

        [SerializeField]
        private RotateIdleAnimation rotateIdleAnimation;

        [SerializeField]
        private FadeInAnimation fadeInAnimation;

        [SerializeField]
        private FadeOutAnimation fadeOutAnimation;

        [SerializeField]
        private FadeIdleAnimation fadeIdleAnimation;

        [SerializeField]
        private ScaleInAnimation scaleInAnimation;

        [SerializeField]
        private ScaleOutAnimation scaleOutAnimation;

        [SerializeField]
        private ScaleIdleAnimation scaleIdleAnimation;

        #endregion

        #region Properties

        /// <summary>
        /// The "move in" animation this animator holds.
        /// </summary>
        public MoveInAnimation MoveInAnimation { get => moveInAnimation; set => moveInAnimation = value; }

        /// <summary>
        /// The "move out" animation this animator holds.
        /// </summary>
        public MoveOutAnimation MoveOutAnimation { get => moveOutAnimation; set => moveOutAnimation = value; }

        /// <summary>
        /// The "move idle" animation this animator holds.
        /// </summary>
        public MoveIdleAnimation MoveIdleAnimation { get => moveIdleAnimation; set => moveIdleAnimation = value; }

        /// <summary>
        /// The "rotate in" animation this animator holds.
        /// </summary>
        public RotateInAnimation RotateInAnimation { get => rotateInAnimation; set => rotateInAnimation = value; }

        /// <summary>
        /// The "rotate out" animation this animator holds.
        /// </summary>
        public RotateOutAnimation RotateOutAnimation { get => rotateOutAnimation; set => rotateOutAnimation = value; }

        /// <summary>
        /// The "rotate idle" animation this animator holds.
        /// </summary>
        public RotateIdleAnimation RotateIdleAnimation { get => rotateIdleAnimation; set => rotateIdleAnimation = value; }

        /// <summary>
        /// The "scale in" animation this animator holds.
        /// </summary>
        public ScaleInAnimation ScaleInAnimation { get => scaleInAnimation; set => scaleInAnimation = value; }

        /// <summary>
        /// The "scale out" animation this animator holds.
        /// </summary>
        public ScaleOutAnimation ScaleOutAnimation { get => scaleOutAnimation; set => scaleOutAnimation = value; }

        /// <summary>
        /// The "scale idle" animation this animator holds.
        /// </summary>
        public ScaleIdleAnimation ScaleIdleAnimation { get => scaleIdleAnimation; set => scaleIdleAnimation = value; }

        /// <summary>
        /// The "fade in" animation this animator holds.
        /// </summary>
        public FadeInAnimation FadeInAnimation { get => fadeInAnimation; set => fadeInAnimation = value; }

        /// <summary>
        /// The "fade out" animation this animator holds.
        /// </summary>
        public FadeOutAnimation FadeOutAnimation { get => fadeOutAnimation; set => fadeOutAnimation = value; }

        /// <summary>
        /// The "fade idle" animation this animator holds.
        /// </summary>
        public FadeIdleAnimation FadeIdleAnimation { get => fadeIdleAnimation; set => fadeIdleAnimation = value; }

        #endregion

        #region MonoBehaviour

        /// <summary>
        /// MonoBehaviour Awake callback.
        /// </summary>
        private void Awake()
        {
            CloneAnimations();

            var childConfig = new AnimatorConfiguration
            {
                startOut = configuration.startOut,
                hideAfterOutAnimation = false,
                idleAfterInAnimation = false,
                interruptionBehaviour = configuration.interruptionBehaviour,
                startAutomatically = false
            };

            var moveAnimations = new MoveAnimationSet(moveInAnimation, moveOutAnimation, moveIdleAnimation);
            moveAnimator = gameObject.AddComponent<MoveAnimator>(x => x.Init(moveAnimations, childConfig, this));

            var rotateAnimations = new RotateAnimationSet(rotateInAnimation, rotateOutAnimation, rotateIdleAnimation);
            rotateAnimator = gameObject.AddComponent<RotateAnimator>(x => x.Init(rotateAnimations, childConfig, this));

            var scaleAnimations = new ScaleAnimationSet(scaleInAnimation, scaleOutAnimation, scaleIdleAnimation);
            scaleAnimator = gameObject.AddComponent<ScaleAnimator>(x => x.Init(scaleAnimations, childConfig, this));

            var fadeAnimations = new FadeAnimationSet(fadeInAnimation, fadeOutAnimation, fadeIdleAnimation);
            fadeAnimator = gameObject.AddComponent<FadeAnimator>(x => x.Init(fadeAnimations, childConfig, this));

            isInitialized = true;
        }

        /// <summary>
        /// MonoBehaviour Start callback.
        /// </summary>
        private void Start()
        {
            if (!configuration.startAutomatically)
            {
                return;
            }

            float initialAnimationDuration = CalculateInitialAnimationDuration();
            float initialAnimationStartTime = Time.time;

            initialAnimationEndTime = initialAnimationStartTime + initialAnimationDuration;

            switch (configuration.initialAnimationType)
            {
                case InitialAnimationType.All:
                    StartAllInAnimations();
                    float delay = configuration.initialAnimationIdleDuration;
                    StartCoroutine(AnimateOutCoroutine(delay));
                    break;
                case InitialAnimationType.In:
                    AnimateIn(AnimationContext.Self);
                    break;
                case InitialAnimationType.Idle:
                    AnimateIdle(AnimationContext.SelfAndChildren);
                    break;
                case InitialAnimationType.Out:
                    ResetOut();
                    StartCoroutine(AnimateOutCoroutine(1f));
                    break;
            }
        }

        #endregion

        #region Public

        /// <summary>
        /// Executes the entrance animation of this animator, if any.
        /// </summary>
        /// <returns>Whether the animation was executed.</returns>
        protected override bool AnimateIn()
        {
            if (!PrepareForAnimation())
            {
                return false;
            }

            bool animated = StartAllInAnimations();

            if (!animated)
            {
                Debug.LogWarning("AnimateIn called in an animator with no associated 'in' animations");
            }
            else
            {
                SetButtonsEnabled(false);
            }

            return animated;
        }

        /// <summary>
        /// Executes the exit animation of this animator, if any.
        /// </summary>
        /// <returns>Whether the animation was executed.</returns>
        protected override bool AnimateOut()
        {
            if (!PrepareForAnimation())
            {
                return false;
            }

            bool animated = StartAllOutAnimations();

            if (!animated)
            {
                Debug.LogWarning("AnimateOut called in an animator with no associated 'out' animations");
            }
            else
            {
                SetButtonsEnabled(false);
            }

            return animated;
        }

        /// <summary>
        /// Executes the idle animation of this animator, if any.
        /// </summary>
        /// <returns>Whether the animation was executed.</returns>
        protected override bool AnimateIdle()
        {
            if (!PrepareForAnimation())
            {
                return false;
            }

            bool animated = StartAllPingPongAnimations();

            if (!animated)
            {
                Debug.LogWarning("AnimatePingPong called in an animator with no associated 'idle' animations");
            }

            return animated;
        }

        /// <summary>
        /// Checks whether this game object is being animated.
        /// </summary>
        /// <returns>A boolean indicating whether this game object is being animated.</returns>
        protected override bool IsAnimating()
        {
            if (!gameObject.activeSelf)
            {
                return false;
            }

            bool isAnimating = false;

            isAnimating |= moveAnimator.IsAnimating(defaultContext);
            isAnimating |= rotateAnimator.IsAnimating(defaultContext);
            isAnimating |= scaleAnimator.IsAnimating(defaultContext);
            isAnimating |= fadeAnimator.IsAnimating(defaultContext);

            return isAnimating;
        }

        /// <summary>
        /// Stops the current animation.
        /// </summary>
        protected override void StopAnimation()
        {
            moveAnimator.StopAnimation(defaultContext);
            rotateAnimator.StopAnimation(defaultContext);
            scaleAnimator.StopAnimation(defaultContext);
            fadeAnimator.StopAnimation(defaultContext);
        }

        /// <summary>
        /// Stops the current idle animation.
        /// </summary>
        protected override void StopPingPongAnimation()
        {
            moveAnimator.StopPingPongAnimation(defaultContext);
            rotateAnimator.StopPingPongAnimation(defaultContext);
            scaleAnimator.StopPingPongAnimation(defaultContext);
            fadeAnimator.StopPingPongAnimation(defaultContext);
        }

        /// <summary>
        /// Resets the exit animation.
        /// </summary>
        private void ResetOut()
        {
            moveAnimator.PrepareForOutAnimation();
            rotateAnimator.PrepareForOutAnimation();
            scaleAnimator.PrepareForOutAnimation();
            fadeAnimator.PrepareForOutAnimation();
        }

        /// <summary>
        /// Informs this animator that the screen orientation of the device has
        /// changed, which means that the canvas bounds must be recalculated.
        /// </summary>
        public void NotifyScreenResolutionChanged()
        {
            moveAnimator.PrepareForInAnimation();
            moveAnimator.PrepareForOutAnimation();
        }

        #endregion

        #region Coroutines

        /// <summary>
        /// Starts the exit animation after the given delay.
        /// </summary>
        /// <param name="delay">The start delay.</param>
        /// <returns>The coroutine IEnumerator.</returns>
        private IEnumerator AnimateOutCoroutine(float delay)
        {
            yield return new WaitForSeconds(delay);

            if (Time.time < initialAnimationEndTime)
            {
                StartAllOutAnimations();
            }
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Calculates the duration in seconds of the initial animation based on the duration
        /// and delay of the animations this animator holds.
        /// </summary>
        /// <returns>The duration in seconds of the initial animation.</returns>
        private float CalculateInitialAnimationDuration()
        {
            switch (configuration.initialAnimationType)
            {
                case InitialAnimationType.All:
                    float totalInDuration = CalculateInitialInAnimationDuration();
                    float totalIdleDuration = configuration.initialAnimationIdleDuration;
                    float totalOutDuration = CalculateInitialOutAnimationDuration();

                    return totalInDuration + totalIdleDuration + totalOutDuration;
                case InitialAnimationType.In:
                    return CalculateInitialInAnimationDuration();
                case InitialAnimationType.Idle:
                    return 0;
                case InitialAnimationType.Out:
                    return CalculateInitialOutAnimationDuration();
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Calculates the duration in seconds of the initial 'in' animation based on the duration
        /// and delay of the 'in' animations this animator holds.
        /// </summary>
        /// <returns>The duration in seconds of the initial 'in' animation.</returns>
        private float CalculateInitialInAnimationDuration()
        {
            float moveInDuration = 0;
            float rotateInDuration = 0;
            float scaleInDuration = 0;
            float fadeInDuration = 0;

            if (moveInAnimation != null)
            {
                moveInDuration += (moveInAnimation.Delay + moveInAnimation.Duration);
            }

            if (rotateInAnimation != null)
            {
                rotateInDuration += (rotateInAnimation.Delay + rotateInAnimation.Duration);
            }

            if (scaleInAnimation != null)
            {
                scaleInDuration += (scaleInAnimation.Delay + scaleInAnimation.Duration);
            }

            if (fadeInAnimation != null)
            {
                fadeInDuration += (fadeInAnimation.Delay + fadeInAnimation.Duration);
            }

            return Mathf.Max(moveInDuration, rotateInDuration, scaleInDuration, fadeInDuration);
        }

        /// <summary>
        /// Calculates the duration in seconds of the initial 'out' animation based on the duration
        /// and delay of the 'out' animations this animator holds.
        /// </summary>
        /// <returns>The duration in seconds of the initial 'out' animation.</returns>
        private float CalculateInitialOutAnimationDuration()
        {
            float moveOutDuration = 0;
            float rotateOutDuration = 0;
            float scaleOutDuration = 0;
            float fadeOutDuration = 0;

            if (moveOutAnimation != null)
            {
                moveOutDuration += (moveOutAnimation.Delay + moveOutAnimation.Duration);
            }

            if (rotateOutAnimation != null)
            {
                rotateOutDuration += (rotateOutAnimation.Delay + rotateOutAnimation.Duration);
            }

            if (scaleOutAnimation != null)
            {
                scaleOutDuration += (scaleOutAnimation.Delay + scaleOutAnimation.Duration);
            }

            if (fadeOutAnimation != null)
            {
                fadeOutDuration += (fadeOutAnimation.Delay + fadeOutAnimation.Duration);
            }

            return Mathf.Max(moveOutDuration, rotateOutDuration, scaleOutDuration, fadeOutDuration); ;
        }

        /// <summary>
        /// Performs preparation actions before running any animation.
        /// </summary>
        /// <returns>A boolean indicating the result of the preparation.</returns>
        private bool PrepareForAnimation()
        {
            if (configuration.startAutomatically && Time.time < initialAnimationEndTime)
            {
                if (configuration.interruptionBehaviour == AnimationInterruptionBehaviour.Interrupt)
                {
                    initialAnimationEndTime = 0;
                    return true;
                }

                return false;
            }

            return true;
        }

        /// <summary>
        /// Starts all the 'in' animations this animator holds.
        /// </summary>
        /// <returns>A boolean indicating whether any animation was actually run.</returns>
        private bool StartAllInAnimations()
        {
            InitializeGameObjectIfNeeded();

            inAnimationsPlayingCount = 0;

            if (moveInAnimation != null && moveAnimator.AnimateIn(defaultContext))
            {
                inAnimationsPlayingCount++;
            }

            if (rotateInAnimation != null && rotateAnimator.AnimateIn(defaultContext))
            {
                inAnimationsPlayingCount++;
            }

            if (scaleInAnimation != null && scaleAnimator.AnimateIn(defaultContext))
            {
                inAnimationsPlayingCount++;
            }

            if (fadeInAnimation != null && fadeAnimator.AnimateIn(defaultContext))
            {
                inAnimationsPlayingCount++;
            }

            return inAnimationsPlayingCount > 0;
        }

        /// <summary>
        /// Starts all the 'out' animations this animator holds.
        /// </summary>
        /// <returns>A boolean indicating whether any animation was actually run.</returns>
        private bool StartAllOutAnimations()
        {
            InitializeGameObjectIfNeeded();

            outAnimationsPlayingCount = 0;

            if (moveOutAnimation != null && moveAnimator.AnimateOut(defaultContext))
            {
                outAnimationsPlayingCount++;
            }

            if (rotateOutAnimation != null && rotateAnimator.AnimateOut(defaultContext))
            {
                outAnimationsPlayingCount++;
            }

            if (scaleOutAnimation != null && scaleAnimator.AnimateOut(defaultContext))
            {
                outAnimationsPlayingCount++;
            }

            if (fadeOutAnimation != null && fadeAnimator.AnimateOut(defaultContext))
            {
                outAnimationsPlayingCount++;
            }

            return outAnimationsPlayingCount > 0;
        }

        /// <summary>
        /// Starts all the 'idle' animations this animator holds.
        /// </summary>
        /// <returns>A boolean indicating whether any animation was actually run.</returns>
        private bool StartAllPingPongAnimations()
        {
            InitializeGameObjectIfNeeded();

            bool animated = false;

            if (moveIdleAnimation != null)
            {
                animated |= moveAnimator.AnimateIdle(defaultContext);
            }

            if (rotateIdleAnimation != null)
            {
                animated |= rotateAnimator.AnimateIdle(defaultContext);
            }

            if (scaleIdleAnimation != null)
            {
                animated |= scaleAnimator.AnimateIdle(defaultContext);
            }

            if (fadeIdleAnimation != null)
            {
                animated |= fadeAnimator.AnimateIdle(defaultContext);
            }

            return animated;
        }

        /// <summary>
        /// Forces this game object to be initialized.
        /// </summary>
        private void InitializeGameObjectIfNeeded()
        {
            if (isInitialized)
            {
                return;
            }

            List<GameObject> activatedObjects = new List<GameObject>();

            var currentTransform = transform;

            while (currentTransform != null)
            {
                if (!currentTransform.gameObject.activeInHierarchy)
                {
                    currentTransform.gameObject.SetActive(true);
                    activatedObjects.Add(currentTransform.gameObject);
                }

                currentTransform = currentTransform.parent;
            }

            foreach (var gameObject in activatedObjects)
            {
                gameObject.SetActive(false);
            }
        }

        private void SetButtonsEnabled(bool interactable)
        {
            foreach (var button in configuration.buttonsToDeactivateDuringAnimation)
            {
                button.enabled = interactable;
            }
        }

        /// <summary>
        /// Checks whether any entrance animation was provided to this animator.
        /// </summary>
        /// <returns>A boolean indicating whether any entrance animation was
        /// provided to this animator.</returns>
        private bool HasAnyInAnimation()
        {
            return moveInAnimation != null || rotateInAnimation != null
                || scaleInAnimation != null || fadeInAnimation != null;
        }

        /// <summary>
        /// Checks whether any exit animation was provided to this animator.
        /// </summary>
        /// <returns>A boolean indicating whether any exit animation was
        /// provided to this animator.</returns>
        private bool HasAnyOutAnimation()
        {
            return moveOutAnimation != null || rotateOutAnimation != null
                || scaleOutAnimation != null || fadeOutAnimation != null;
        }

        /// <summary>
        /// Checks whether any idle animation was provided to this animator.
        /// </summary>
        /// <returns>A boolean indicating whether any idle animation was
        /// provided to this animator.</returns>
        private bool HasAnyPingPongAnimation()
        {
            return moveIdleAnimation != null || rotateIdleAnimation != null
                || scaleIdleAnimation != null || fadeIdleAnimation != null;
        }

        /// <summary>
        /// Gets all the animators from the children of this game object
        /// with the same type as this animator.
        /// </summary>
        /// <returns>The animators from the children of this game object
        /// with the same type as this animator.</returns>
        protected override AnimatorBehaviour[] GetChildAnimators()
        {
            AnimatorBehaviour[] childrenWithParent = gameObject.transform.GetComponentsInChildren<KAnimator>();
            AnimatorBehaviour[] children = new AnimatorBehaviour[childrenWithParent.Length - 1];
            int i = 0;

            foreach (AnimatorBehaviour child in childrenWithParent)
            {
                if (child.gameObject.GetInstanceID() != gameObject.GetInstanceID())
                {
                    children[i] = child;
                    i++;
                }
            }

            return children;
        }

        /// <summary>
        /// Clones all the animations provided to this animator, so the original
        /// scriptable objects don't get changed.
        /// </summary>
        private void CloneAnimations()
        {
            if (moveInAnimation != null)
            {
                moveInAnimation = Instantiate(moveInAnimation) as MoveInAnimation;
            }

            if (moveOutAnimation != null)
            {
                moveOutAnimation = Instantiate(moveOutAnimation) as MoveOutAnimation;
            }

            if (moveIdleAnimation != null)
            {
                moveIdleAnimation = Instantiate(moveIdleAnimation) as MoveIdleAnimation;
            }

            if (rotateInAnimation != null)
            {
                rotateInAnimation = Instantiate(rotateInAnimation) as RotateInAnimation;
            }

            if (rotateOutAnimation != null)
            {
                rotateOutAnimation = Instantiate(rotateOutAnimation) as RotateOutAnimation;
            }

            if (rotateIdleAnimation != null)
            {
                rotateIdleAnimation = Instantiate(rotateIdleAnimation) as RotateIdleAnimation;
            }

            if (scaleInAnimation != null)
            {
                scaleInAnimation = Instantiate(scaleInAnimation) as ScaleInAnimation;
            }

            if (scaleOutAnimation != null)
            {
                scaleOutAnimation = Instantiate(scaleOutAnimation) as ScaleOutAnimation;
            }

            if (scaleIdleAnimation != null)
            {
                scaleIdleAnimation = Instantiate(scaleIdleAnimation) as ScaleIdleAnimation;
            }

            if (fadeInAnimation != null)
            {
                fadeInAnimation = Instantiate(fadeInAnimation) as FadeInAnimation;
            }

            if (fadeOutAnimation != null)
            {
                fadeOutAnimation = Instantiate(fadeOutAnimation) as FadeOutAnimation;
            }

            if (fadeIdleAnimation != null)
            {
                fadeIdleAnimation = Instantiate(fadeIdleAnimation) as FadeIdleAnimation;
            }
        }

        #endregion

        #region IAnimationObserver

        /// <summary>
        /// Callback invoked when an entrance animation has just started.
        /// </summary>
        /// <param name="animation">The animation.</param>
        public void InAnimationStarted(Animation.Base.Animation animation)
        {
            Debug.Log(gameObject.name + " - " + animation.GetType().Name + " started");
        }

        /// <summary>
        /// Callback invoked when an entrance animation has just finished.
        /// </summary>
        /// <param name="animation">The animation.</param>
        public void InAnimationFinished(Animation.Base.Animation animation)
        {
            Debug.Log(gameObject.name + " - " + animation.GetType().Name + " finished");

            inAnimationsPlayingCount--;

            if (inAnimationsPlayingCount == 0)
            {
                SetButtonsEnabled(true);

                if (configuration.idleAfterInAnimation)
                {
                    StartAllPingPongAnimations();
                }
            }
        }

        /// <summary>
        /// Callback invoked when an entrance animation is stopped before finishing.
        /// </summary>
        /// <param name="animation">The animation.</param>
        public void InAnimationStopped(Animation.Base.Animation animation)
        {
            inAnimationsPlayingCount--;

            Debug.Log(gameObject.name + " - " + animation.GetType().Name + " stopped");
        }

        /// <summary>
        /// Callback invoked when an exit animation has just started.
        /// </summary>
        /// <param name="animation">The animation.</param>
        public void OutAnimationStarted(Animation.Base.Animation animation)
        {
            Debug.Log(gameObject.name + " - " + animation.GetType().Name + " started");
        }

        /// <summary>
        /// Callback invoked when an exit animation has just finished.
        /// </summary>
        /// <param name="animation">The animation.</param>
        public void OutAnimationFinished(Animation.Base.Animation animation)
        {
            Debug.Log(gameObject.name + " - " + animation.GetType().Name + " finished");

            outAnimationsPlayingCount--;

            if (outAnimationsPlayingCount == 0)
            {
                SetButtonsEnabled(true);

                if (configuration.hideAfterOutAnimation)
                {
                    gameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// Callback invoked when an exit animation is stopped before finishing.
        /// </summary>
        /// <param name="animation">The animation.</param>
        public void OutAnimationStopped(Animation.Base.Animation animation)
        {
            Debug.Log(gameObject.name + " - " + animation.GetType().Name + " stopped");
        }

        /// <summary>
        /// Callback invoked when a "idle" animation has just started.
        /// </summary>
        /// <param name="animation">The animation.</param>
        public void PingPongAnimationStarted(IdleAnimation animation)
        {
            Debug.Log(gameObject.name + " - " + animation.GetType().Name + " started");
        }

        /// <summary>
        /// Callback invoked when an "idle" animation has just finished.
        /// </summary>
        /// <param name="animation">The animation.</param>
        public void PingPongAnimationFinished(IdleAnimation animation)
        {
            Debug.Log(gameObject.name + " - " + animation.GetType().Name + " finished");
        }

        /// <summary>
        /// Callback invoked when a "idle" animation is stopped before finishing.
        /// </summary>
        /// <param name="animation">The animation.</param>
        public void PingPongAnimationStopped(IdleAnimation animation)
        {
            Debug.Log(gameObject.name + " - " + animation.GetType().Name + " stopped");
        }

        #endregion
    }
}