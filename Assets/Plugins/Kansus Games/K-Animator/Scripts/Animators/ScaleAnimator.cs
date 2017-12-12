using KansusAnimator.Animations;
using UnityEngine;

namespace KansusAnimator.Animators
{
    /// <summary>
    /// Component that animates the scale of its game object.
    /// </summary>
    [Disallow(typeof(KAnimator))]
    [DisallowMultipleComponent]
    public class ScaleAnimator : ValueAnimator<ScaleInAnimation, ScaleOutAnimation, ScalePingPongAnimation>
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

            if (!initializedFromInit)
            {
                if (inAnimation != null)
                {
                    inAnimation = Instantiate(inAnimation) as ScaleInAnimation;
                }

                if (outAnimation != null)
                {
                    outAnimation = Instantiate(outAnimation) as ScaleOutAnimation;
                }

                if (pingPongAnimation != null)
                {
                    pingPongAnimation = Instantiate(pingPongAnimation) as ScalePingPongAnimation;
                }
            }

            initialScale = transform.localScale;

            ResetIn();
            ResetOut();
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
                transform.localScale = inAnimation.StartScale;
            }
        }

        /// <summary>
        /// Resets the state of the "out" animation.
        /// </summary>
        public override void ResetOut()
        {
            if (gameObject != null && outAnimation != null && outAnimation.IsEnabled)
            {
                transform.localScale = outAnimation.StartScale;
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
                float x = (inAnimation.StartScale.x * initialScale.x) + ((initialScale.x - (inAnimation.StartScale.x * initialScale.x)) * value);
                float y = (inAnimation.StartScale.y * initialScale.y) + ((initialScale.y - (inAnimation.StartScale.y * initialScale.y)) * value);
                float z = (inAnimation.StartScale.z * initialScale.z) + ((initialScale.z - (inAnimation.StartScale.z * initialScale.z)) * value);
                transform.localScale = new Vector3(x, y, z);

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
                if (outAnimation.StartScale == initialScale)
                {
                    float scaleX = outAnimation.StartScale.x + ((outAnimation.EndScale.x - outAnimation.StartScale.x) * value);
                    float scaleY = outAnimation.StartScale.y + ((outAnimation.EndScale.y - outAnimation.StartScale.y) * value);
                    float scaleZ = outAnimation.StartScale.z + ((outAnimation.EndScale.z - outAnimation.StartScale.z) * value);

                    transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
                }
                else
                {
                    float x = Mathf.Lerp(outAnimation.StartScale.x, initialScale.x, value);
                    float y = Mathf.Lerp(outAnimation.StartScale.y, initialScale.y, value);
                    float z = Mathf.Lerp(outAnimation.StartScale.z, initialScale.z, value);

                    Vector3 vector = new Vector3(x, y, z);

                    float scaleX = vector.x + ((outAnimation.EndScale.x - vector.x) * value);
                    float scaleY = vector.y + ((outAnimation.EndScale.y - vector.y) * value);
                    float scaleZ = vector.z + ((outAnimation.EndScale.z - vector.z) * value);

                    transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
                }
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

                float newScaleX = (pingPongAnimation.StartScale.x * initialScale.x)
                    + (((pingPongAnimation.EndScale.x * initialScale.x)
                    - (pingPongAnimation.StartScale.x * initialScale.x)) * value);

                float newScaleY = (pingPongAnimation.StartScale.y * initialScale.y)
                    + (((pingPongAnimation.EndScale.y * initialScale.y)
                    - (pingPongAnimation.StartScale.y * initialScale.y)) * value);

                float newScaleZ = (pingPongAnimation.StartScale.z * initialScale.z)
                    + (((pingPongAnimation.EndScale.z * initialScale.z)
                    - (pingPongAnimation.StartScale.z * initialScale.z)) * value);

                transform.localScale = new Vector3(newScaleX, newScaleY, newScaleZ);

                pingPongAnimation.Scale = transform.localScale;
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
            outAnimation.StartScale = transform.localScale;
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
            pingPongAnimation.Scale = transform.localScale;
        }

        #endregion
    }
}