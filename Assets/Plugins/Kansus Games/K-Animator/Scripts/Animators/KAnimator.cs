using KansusAnimator.Animations;
using System;
using System.Collections;
using UnityEngine;

namespace KansusAnimator.Animators
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

        private AudioManager audioManager;
        
        private float initialAnimationEndTime = 0;

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
        private MovePingPongAnimation movePingPongAnimation;

        [SerializeField]
        private RotateInAnimation rotateInAnimation;

        [SerializeField]
        private RotateOutAnimation rotateOutAnimation;

        [SerializeField]
        private RotatePingPongAnimation rotatePingPongAnimation;

        [SerializeField]
        private FadeInAnimation fadeInAnimation;

        [SerializeField]
        private FadeOutAnimation fadeOutAnimation;

        [SerializeField]
        private FadePingPongAnimation fadePingPongAnimation;

        [SerializeField]
        private ScaleInAnimation scaleInAnimation;

        [SerializeField]
        private ScaleOutAnimation scaleOutAnimation;

        [SerializeField]
        private ScalePingPongAnimation scalePingPongAnimation;

        #endregion

        #region Properties - Move Animation

        /// <summary>
        /// The "move in" animation this animator holds.
        /// </summary>
        public MoveInAnimation MoveInAnimation
        {
            get { return moveInAnimation; }
            set { moveInAnimation = value; }
        }

        /// <summary>
        /// The "move out" animation this animator holds.
        /// </summary>
        public MoveOutAnimation MoveOutAnimation
        {
            get { return moveOutAnimation; }
            set { moveOutAnimation = value; }
        }

        /// <summary>
        /// The "move ping-pong" animation this animator holds.
        /// </summary>
        public MovePingPongAnimation MovePingPongAnimation
        {
            get { return movePingPongAnimation; }
            set { movePingPongAnimation = value; }
        }

        #endregion

        #region Properties - Rotation Animation

        /// <summary>
        /// The "rotate in" animation this animator holds.
        /// </summary>
        public RotateInAnimation RotateInAnimation
        {
            get { return rotateInAnimation; }
            set { rotateInAnimation = value; }
        }

        /// <summary>
        /// The "rotate out" animation this animator holds.
        /// </summary>
        public RotateOutAnimation RotateOutAnimation
        {
            get { return rotateOutAnimation; }
            set { rotateOutAnimation = value; }
        }

        /// <summary>
        /// The "rotate ping-pong" animation this animator holds.
        /// </summary>
        public RotatePingPongAnimation RotatePingPongAnimation
        {
            get { return rotatePingPongAnimation; }
            set { rotatePingPongAnimation = value; }
        }

        #endregion

        #region Properties - Scale Animation

        /// <summary>
        /// The "scale in" animation this animator holds.
        /// </summary>
        public ScaleInAnimation ScaleInAnimation
        {
            get { return scaleInAnimation; }
            set { scaleInAnimation = value; }
        }

        /// <summary>
        /// The "scale out" animation this animator holds.
        /// </summary>
        public ScaleOutAnimation ScaleOutAnimation
        {
            get { return scaleOutAnimation; }
            set { scaleOutAnimation = value; }
        }

        /// <summary>
        /// The "scale ping-pong" animation this animator holds.
        /// </summary>
        public ScalePingPongAnimation ScalePingPongAnimation
        {
            get { return scalePingPongAnimation; }
            set { scalePingPongAnimation = value; }
        }

        #endregion

        #region Properties - Fade Animation

        /// <summary>
        /// The "fade in" animation this animator holds.
        /// </summary>
        public FadeInAnimation FadeInAnimation
        {
            get { return fadeInAnimation; }
            set { fadeInAnimation = value; }
        }

        /// <summary>
        /// The "fade out" animation this animator holds.
        /// </summary>
        public FadeOutAnimation FadeOutAnimation
        {
            get { return fadeOutAnimation; }
            set { fadeOutAnimation = value; }
        }

        /// <summary>
        /// The "fade ping-pong" animation this animator holds.
        /// </summary>
        public FadePingPongAnimation FadePingPongAnimation
        {
            get { return fadePingPongAnimation; }
            set { fadePingPongAnimation = value; }
        }

        #endregion

        #region MonoBehaviour

        /// <summary>
        /// MonoBehaviour Awake callback.
        /// </summary>
        private void Awake()
        {
            if (gameObject != null)
            {
                CloneAnimations();

                MoveAnimationSet moveAnimations = new MoveAnimationSet(
                    moveInAnimation,
                    moveOutAnimation,
                    movePingPongAnimation
                );
                moveAnimator = gameObject.AddComponent<MoveAnimator>(x => x.Init(moveAnimations, this));

                RotateAnimationSet rotateAnimations = new RotateAnimationSet(
                    rotateInAnimation,
                    rotateOutAnimation,
                    rotatePingPongAnimation
                );
                rotateAnimator = gameObject.AddComponent<RotateAnimator>(x => x.Init(rotateAnimations, this));

                ScaleAnimationSet scaleAnimations = new ScaleAnimationSet(
                    scaleInAnimation,
                    scaleOutAnimation,
                    scalePingPongAnimation
                );
                scaleAnimator = gameObject.AddComponent<ScaleAnimator>(x => x.Init(scaleAnimations, this));

                FadeAnimationSet fadeAnimations = new FadeAnimationSet(
                    fadeInAnimation,
                    fadeOutAnimation,
                    fadePingPongAnimation
                );
                fadeAnimator = gameObject.AddComponent<FadeAnimator>(x => x.Init(fadeAnimations, this));
            }
        }

        /// <summary>
        /// MonoBehaviour Start callback.
        /// </summary>
        private void Start()
        {
            if (AnimationManager.Instance.Configuration.StartAutomatically)
            {
                float initialAnimationDuration = CalculateInitialAnimationDuration();
                float initialAnimationStartTime = Time.time;

                initialAnimationEndTime = initialAnimationStartTime + initialAnimationDuration;

                switch (AnimationManager.Instance.Configuration.InitialAnimationType)
                {
                    case InitialAnimationType.All:
                        StartAllInAnimations();
                        float delay = AnimationManager.Instance.Configuration.InitialAnimationIdleDuration;
                        StartCoroutine(AnimateOutCoroutine(delay));
                        break;
                    case InitialAnimationType.In:
                        StartAllInAnimations();
                        break;
                    case InitialAnimationType.Idle:
                        StartAllPingPongAnimations();
                        break;
                    case InitialAnimationType.Out:
                        ResetOut();
                        StartCoroutine(AnimateOutCoroutine(1f));
                        break;
                }
            }
        }

        #endregion

        #region Public
        
        /// <summary>
        /// Executes the "in" animation of this animator, if any.
        /// </summary>
        /// <returns>Whether the animation was executed.</returns>
        public override bool AnimateIn()
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

            return animated;
        }

        /// <summary>
        /// Executes the "out" animation of this animator, if any.
        /// </summary>
        /// <returns>Whether the animation was executed.</returns>
        public override bool AnimateOut()
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

            return animated;
        }

        /// <summary>
        /// Executes the "ping pong" animation of this animator, if any.
        /// </summary>
        /// <returns>Whether the animation was executed.</returns>
        public override bool AnimatePingPong()
        {
            if (!PrepareForAnimation())
            {
                return false;
            }

            bool animated = StartAllPingPongAnimations();

            if (!animated)
            {
                Debug.LogWarning("AnimatePingPong called in an animator with no associated 'ping pong' animations");
            }

            return animated;
        }

        /// <summary>
        /// Checks whether this game object is being animated.
        /// </summary>
        /// <returns>A boolean indicating whether this game object is being animated.</returns>
        public override bool IsAnimating()
        {
            if (this == null || gameObject == null || !gameObject.activeSelf || !enabled)
            {
                return false;
            }

            bool isAnimating = false;

            isAnimating |= moveInAnimation != null && moveInAnimation.HasBegan;
            isAnimating |= moveOutAnimation != null && moveOutAnimation.HasBegan;
            isAnimating |= rotateInAnimation != null && rotateInAnimation.HasBegan;
            isAnimating |= rotateOutAnimation != null && rotateOutAnimation.HasBegan;
            isAnimating |= scaleInAnimation != null && scaleInAnimation.HasBegan;
            isAnimating |= scaleOutAnimation != null && scaleOutAnimation.HasBegan;
            isAnimating |= fadeInAnimation != null && fadeInAnimation.HasBegan;
            isAnimating |= fadeOutAnimation != null && fadeOutAnimation.HasBegan;

            return isAnimating;
        }

        /// <summary>
        /// Stops the current animation.
        /// </summary>
        public override void StopAnimation()
        {
            moveAnimator.StopAnimation();
            rotateAnimator.StopAnimation();
            scaleAnimator.StopAnimation();
            fadeAnimator.StopAnimation();
        }

        /// <summary>
        /// Stops the current "ping pong" animation.
        /// </summary>
        public override void StopPingPongAnimation()
        {
            moveAnimator.StopPingPongAnimation();
            rotateAnimator.StopPingPongAnimation();
            scaleAnimator.StopPingPongAnimation();
            fadeAnimator.StopPingPongAnimation();
        }

        /// <summary>
        /// Resets the "out" animation.
        /// </summary>
        private void ResetOut()
        {
            moveAnimator.ResetOut();
            rotateAnimator.ResetOut();
            scaleAnimator.ResetOut();
            fadeAnimator.ResetOut();
        }

        /// <summary>
        /// Informs this animator that the screen orientation of the device has
        /// changed, which means that the canvas bounds must be recalculated.
        /// </summary>
        public void NotifyScreenResolutionChanged()
        {
            moveAnimator.ResetIn();
            moveAnimator.ResetOut();
        }

        #endregion

        #region Coroutines

        /// <summary>
        /// Starts the "out" animation after the given delay.
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
            switch (AnimationManager.Instance.Configuration.InitialAnimationType)
            {
                case InitialAnimationType.All:
                    float totalInDuration = CalculateInitialInAnimationDuration();
                    float totalIdleDuration = AnimationManager.Instance.Configuration.InitialAnimationIdleDuration;
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
            var interruptionBehaviour = AnimationManager.Instance.Configuration.InterruptionBehavior;
            
            if (AnimationManager.Instance.Configuration.StartAutomatically
                && Time.time < initialAnimationEndTime)
            {
                if (interruptionBehaviour == AnimationInterruptionBehaviour.Interrupt)
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
            bool animated = false;

            if (gameObject.activeSelf && enabled)
            {
                if (moveInAnimation != null)
                {
                    animated |= moveAnimator.AnimateIn();
                }

                if (rotateInAnimation != null)
                {
                    animated |= rotateAnimator.AnimateIn();
                }

                if (scaleInAnimation != null)
                {
                    animated |= scaleAnimator.AnimateIn();
                }

                if (fadeInAnimation != null)
                {
                    animated |= fadeAnimator.AnimateIn();
                }
            }

            return animated;
        }

        /// <summary>
        /// Starts all the 'out' animations this animator holds.
        /// </summary>
        /// <returns>A boolean indicating whether any animation was actually run.</returns>
        private bool StartAllOutAnimations()
        {
            bool animated = false;

            if (gameObject.activeSelf && enabled)
            {
                if (moveOutAnimation != null)
                {
                    animated |= moveAnimator.AnimateOut();
                }

                if (rotateOutAnimation != null)
                {
                    animated |= rotateAnimator.AnimateOut();
                }

                if (scaleOutAnimation != null)
                {
                    animated |= scaleAnimator.AnimateOut();
                }

                if (fadeOutAnimation != null)
                {
                    animated |= fadeAnimator.AnimateOut();
                }
            }

            return animated;
        }

        /// <summary>
        /// Starts all the 'ping-pong' animations this animator holds.
        /// </summary>
        /// <returns>A boolean indicating whether any animation was actually run.</returns>
        private bool StartAllPingPongAnimations()
        {
            bool animated = false;

            if (gameObject.activeSelf && enabled)
            {
                if (movePingPongAnimation != null)
                {
                    animated |= moveAnimator.AnimatePingPong();
                }

                if (rotatePingPongAnimation != null)
                {
                    animated |= rotateAnimator.AnimatePingPong();
                }

                if (scalePingPongAnimation != null)
                {
                    animated |= scaleAnimator.AnimatePingPong();
                }

                if (fadePingPongAnimation != null)
                {
                    animated |= fadeAnimator.AnimatePingPong();
                }
            }

            return animated;
        }

        /// <summary>
        /// Checks whether any "in" animation was provided to this animator.
        /// </summary>
        /// <returns>A boolean indicating whether any "in" animation was
        /// provided to this animator.</returns>
        private bool HasAnyInAnimation()
        {
            return moveInAnimation != null || rotateInAnimation != null
                || scaleInAnimation != null || fadeInAnimation != null;
        }

        /// <summary>
        /// Checks whether any "out" animation was provided to this animator.
        /// </summary>
        /// <returns>A boolean indicating whether any "out" animation was
        /// provided to this animator.</returns>
        private bool HasAnyOutAnimation()
        {
            return moveOutAnimation != null || rotateOutAnimation != null
                || scaleOutAnimation != null || fadeOutAnimation != null;
        }

        /// <summary>
        /// Checks whether any "ping pong" animation was provided to this animator.
        /// </summary>
        /// <returns>A boolean indicating whether any "ping pong" animation was
        /// provided to this animator.</returns>
        private bool HasAnyPingPongAnimation()
        {
            return movePingPongAnimation != null || rotatePingPongAnimation != null
                || scalePingPongAnimation != null || fadePingPongAnimation != null;
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

            if (movePingPongAnimation != null)
            {
                movePingPongAnimation = Instantiate(movePingPongAnimation) as MovePingPongAnimation;
            }

            if (rotateInAnimation != null)
            {
                rotateInAnimation = Instantiate(rotateInAnimation) as RotateInAnimation;
            }

            if (rotateOutAnimation != null)
            {
                rotateOutAnimation = Instantiate(rotateOutAnimation) as RotateOutAnimation;
            }

            if (rotatePingPongAnimation != null)
            {
                rotatePingPongAnimation = Instantiate(rotatePingPongAnimation) as RotatePingPongAnimation;
            }

            if (scaleInAnimation != null)
            {
                scaleInAnimation = Instantiate(scaleInAnimation) as ScaleInAnimation;
            }

            if (scaleOutAnimation != null)
            {
                scaleOutAnimation = Instantiate(scaleOutAnimation) as ScaleOutAnimation;
            }

            if (scalePingPongAnimation != null)
            {
                scalePingPongAnimation = Instantiate(scalePingPongAnimation) as ScalePingPongAnimation;
            }

            if (fadeInAnimation != null)
            {
                fadeInAnimation = Instantiate(fadeInAnimation) as FadeInAnimation;
            }

            if (fadeOutAnimation != null)
            {
                fadeOutAnimation = Instantiate(fadeOutAnimation) as FadeOutAnimation;
            }

            if (fadePingPongAnimation != null)
            {
                fadePingPongAnimation = Instantiate(fadePingPongAnimation) as FadePingPongAnimation;
            }
        }

        #endregion

        #region IAnimationObserver

        /// <summary>
        /// Callback invoked when an "in" animation has just started.
        /// </summary>
        /// <param name="animation">The animation.</param>
        public void InAnimationStarted(Animations.Animation animation)
        {
            Debug.Log(gameObject.name + " - " + animation.GetType().Name + " started");
        }

        /// <summary>
        /// Callback invoked when an "in" animation has just finished.
        /// </summary>
        /// <param name="animation">The animation.</param>
        public void InAnimationFinished(Animations.Animation animation)
        {
            Debug.Log(gameObject.name + " - " + animation.GetType().Name + " finished");

            if (AnimationManager.Instance.Configuration.PingPongAfterInAnimation)
            {
                StartAllPingPongAnimations();
            }
        }

        /// <summary>
        /// Callback invoked when an "in" animation is stopped before finishing.
        /// </summary>
        /// <param name="animation">The animation.</param>
        public void InAnimationStopped(Animations.Animation animation)
        {
            Debug.Log(gameObject.name + " - " + animation.GetType().Name + " stopped");
        }

        /// <summary>
        /// Callback invoked when an "out" animation has just started.
        /// </summary>
        /// <param name="animation">The animation.</param>
        public void OutAnimationStarted(Animations.Animation animation)
        {
            Debug.Log(gameObject.name + " - " + animation.GetType().Name + " started");
        }

        /// <summary>
        /// Callback invoked when an "out" animation has just finished.
        /// </summary>
        /// <param name="animation">The animation.</param>
        public void OutAnimationFinished(Animations.Animation animation)
        {
            Debug.Log(gameObject.name + " - " + animation.GetType().Name + " finished");
        }

        /// <summary>
        /// Callback invoked when an "out" animation is stopped before finishing.
        /// </summary>
        /// <param name="animation">The animation.</param>
        public void OutAnimationStopped(Animations.Animation animation)
        {
            Debug.Log(gameObject.name + " - " + animation.GetType().Name + " stopped");
        }

        /// <summary>
        /// Callback invoked when a "ping-pong" animation has just started.
        /// </summary>
        /// <param name="animation">The animation.</param>
        public void PingPongAnimationStarted(PingPongAnimation animation)
        {
            Debug.Log(gameObject.name + " - " + animation.GetType().Name + " started");
        }

        /// <summary>
        /// Callback invoked when an "ping-pong" animation has just finished.
        /// </summary>
        /// <param name="animation">The animation.</param>
        public void PingPongAnimationFinished(PingPongAnimation animation)
        {
            Debug.Log(gameObject.name + " - " + animation.GetType().Name + " finished");
        }

        /// <summary>
        /// Callback invoked when a "ping-pong" animation is stopped before finishing.
        /// </summary>
        /// <param name="animation">The animation.</param>
        public void PingPongAnimationStopped(PingPongAnimation animation)
        {
            Debug.Log(gameObject.name + " - " + animation.GetType().Name + " stopped");
        }

        #endregion
    }
}