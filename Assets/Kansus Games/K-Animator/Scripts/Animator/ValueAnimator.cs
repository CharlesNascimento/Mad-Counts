using KansusGames.KansusAnimator.Animation;
using KansusGames.KansusAnimator.Animation.Base;
using KansusGames.KansusAnimator.Core;
using KansusGames.KansusAnimator.Tweening;
using UnityEngine;

namespace KansusGames.KansusAnimator.Animator
{
    /// <summary>
    /// Component that animates a specific value of its game object.
    /// </summary>
    /// <typeparam name="TInAnimation">The model of the entrance animation.</typeparam>
    /// <typeparam name="TOutAnimation">The model of the exit animation.</typeparam>
    /// <typeparam name="TIdleAnimation">The model of the idle animation.</typeparam>
    public abstract class ValueAnimator<TInAnimation, TOutAnimation, TIdleAnimation> : AnimatorBehaviour
    where TInAnimation : Animation.Base.Animation
    where TOutAnimation : Animation.Base.Animation
    where TIdleAnimation : IdleAnimation
    {
        #region Fields

        protected ITweener tweener;
        protected IAnimationObserver animationObserver;
        protected bool initializedFromInit = false;

        #endregion

        #region Fields - Animation

        protected Tween tween;

        [SerializeField]
        protected TInAnimation inAnimation;

        [SerializeField]
        protected TOutAnimation outAnimation;

        [SerializeField]
        protected TIdleAnimation idleAnimation;

        protected bool isInAnimationDone = false;
        protected bool isOutAnimationDone = false;
        protected bool isIdleAnimationDone = false;

        protected bool isAnimatingIn = false;
        protected bool isAnimatingOut = false;
        protected bool isAnimatingIdle = false;

        protected bool hasInAnimationBegan = false;
        protected bool hasOutAnimationBegan = false;
        protected bool hasIdleAnimationBegan = false;

        #endregion

        #region Properties

        /// <summary>
        /// The entrance animation.
        /// </summary>
        public TInAnimation InAnimation
        {
            get { return inAnimation; }
        }

        /// <summary>
        /// The exit animation.
        /// </summary>
        public TOutAnimation OutAnimation
        {
            get { return outAnimation; }
        }

        /// <summary>
        /// The idle animation.
        /// </summary>
        public TIdleAnimation IdleAnimation
        {
            get { return idleAnimation; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes this component.
        /// </summary>
        /// <param name="animationSet">A set of the three animations (in, out and idle)
        /// to assign to this animator.</param>
        /// <param name="animationObserver">An observer that will be notified about animation
        /// events from this animator.</param>
        public void Init(AnimationSet<TInAnimation, TOutAnimation, TIdleAnimation> animationSet,
            AnimatorConfiguration configuration, IAnimationObserver animationObserver)
        {
            initializedFromInit = true;

            inAnimation = animationSet.InAnimation;
            outAnimation = animationSet.OutAnimation;
            idleAnimation = animationSet.PingPongAnimation;

            this.animationObserver = animationObserver;
            this.configuration = configuration;
        }

        #endregion

        #region MonoBehaviour

        /// <summary>
        /// MonoBehaviour Awake callback.
        /// </summary>
        protected virtual void Awake()
        {
            tweener = KAnimatorConfiguration.Instance.Tweener;

            if (!initializedFromInit)
            {
                if (inAnimation != null)
                {
                    inAnimation = Instantiate(inAnimation) as TInAnimation;
                }

                if (outAnimation != null)
                {
                    outAnimation = Instantiate(outAnimation) as TOutAnimation;
                }

                if (idleAnimation != null)
                {
                    idleAnimation = Instantiate(idleAnimation) as TIdleAnimation;
                }
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
            if (inAnimation == null)
            {
                Debug.LogWarning("AnimateIn called in an animator with no associated 'in' animation");

                if (idleAnimation != null && !hasIdleAnimationBegan && configuration.idleAfterInAnimation)
                {
                    AnimateIdle();
                }

                return false;
            }

            if (!hasInAnimationBegan)
            {
                StopPingPongAnimation();

                hasInAnimationBegan = true;
                isAnimatingIn = false;
                isInAnimationDone = false;

                var duration = inAnimation.Duration / KAnimatorConfiguration.Instance.AnimationSpeed;

                var valueTo = new ValueToAnimation(gameObject, AnimateInUpdate, 0f, 1f, duration)
                    .SetDelay(inAnimation.Delay / KAnimatorConfiguration.Instance.AnimationSpeed)
                    .SetEaseType(inAnimation.EaseType)
                    .SetOnStart(InAnimationWillBegin)
                    .SetOnComplete(AnimateInComplete);

                tween = tweener.StartValueTo(valueTo);

                animationObserver?.InAnimationStarted(inAnimation);

                if (inAnimation.Sounds.Begin != null)
                {
                    AudioManager.Instance.PlaySound(inAnimation.Sounds.Begin);
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Executes the exit animation of this animator, if any.
        /// </summary>
        /// <returns>Whether the animation was executed.</returns>
        protected override bool AnimateOut()
        {
            if (outAnimation == null)
            {
                Debug.LogWarning("AnimateOut called in an animator with no associated 'out' animation");

                if (idleAnimation != null && hasIdleAnimationBegan)
                {
                    StopPingPongAnimation();
                }

                return false;
            }

            if (!hasOutAnimationBegan)
            {
                StopPingPongAnimation();

                hasOutAnimationBegan = true;
                isAnimatingOut = false;
                isOutAnimationDone = false;

                var duration = outAnimation.Duration / KAnimatorConfiguration.Instance.AnimationSpeed;

                var valueTo = new ValueToAnimation(gameObject, AnimateOutUpdate, 0f, 1f, duration)
                    .SetDelay(outAnimation.Delay / KAnimatorConfiguration.Instance.AnimationSpeed)
                    .SetEaseType(outAnimation.EaseType)
                    .SetOnStart(OutAnimationWillBegin)
                    .SetOnComplete(AnimateOutComplete);

                tween = tweener.StartValueTo(valueTo);

                animationObserver?.OutAnimationStarted(outAnimation);

                if (outAnimation.Sounds.Begin != null)
                {
                    AudioManager.Instance.PlaySound(outAnimation.Sounds.Begin);
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Executes the idle animation of this animator, if any.
        /// </summary>
        /// <returns>Whether the animation was executed.</returns>
        protected override bool AnimateIdle()
        {
            if (idleAnimation == null)
            {
                Debug.LogWarning("AnimatePingPong called in an animator with no associated 'idle' animation");
                return false;
            }

            if (!gameObject.activeSelf)
            {
                return false;
            }

            StopPingPongAnimation();

            hasIdleAnimationBegan = true;
            isAnimatingIdle = false;
            isIdleAnimationDone = false;

            var duration = idleAnimation.Duration / KAnimatorConfiguration.Instance.AnimationSpeed;

            var valueTo = new ValueToAnimation(gameObject, AnimateIdleUpdate, 0f, 1f, duration)
                .SetDelay(idleAnimation.Delay / KAnimatorConfiguration.Instance.AnimationSpeed)
                .SetEaseType(idleAnimation.EaseType)
                .SetPingPongLoop(true)
                .SetOnStart(IdleAnimationWillBegin)
                .SetOnComplete(AnimatePingPongComplete);

            tween = tweener.StartValueTo(valueTo);

            animationObserver?.PingPongAnimationStarted(idleAnimation);

            return true;
        }

        /// <summary>
        /// Checks whether this animator is performing any animation.
        /// </summary>
        /// <returns>A boolean indicating if any animation is in progress.</returns>
        protected override bool IsAnimating()
        {
            bool isAnimating = inAnimation != null && hasInAnimationBegan;
            isAnimating |= outAnimation != null && hasOutAnimationBegan;

            return isAnimating;
        }

        /// <summary>
        /// Checks whether any animator of this game object is performing any animation.
        /// </summary>
        /// <returns>A boolean indicating if any animation is in progress.</returns>
        protected bool IsGameObjectAnimating()
        {
            bool isAnimating = false;

            AnimatorBehaviour[] components = gameObject.GetComponents<AnimatorBehaviour>();

            foreach (AnimatorBehaviour component in components)
            {
                if (component != null && component.enabled)
                {
                    isAnimating |= component.IsAnimating(AnimationContext.Self);
                }
            }

            return isAnimating;
        }

        /// <summary>
        /// Stops the current animation.
        /// </summary>
        protected override void StopAnimation()
        {
            if (tween != null)
            {
                tweener.Stop(tween);
                tween = null;
            }

            if (inAnimation != null)
            {
                if (hasInAnimationBegan)
                {
                    animationObserver?.InAnimationStopped(inAnimation);
                }

                isAnimatingIn = false;
                hasInAnimationBegan = false;
                isInAnimationDone = false;
            }

            if (outAnimation != null)
            {
                if (hasOutAnimationBegan)
                {
                    animationObserver?.OutAnimationStopped(outAnimation);
                }

                isAnimatingOut = false;
                hasOutAnimationBegan = false;
                isOutAnimationDone = false;
            }
        }

        /// <summary>
        /// Stops all animators of this game object.
        /// </summary>
        public void StopGameObjectAnimations()
        {
            AnimatorBehaviour[] animators = gameObject.GetComponents<AnimatorBehaviour>();

            foreach (AnimatorBehaviour animator in animators)
            {
                if (animator != null && animator.enabled)
                {
                    animator.StopAnimation(AnimationContext.Self);
                }
            }
        }

        /// <summary>
        /// Stops the current idle animation.
        /// </summary>
        protected override void StopPingPongAnimation()
        {
            if (idleAnimation == null || !hasIdleAnimationBegan)
            {
                return;
            }

            if (tween != null)
            {
                tweener.Stop(tween);
                tween = null;
            }

            AnimateIdleUpdate(1);

            isAnimatingIdle = false;
            hasIdleAnimationBegan = false;
            isIdleAnimationDone = true;

            StopPingPongSound();

            animationObserver?.PingPongAnimationStopped(idleAnimation);
        }

        /// <summary>
        /// Resets the state of the entrance animation.
        /// </summary>
        public abstract void PrepareForInAnimation();

        /// <summary>
        /// Resets the state of the exit animation.
        /// </summary>
        public abstract void PrepareForOutAnimation();

        #endregion

        #region Callbacks

        /// <summary>
        /// Callback invoked when an entrance animation is about to start.
        /// </summary>
        protected virtual void InAnimationWillBegin()
        {
            gameObject.SetActive(true);
            PrepareForInAnimation();
        }

        /// <summary>
        /// Callback invoked when an entrance animation has just finished.
        /// </summary>
        protected virtual void InAnimationDidFinish()
        {

        }

        /// <summary>
        /// Callback invoked when an exit animation is about to start.
        /// </summary>
        protected virtual void OutAnimationWillBegin()
        {
            PrepareForOutAnimation();
        }

        /// <summary>
        /// Callback invoked when an exit animation has just finished.
        /// </summary>
        protected virtual void OutAnimationDidFinish()
        {

        }

        /// <summary>
        /// Callback invoked when an idle animation is about to start.
        /// </summary>
        protected virtual void IdleAnimationWillBegin()
        {

        }

        /// <summary>
        /// Callback invoked when an idle animation has just finished.
        /// </summary>
        protected virtual void PingPongAnimationDidFinish()
        {

        }

        /// <summary>
        /// Callback invoked when the entrance animation progresses.
        /// </summary>
        /// <param name="value">The new value.</param>
        protected abstract void AnimateInUpdate(float value);

        /// <summary>
        /// Callback invoked when the entrance animation reaches its target value.
        /// </summary>
        protected void AnimateInComplete()
        {
            if (!gameObject.activeSelf)
            {
                return;
            }

            if (inAnimation.Sounds.End != null)
            {
                AudioManager.Instance.PlaySound(inAnimation.Sounds.End);
            }

            hasInAnimationBegan = false;
            isAnimatingIn = false;
            isInAnimationDone = true;

            if (outAnimation != null)
            {
                hasOutAnimationBegan = false;
                isAnimatingOut = false;
                isOutAnimationDone = false;
            }

            InAnimationDidFinish();

            animationObserver?.InAnimationFinished(inAnimation);
        }

        /// <summary>
        /// Callback invoked when the exit animation progresses.
        /// </summary>
        /// <param name="value">The new value.</param>
        protected abstract void AnimateOutUpdate(float value);

        /// <summary>
        /// Callback invoked when the exit animation reaches its target value.
        /// </summary>
        protected void AnimateOutComplete()
        {
            if (!gameObject.activeSelf)
            {
                return;
            }

            if (outAnimation.Sounds.End != null)
            {
                AudioManager.Instance.PlaySound(outAnimation.Sounds.End);
            }

            if (inAnimation != null)
            {
                hasInAnimationBegan = false;
                isAnimatingIn = false;
                isInAnimationDone = false;
            }

            hasOutAnimationBegan = false;
            isAnimatingOut = false;
            isOutAnimationDone = true;

            OutAnimationDidFinish();

            animationObserver?.OutAnimationFinished(outAnimation);
        }

        /// <summary>
        /// Callback invoked when the idle animation progresses.
        /// </summary>
        /// <param name="value">The new value.</param>
        protected abstract void AnimateIdleUpdate(float value);

        /// <summary>
        /// Callback invoked when the idle animation finishes.
        /// </summary>
        protected void AnimatePingPongComplete()
        {
            PingPongAnimationDidFinish();

            animationObserver?.PingPongAnimationFinished(idleAnimation);
        }

        #endregion

        #region Audio

        /// <summary>
        /// Starts the idle animation sound.
        /// </summary>
        protected void StartPingPongSound()
        {
            if (isAnimatingIdle || idleAnimation.Sound.Clip == null)
            {
                return;
            }

            isAnimatingIdle = true;
            idleAnimation.Sound.Source = null;

            if (idleAnimation.Sound.Loop)
            {
                idleAnimation.Sound.Source = AudioManager.Instance.PlaySound(idleAnimation.Sound.Clip, true);
            }
            else
            {
                AudioManager.Instance.PlaySound(idleAnimation.Sound.Clip);
            }
        }

        /// <summary>
        /// Stops the idle animation sound.
        /// </summary>
        private void StopPingPongSound()
        {
            if (idleAnimation.Sound.Clip != null && idleAnimation.Sound.Source != null)
            {
                idleAnimation.Sound.Source.Stop();
                idleAnimation.Sound.Source = null;
            }
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Gets all the animators from the children of this game object
        /// with the same type as this animator.
        /// </summary>
        /// <returns>The animators from the children of this game object
        /// with the same type as this animator.</returns>
        protected override AnimatorBehaviour[] GetChildAnimators()
        {
            Component[] childrenWithParent = gameObject.transform.GetComponentsInChildren(GetType());

            AnimatorBehaviour[] children = new AnimatorBehaviour[childrenWithParent.Length];
            int i = 0;

            foreach (AnimatorBehaviour child in childrenWithParent)
            {
                if (child.gameObject.GetInstanceID() != GetInstanceID())
                {
                    children[i] = child;
                    i++;
                }
            }

            return children;
        }

        #endregion
    }
}