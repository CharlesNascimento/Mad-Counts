using KansusAnimator.Animations;
using System;
using UnityEngine;

namespace KansusAnimator
{
    using BaseValueAnimator = ValueAnimator<Animations.Animation, Animations.Animation, PingPongAnimation>;

    /// <summary>
    /// Component that animates a specific value of its game object.
    /// </summary>
    /// <typeparam name="T">The model of the "in" animation.</typeparam>
    /// <typeparam name="U">The model of the "out" animation.</typeparam>
    /// <typeparam name="V">The model of the "ping pong" animation.</typeparam>
    public abstract class ValueAnimator<T, U, V> : AnimatorBehaviour
    where T : Animations.Animation
    where U : Animations.Animation
    where V : PingPongAnimation
    {
        #region Fields

        protected ITweener tweener;

        protected IAnimationObserver animationObserver;

        protected bool initializedFromInit = false;

        #endregion

        #region Fields - Animation

        protected Tween tween;

        [SerializeField]
        protected T inAnimation;

        [SerializeField]
        protected U outAnimation;

        [SerializeField]
        protected V pingPongAnimation;

        #endregion

        #region Properties

        /// <summary>
        /// The "in" animation.
        /// </summary>
        public T InAnimation
        {
            get { return inAnimation; }
        }

        /// <summary>
        /// The "out" animation.
        /// </summary>
        public U OutAnimation
        {
            get { return outAnimation; }
        }

        /// <summary>
        /// The "ping pong" animation.
        /// </summary>
        public V PingPongAnimation
        {
            get { return pingPongAnimation; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes this component.
        /// </summary>
        /// <param name="animationSet">A set of the three animations (in, out and ping pong)
        /// to assign to this animator.</param>
        /// <param name="animationObserver">An observer that will be notified about animation
        /// events from this animator.</param>
        public void Init(AnimationSet<T, U, V> animationSet, IAnimationObserver animationObserver)
        {
            initializedFromInit = true;

            inAnimation = animationSet.InAnimation;
            outAnimation = animationSet.OutAnimation;
            pingPongAnimation = animationSet.PingPongAnimation;

            if (animationObserver != null)
            {
                this.animationObserver = animationObserver;
            }
        }

        #endregion

        #region MonoBehaviour

        /// <summary>
        /// MonoBehaviour Awake callback.
        /// </summary>
        protected virtual void Awake()
        {
            tweener = AnimationManager.Instance.Configuration.Tweener;
        }

        #endregion

        #region Public

        /// <summary>
        /// Executes the "in" animation of this animator, if any.
        /// </summary>
        /// <returns>Whether the animation was executed.</returns>
        public override bool AnimateIn()
        {
            if (inAnimation == null)
            {
                Debug.LogWarning("AnimateIn called in an animator with no associated 'in' animation");

                if (pingPongAnimation != null && !pingPongAnimation.HasBegan)
                {
                    AnimatePingPong();
                }

                return false;
            }

            if (inAnimation.IsEnabled && !inAnimation.HasBegan)
            {
                StopPingPongAnimation();

                InAnimationWillBegin();
                inAnimation.HasBegan = true;
                inAnimation.IsAnimating = false;
                inAnimation.IsDone = false;

                ValueToAnimation valueTo = new ValueToAnimation(
                    gameObject,
                    new Action<float>(AnimateInUpdate),
                    0f,
                    1f,
                    inAnimation.Duration / AnimationManager.Instance.Configuration.AnimationSpeed)
                    .SetDelay(inAnimation.Delay / AnimationManager.Instance.Configuration.AnimationSpeed)
                    .SetEaseType(inAnimation.EaseType)
                    .SetOnComplete(new Action(AnimateInComplete));

                tween = tweener.StartValueTo(valueTo);

                if (animationObserver != null)
                {
                    animationObserver.InAnimationStarted(inAnimation);
                }

                if (inAnimation.Sounds.Begin != null)
                {
                    AudioManager.Instance.PlaySoundOneShot(inAnimation.Sounds.Begin);
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Executes the "out" animation of this animator, if any.
        /// </summary>
        /// <returns>Whether the animation was executed.</returns>
        public override bool AnimateOut()
        {
            if (outAnimation == null)
            {
                Debug.LogWarning("AnimateOut called in an animator with no associated 'out' animation");

                if (pingPongAnimation != null && pingPongAnimation.HasBegan)
                {
                    StopPingPongAnimation();
                }

                return false;
            }

            if (outAnimation.IsEnabled && !outAnimation.HasBegan)
            {
                StopPingPongAnimation();

                OutAnimationWillBegin();
                outAnimation.HasBegan = true;
                outAnimation.IsAnimating = false;
                outAnimation.IsDone = false;

                ValueToAnimation valueTo = new ValueToAnimation(
                    gameObject,
                    new Action<float>(AnimateOutUpdate),
                    0f,
                    1f,
                    outAnimation.Duration / AnimationManager.Instance.Configuration.AnimationSpeed)
                    .SetDelay(outAnimation.Delay / AnimationManager.Instance.Configuration.AnimationSpeed)
                    .SetEaseType(outAnimation.EaseType)
                    .SetOnComplete(new Action(AnimateOutComplete));

                tween = tweener.StartValueTo(valueTo);

                if (animationObserver != null)
                {
                    animationObserver.OutAnimationStarted(outAnimation);
                }

                if (outAnimation.Sounds.Begin != null)
                {
                    AudioManager.Instance.PlaySoundOneShot(outAnimation.Sounds.Begin);
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Executes the "ping pong" animation of this animator, if any.
        /// </summary>
        /// <returns>Whether the animation was executed.</returns>
        public override bool AnimatePingPong()
        {
            if (pingPongAnimation == null)
            {
                Debug.LogWarning("AnimatePingPong called in an animator with no associated 'ping pong' animation");
                return false;
            }

            if (gameObject != null && gameObject.activeSelf && enabled)
            {
                StopPingPongAnimation();

                pingPongAnimation.HasBegan = true;
                pingPongAnimation.IsAnimating = false;
                pingPongAnimation.IsDone = false;
                PingPongAnimationWillBegin();

                ValueToAnimation valueTo = new ValueToAnimation(
                    gameObject,
                    new Action<float>(AnimatePingPongUpdate),
                    0f,
                    1f,
                    pingPongAnimation.Duration / AnimationManager.Instance.Configuration.AnimationSpeed)
                    .SetDelay(pingPongAnimation.Delay / AnimationManager.Instance.Configuration.AnimationSpeed)
                    .SetEaseType(pingPongAnimation.EaseType)
                    .SetPingPongLoop(true)
                    .SetOnComplete(new Action(AnimatePingPongComplete));

                tween = tweener.StartValueTo(valueTo);

                if (animationObserver != null)
                {
                    animationObserver.PingPongAnimationStarted(pingPongAnimation);
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks whether this animator is performing any animation.
        /// </summary>
        /// <returns>A boolean indicating if any animation is in progress.</returns>
        public override bool IsAnimating()
        {
            bool isAnimating = inAnimation != null && inAnimation.HasBegan;
            isAnimating |= outAnimation != null && outAnimation.HasBegan;

            return isAnimating;
        }

        /// <summary>
        /// Checks whether any animator of this game object is performing any animation.
        /// </summary>
        /// <returns>A boolean indicating if any animation is in progress.</returns>
        public bool IsGameObjectAnimating()
        {
            bool isAnimating = false;

            AnimatorBehaviour[] components = gameObject.GetComponents<AnimatorBehaviour>();

            foreach (AnimatorBehaviour component in components)
            {
                if (component != null && component.enabled)
                {
                    isAnimating |= component.IsAnimating();
                }
            }

            return isAnimating;
        }

        /// <summary>
        /// Stops the current animation.
        /// </summary>
        public override void StopAnimation()
        {
            if (tween != null)
            {
                tweener.Stop(tween);
                tween = null;
            }

            if (inAnimation != null)
            {
                if (inAnimation.HasBegan)
                {
                    if (animationObserver != null)
                    {
                        animationObserver.InAnimationStopped(inAnimation);
                    }
                }

                inAnimation.IsAnimating = false;
                inAnimation.HasBegan = false;
                inAnimation.IsDone = false;
            }

            if (outAnimation != null)
            {
                if (outAnimation.HasBegan)
                {
                    if (animationObserver != null)
                    {
                        animationObserver.OutAnimationStopped(outAnimation);
                    }
                }

                outAnimation.IsAnimating = false;
                outAnimation.HasBegan = false;
                outAnimation.IsDone = false;
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
                    animator.StopAnimation();
                }
            }
        }

        /// <summary>
        /// Stops the current "ping pong" animation.
        /// </summary>
        public override void StopPingPongAnimation()
        {
            if (gameObject != null && pingPongAnimation != null && pingPongAnimation.HasBegan)
            {
                if (tween != null)
                {
                    tweener.Stop(tween);
                    tween = null;
                }

                AnimatePingPongUpdate(1);

                pingPongAnimation.IsAnimating = false;
                pingPongAnimation.HasBegan = false;
                pingPongAnimation.IsDone = true;

                StopPingPongSound();

                if (animationObserver != null)
                {
                    animationObserver.PingPongAnimationStopped(pingPongAnimation);
                }
            }
        }

        /// <summary>
        /// Resets the state of the "in" animation.
        /// </summary>
        public abstract void ResetIn();

        /// <summary>
        /// Resets the state of the "out" animation.
        /// </summary>
        public abstract void ResetOut();

        #endregion

        #region Callbacks

        /// <summary>
        /// Callback invoked when an "in" animation is about to start.
        /// </summary>
        protected virtual void InAnimationWillBegin()
        {

        }

        /// <summary>
        /// Callback invoked when an "in" animation has just finished.
        /// </summary>
        protected virtual void InAnimationDidFinish()
        {

        }

        /// <summary>
        /// Callback invoked when an "out" animation is about to start.
        /// </summary>
        protected virtual void OutAnimationWillBegin()
        {

        }

        /// <summary>
        /// Callback invoked when an "out" animation has just finished.
        /// </summary>
        protected virtual void OutAnimationDidFinish()
        {

        }

        /// <summary>
        /// Callback invoked when a "ping pong" animation is about to start.
        /// </summary>
        protected virtual void PingPongAnimationWillBegin()
        {

        }

        /// <summary>
        /// Callback invoked when a "ping pong" animation has just finished.
        /// </summary>
        protected virtual void PingPongAnimationDidFinish()
        {

        }

        /// <summary>
        /// Callback invoked when the "in" animation progresses.
        /// </summary>
        /// <param name="value">The new value.</param>
        protected abstract void AnimateInUpdate(float value);

        /// <summary>
        /// Callback invoked when the "in" animation reaches its target value.
        /// </summary>
        protected void AnimateInComplete()
        {
            if (gameObject != null && gameObject.activeSelf && enabled)
            {
                if (inAnimation.Sounds.End != null)
                {
                    AudioManager.Instance.PlaySoundOneShot(inAnimation.Sounds.End);
                }

                inAnimation.HasBegan = false;
                inAnimation.IsAnimating = false;
                inAnimation.IsDone = true;

                if (outAnimation != null)
                {
                    outAnimation.HasBegan = false;
                    outAnimation.IsAnimating = false;
                    outAnimation.IsDone = false;
                }

                InAnimationDidFinish();

                if (animationObserver != null)
                {
                    animationObserver.InAnimationFinished(inAnimation);
                }
            }
        }

        /// <summary>
        /// Callback invoked when the "out" animation progresses.
        /// </summary>
        /// <param name="value">The new value.</param>
        protected abstract void AnimateOutUpdate(float value);

        /// <summary>
        /// Callback invoked when the "out" animation reaches its target value.
        /// </summary>
        protected void AnimateOutComplete()
        {
            if (gameObject != null && gameObject.activeSelf && enabled)
            {
                if (outAnimation.Sounds.End != null)
                {
                    AudioManager.Instance.PlaySoundOneShot(outAnimation.Sounds.End);
                }

                if (inAnimation != null)
                {
                    inAnimation.HasBegan = false;
                    inAnimation.IsAnimating = false;
                    inAnimation.IsDone = false;
                }

                outAnimation.HasBegan = false;
                outAnimation.IsAnimating = false;
                outAnimation.IsDone = true;

                OutAnimationDidFinish();

                if (animationObserver != null)
                {
                    animationObserver.OutAnimationFinished(outAnimation);
                }
            }
        }

        /// <summary>
        /// Callback invoked when the "ping pong" animation progresses.
        /// </summary>
        /// <param name="value">The new value.</param>
        protected abstract void AnimatePingPongUpdate(float value);

        /// <summary>
        /// Callback invoked when the "ping pong" animation finishes.
        /// </summary>
        protected void AnimatePingPongComplete()
        {
            PingPongAnimationDidFinish();

            if (animationObserver != null)
            {
                animationObserver.PingPongAnimationFinished(pingPongAnimation);
            }
        }

        #endregion

        #region Audio

        /// <summary>
        /// Starts the "ping pong" animation sound.
        /// </summary>
        protected void StartPingPongSound()
        {
            if (!pingPongAnimation.IsAnimating && (pingPongAnimation.Sound.Clip != null))
            {
                pingPongAnimation.IsAnimating = true;
                pingPongAnimation.Sound.Source = null;

                if (pingPongAnimation.Sound.Loop)
                {
                    pingPongAnimation.Sound.Source = AudioManager.Instance.PlaySoundLoop(pingPongAnimation.Sound.Clip);
                }
                else
                {
                    AudioManager.Instance.PlaySoundOneShot(pingPongAnimation.Sound.Clip);
                }
            }
        }

        /// <summary>
        /// Stops the "ping pong" animation sound.
        /// </summary>
        private void StopPingPongSound()
        {
            if ((pingPongAnimation.Sound.Clip != null) && (pingPongAnimation.Sound.Source != null))
            {
                pingPongAnimation.Sound.Source.Stop();
                pingPongAnimation.Sound.Source = null;
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
            AnimatorBehaviour[] childrenWithParent = gameObject.transform.GetComponentsInChildren<BaseValueAnimator>();
            AnimatorBehaviour[] children = new AnimatorBehaviour[childrenWithParent.Length - 1];
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