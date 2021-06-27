using KansusGames.KansusAnimator.Animation;
using KansusGames.KansusAnimator.Attribute;
using KansusGames.KansusAnimator.Core;
using UnityEngine;

namespace KansusGames.KansusAnimator.Animator
{
    /// <summary>
    /// Component that animates the rotation of its game object.
    /// </summary>
    [Disallow(typeof(KAnimator))]
    [DisallowMultipleComponent]
    public class RotateAnimator : ValueAnimator<RotateInAnimation, RotateOutAnimation, RotateIdleAnimation>
    {
        #region Fields - Animation

        private Quaternion initialRotation;

        #endregion

        #region MonoBehaviour

        /// <summary>
        /// MonoBehaviour Awake callback.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            initialRotation = transform.localRotation;

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
                inAnimation.StartRotation = inAnimation.Rotation;
                inAnimation.EndRotation = initialRotation.eulerAngles;
            }
        }

        /// <summary>
        /// Resets the state of the exit animation.
        /// </summary>
        public override void PrepareForOutAnimation()
        {
            if (outAnimation != null)
            {
                /*
                outAnimation.StartRotation = outAnimation.Rotation;
                outAnimation.EndRotation = initialRotation.eulerAngles;*/
                transform.localRotation = initialRotation;
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

            var distance = inAnimation.EndRotation - inAnimation.StartRotation;
            transform.localRotation = Quaternion.Euler(inAnimation.StartRotation + distance * value);

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

            var distance = outAnimation.EndRotation - outAnimation.StartRotation;
            transform.localRotation = Quaternion.Euler(outAnimation.StartRotation + distance * value);

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

            var distance = idleAnimation.EndRotation - idleAnimation.StartRotation;
            transform.localRotation = Quaternion.Euler(idleAnimation.StartRotation + distance * value);

            idleAnimation.Rotation = transform.localRotation.eulerAngles;
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
            outAnimation.StartRotation = transform.localRotation.eulerAngles;
            outAnimation.EndRotation = outAnimation.Rotation;
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
            idleAnimation.Rotation = transform.localRotation.eulerAngles;
        }

        #endregion
    }
}