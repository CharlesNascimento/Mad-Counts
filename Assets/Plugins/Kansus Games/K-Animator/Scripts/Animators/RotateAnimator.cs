using KansusAnimator.Animations;
using UnityEngine;

namespace KansusAnimator.Animators
{
    /// <summary>
    /// Component that animates the rotation of its game object.
    /// </summary>
    [Disallow(typeof(KAnimator))]
    [DisallowMultipleComponent]
    public class RotateAnimator : ValueAnimator<RotateInAnimation, RotateOutAnimation, RotatePingPongAnimation>
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

            if (!initializedFromInit)
            {
                if (inAnimation != null)
                {
                    inAnimation = Instantiate(inAnimation) as RotateInAnimation;
                }

                if (outAnimation != null)
                {
                    outAnimation = Instantiate(outAnimation) as RotateOutAnimation;
                }

                if (pingPongAnimation != null)
                {
                    pingPongAnimation = Instantiate(pingPongAnimation) as RotatePingPongAnimation;
                }
            }

            initialRotation = transform.localRotation;

            ResetIn();
        }

        #endregion

        #region Public

        /// <summary>
        /// Resets the state of the "in" animation.
        /// </summary>
        public override void ResetIn()
        {
            if (gameObject != null && inAnimation != null && inAnimation.IsEnabled)
            {
                inAnimation.StartRotation = inAnimation.Rotation;
                inAnimation.EndRotation = initialRotation.eulerAngles;
            }
        }

        /// <summary>
        /// Resets the state of the "out" animation.
        /// </summary>
        public override void ResetOut()
        {
            if (gameObject != null && outAnimation != null && outAnimation.IsEnabled)
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
        /// Callback invoked when the "in" animation progresses.
        /// </summary>
        /// <param name="value">The new value.</param>
        protected override void AnimateInUpdate(float value)
        {
            if (gameObject != null && gameObject.activeSelf && enabled)
            {
                float x = inAnimation.StartRotation.x + ((inAnimation.EndRotation.x - inAnimation.StartRotation.x) * value);
                float y = inAnimation.StartRotation.y + ((inAnimation.EndRotation.y - inAnimation.StartRotation.y) * value);
                float z = inAnimation.StartRotation.z + ((inAnimation.EndRotation.z - inAnimation.StartRotation.z) * value);
                transform.localRotation = Quaternion.Euler(x, y, z);

                if (!inAnimation.IsAnimating && inAnimation.HasBegan)
                {
                    inAnimation.IsAnimating = true;

                    if (inAnimation.Sounds.AfterDelay != null)
                    {
                        AudioManager.Instance.PlaySoundOneShot(inAnimation.Sounds.AfterDelay);
                    }
                }
            }
        }

        /// <summary>
        /// Callback invoked when the "out" animation progresses.
        /// </summary>
        /// <param name="value">The new value.</param>
        protected override void AnimateOutUpdate(float value)
        {
            if (gameObject != null && gameObject.activeSelf && enabled)
            {
                float x = outAnimation.StartRotation.x
                    + ((outAnimation.EndRotation.x - outAnimation.StartRotation.x) * value);
                float y = outAnimation.StartRotation.y
                    + ((outAnimation.EndRotation.y - outAnimation.StartRotation.y) * value);
                float z = outAnimation.StartRotation.z
                    + ((outAnimation.EndRotation.z - outAnimation.StartRotation.z) * value);

                transform.localRotation = Quaternion.Euler(x, y, z);

                if (!outAnimation.IsAnimating && outAnimation.HasBegan)
                {
                    outAnimation.IsAnimating = true;
                    if (outAnimation.Sounds.AfterDelay != null)
                    {
                        AudioManager.Instance.PlaySoundOneShot(outAnimation.Sounds.AfterDelay);
                    }
                }
            }
        }

        /// <summary>
        /// Callback invoked when the "ping pong" animation progresses.
        /// </summary>
        /// <param name="value">The new value.</param>
        protected override void AnimatePingPongUpdate(float value)
        {
            if (gameObject != null && gameObject.activeSelf && enabled)
            {
                if (!pingPongAnimation.IsAnimating)
                {
                    StartPingPongSound();
                }

                float x = pingPongAnimation.StartRotation.x
                    + ((pingPongAnimation.EndRotation.x - pingPongAnimation.StartRotation.x) * value);
                float y = pingPongAnimation.StartRotation.y
                    + ((pingPongAnimation.EndRotation.y - pingPongAnimation.StartRotation.y) * value);
                float z = pingPongAnimation.StartRotation.z
                    + ((pingPongAnimation.EndRotation.z - pingPongAnimation.StartRotation.z) * value);

                transform.localRotation = Quaternion.Euler(x, y, z);

                pingPongAnimation.Rotation = transform.localRotation.eulerAngles;
            }
        }

        /// <summary>
        /// Callback invoked when an "in" animation has just finished.
        /// </summary>
        protected override void InAnimationDidFinish()
        {
            AnimateInUpdate(1f);
        }

        /// <summary>
        /// Callback invoked when an "out" animation is about to start.
        /// </summary>
        protected override void OutAnimationWillBegin()
        {
            outAnimation.StartRotation = transform.localRotation.eulerAngles;
            outAnimation.EndRotation = outAnimation.Rotation;
        }

        /// <summary>
        /// Callback invoked when an "out" animation has just finished.
        /// </summary>
        protected override void OutAnimationDidFinish()
        {
            AnimateOutUpdate(1f);
        }

        /// <summary>
        /// Callback invoked when a "ping pong" animation is about to start.
        /// </summary>
        protected override void PingPongAnimationWillBegin()
        {
            pingPongAnimation.Rotation = transform.localRotation.eulerAngles;
        }

        #endregion
    }
}