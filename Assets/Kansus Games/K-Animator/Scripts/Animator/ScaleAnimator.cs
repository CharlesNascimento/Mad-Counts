using KansusGames.KansusAnimator.Animation;
using KansusGames.KansusAnimator.Attribute;
using KansusGames.KansusAnimator.Core;
using UnityEngine;

namespace KansusGames.KansusAnimator.Animator
{
    /// <summary>
    /// Component that animates the scale of its game object.
    /// </summary>
    [Disallow(typeof(KAnimator))]
    [DisallowMultipleComponent]
    public class ScaleAnimator : ValueAnimator<ScaleInAnimation, ScaleOutAnimation, ScaleIdleAnimation>
    {
        #region Fields

        private Vector3 initialScale;

        #endregion

        #region MonoBehaviour

        /// <summary>
        /// MonoBehaviour Awake callback.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            initialScale = transform.localScale;

            if (configuration.startOut)
            {
                PrepareForInAnimation();
            }
        }

        #endregion

        #region Public

        /// <summary>
        /// Resets the state of the entrance animation.
        /// </summary>
        public override void PrepareForInAnimation()
        {
            if (inAnimation != null)
            {
                transform.localScale = inAnimation.StartScale;
            }
        }

        /// <summary>
        /// Resets the state of the exit animation.
        /// </summary>
        public override void PrepareForOutAnimation()
        {
            if (outAnimation != null)
            {
                transform.localScale = outAnimation.StartScale;
            }
        }

        #endregion

        #region Animation Callbacks

        /// <summary>
        /// Callback invoked when the entrance animation progresses.
        /// </summary>
        /// <param name="value">The new value.</param>
        protected override void AnimateInUpdate(float value)
        {
            if (!gameObject.activeSelf)
            {
                return;
            }

            transform.localScale = Vector3.Scale(inAnimation.StartScale, initialScale)
                + (initialScale - Vector3.Scale(inAnimation.StartScale, initialScale)) * value;

            if (!isAnimatingIn && hasInAnimationBegan)
            {
                isAnimatingIn = true;

                if (inAnimation.Sounds.AfterDelay != null)
                {
                    AudioManager.Instance.PlaySound(inAnimation.Sounds.AfterDelay);
                }
            }
        }

        /// <summary>
        /// Callback invoked when the exit animation progresses.
        /// </summary>
        /// <param name="value">The new value.</param>
        protected override void AnimateOutUpdate(float value)
        {
            if (!gameObject.activeSelf)
            {
                return;
            }

            if (outAnimation.StartScale == initialScale)
            {
                var distance = outAnimation.EndScale - outAnimation.StartScale;
                transform.localScale = outAnimation.StartScale + (distance * value);
            }
            else
            {
                var start = Vector3.Lerp(outAnimation.StartScale, initialScale, value);
                transform.localScale = start + ((outAnimation.EndScale - start) * value);
            }

            if (!isAnimatingOut && hasOutAnimationBegan)
            {
                isAnimatingOut = true;

                if (outAnimation.Sounds.AfterDelay != null)
                {
                    AudioManager.Instance.PlaySound(outAnimation.Sounds.AfterDelay);
                }
            }
        }

        /// <summary>
        /// Callback invoked when the idle animation progresses.
        /// </summary>
        /// <param name="value">The new value.</param>
        protected override void AnimateIdleUpdate(float value)
        {
            if (!gameObject.activeSelf)
            {
                return;
            }

            if (!isAnimatingIdle)
            {
                StartPingPongSound();
            }

            transform.localScale = Vector3.Scale(idleAnimation.StartScale, initialScale)
                + ((Vector3.Scale(idleAnimation.EndScale, initialScale)
                - Vector3.Scale(idleAnimation.StartScale, initialScale)) * value);

            idleAnimation.Scale = transform.localScale;
        }

        /// <summary>
        /// Callback invoked when an entrance animation has just finished.
        /// </summary>
        protected override void InAnimationDidFinish()
        {
            AnimateInUpdate(1f);
        }

        /// <summary>
        /// Callback invoked when an exit animation is about to start.
        /// </summary>
        protected override void OutAnimationWillBegin()
        {
            outAnimation.StartScale = transform.localScale;
        }

        /// <summary>
        /// Callback invoked when an exit animation has just finished.
        /// </summary>
        protected override void OutAnimationDidFinish()
        {
            AnimateOutUpdate(1f);
        }

        /// <summary>
        /// Callback invoked when an idle animation is about to start.
        /// </summary>
        protected override void IdleAnimationWillBegin()
        {
            idleAnimation.Scale = transform.localScale;
        }

        #endregion
    }
}