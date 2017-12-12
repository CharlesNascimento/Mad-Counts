using KansusAnimator.Animations;
using UnityEngine;
using UnityEngine.UI;

namespace KansusAnimator.Animators
{
    /// <summary>
    /// Component that animates the transparency of its game object.
    /// </summary>
    [Disallow(typeof(KAnimator))]
    [DisallowMultipleComponent]
    public class FadeAnimator : ValueAnimator<FadeInAnimation, FadeOutAnimation, FadePingPongAnimation>
    {
        #region Fields - Animation

        private float initialFade;
        private float initialTextOutlineFade;
        private float initialTextShadowFade;

        #endregion

        #region Fields - Cached

        private Slider slider;
        private Text text;
        private Image image;
        private RawImage rawImage;
        private Outline textOutline;
        private Shadow textShadow;
        private Toggle toggle;
        private CanvasGroup canvasGroup;
        private SpriteRenderer spriteRenderer;
        private MeshRenderer meshRenderer;

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
                    inAnimation = Instantiate(inAnimation) as FadeInAnimation;
                }

                if (outAnimation != null)
                {
                    outAnimation = Instantiate(outAnimation) as FadeOutAnimation;
                }

                if (pingPongAnimation != null)
                {
                    pingPongAnimation = Instantiate(pingPongAnimation) as FadePingPongAnimation;
                }
            }

            image = gameObject.GetComponent<Image>();
            text = gameObject.GetComponent<Text>();
            textOutline = gameObject.GetComponent<Outline>();
            textShadow = gameObject.GetComponent<Shadow>();
            rawImage = gameObject.GetComponent<RawImage>();
            slider = gameObject.GetComponent<Slider>();
            toggle = gameObject.GetComponent<Toggle>();
            canvasGroup = gameObject.GetComponent<CanvasGroup>();
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            meshRenderer = gameObject.GetComponent<MeshRenderer>();

            initialFade = GetFadeValue();
            initialTextOutlineFade = GetTextOutlineFade();
            initialTextShadowFade = GetTextShadowFade();

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
                Fade(gameObject.transform, inAnimation.Fade, inAnimation.FadeChildren);
            }
        }

        /// <summary>
        /// Resets the state of the "out" animation.
        /// </summary>
        public override void ResetOut()
        {
            if (gameObject != null && outAnimation != null && outAnimation.IsEnabled)
            {
                Fade(gameObject.transform, 1, outAnimation.FadeChildren);
            }
        }

        #endregion

        #region Animation callbacks

        /// <summary>
        /// Callback invoked when the "in" animation progresses.
        /// </summary>
        /// <param name="value">The new value.</param>
        protected override void AnimateInUpdate(float value)
        {
            if (gameObject != null && gameObject.activeSelf && enabled)
            {
                float fade = inAnimation.Fade + ((initialFade - inAnimation.Fade) * value);
                Fade(gameObject.transform, fade, inAnimation.FadeChildren);
                if (textOutline != null)
                {
                    float num2 = value - 0.75f;

                    if (num2 < 0f)
                    {
                        num2 = 0f;
                    }

                    if (num2 > 0f)
                    {
                        num2 *= 4f;
                    }

                    float fadeOutline = initialTextOutlineFade * num2;
                    FadeTextOutline(fadeOutline);
                }
                if (textShadow != null)
                {
                    float num4 = value - 0.75f;

                    if (num4 < 0f)
                    {
                        num4 = 0f;
                    }

                    if (num4 > 0f)
                    {
                        num4 *= 4f;
                    }

                    float fadeShadow = initialTextShadowFade * num4;
                    FadeTextShadow(fadeShadow);
                }
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
                float fade = initialFade + ((outAnimation.Fade - initialFade) * value);
                Fade(gameObject.transform, fade, outAnimation.FadeChildren);

                if (textOutline != null)
                {
                    float num2 = value * 3f;

                    if (num2 > 1f)
                    {
                        num2 = 1f;
                    }

                    float fadeOutline = initialTextOutlineFade * (1f - num2);
                    FadeTextOutline(fadeOutline);
                }

                if (textShadow != null)
                {
                    float num4 = value * 3f;

                    if (num4 > 1f)
                    {
                        num4 = 1f;
                    }

                    float fadeShadow = initialTextShadowFade * (1f - num4);
                    FadeTextShadow(fadeShadow);
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
                float fadeValue = GetFadeValue();

                if (!pingPongAnimation.IsAnimating)
                {
                    StartPingPongSound();
                }

                fadeValue = pingPongAnimation.StartFade
                    + ((pingPongAnimation.EndFade - pingPongAnimation.StartFade) * value);
                Fade(gameObject.transform, fadeValue, pingPongAnimation.FadeChildren);

                if (textOutline != null)
                {
                    float diff = initialTextOutlineFade * value;
                    float fadeOutline = 0f;

                    if (diff > textOutline.effectColor.a)
                    {
                        diff = value - 0.75f;

                        if (diff < 0f)
                        {
                            diff = 0f;
                        }

                        if (diff > 0f)
                        {
                            diff *= 4f;

                        }

                        fadeOutline = initialTextOutlineFade * diff;
                        FadeTextOutline(fadeOutline);
                    }

                    if (diff < textOutline.effectColor.a)
                    {
                        diff = value * 2f;

                        if (diff > 1f)
                        {
                            diff = 1f;
                        }

                        fadeOutline = initialTextOutlineFade * (1f - diff);
                        FadeTextOutline(fadeOutline);
                    }
                }
                if (textShadow != null)
                {
                    float diff = initialTextShadowFade * value;
                    float fadeShadow = 0f;

                    if (diff > textShadow.effectColor.a)
                    {
                        diff = value - 0.75f;

                        if (diff < 0f)
                        {
                            diff = 0f;
                        }

                        if (diff > 0f)
                        {
                            diff *= 4f;
                        }

                        fadeShadow = initialTextShadowFade * diff;
                        FadeTextShadow(fadeShadow);
                    }

                    if (diff < textShadow.effectColor.a)
                    {
                        diff = value * 2f;

                        if (diff > 1f)
                        {
                            diff = 1f;
                        }

                        fadeShadow = initialTextShadowFade * (1f - diff);
                        FadeTextShadow(fadeShadow);
                    }
                }

                pingPongAnimation.Fade = fadeValue;
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
            pingPongAnimation.Fade = GetFadeValue();
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Sets the alpha value of any graphic component added to the given game object.
        /// </summary>
        /// <param name="transform">The transform of the game object.</param>
        /// <param name="fade">The new fade value.</param>
        /// <param name="shouldFadeChildren">Whether the new alpha value should be applied
        /// to the children of the given game object.</param>
        private void Fade(Transform transform, float fade, bool shouldFadeChildren)
        {
            bool isAnimatingChild = false;

            if (gameObject.transform != transform)
            {
                AnimatorBehaviour animator = transform.GetComponent<AnimatorBehaviour>();

                if (animator != null)
                {
                    if (inAnimation.IsEnabled && inAnimation.HasBegan)
                    {
                        isAnimatingChild = true;
                    }
                    else if (outAnimation.IsEnabled && outAnimation.HasBegan)
                    {
                        isAnimatingChild = true;
                    }
                }
            }

            if (!isAnimatingChild)
            {
                Image image = transform.gameObject.GetComponent<Image>();

                if (image != null)
                {
                    image.color = new Color(image.color.r, image.color.g, image.color.b, fade);
                }

                Text text = transform.gameObject.GetComponent<Text>();

                if (text != null)
                {
                    text.color = new Color(text.color.r, text.color.g, text.color.b, fade);
                }

                RawImage rawImage = transform.gameObject.GetComponent<RawImage>();

                if (rawImage != null)
                {
                    rawImage.color = new Color(rawImage.color.r, rawImage.color.g, rawImage.color.b, fade);
                }

                SpriteRenderer sprite = transform.gameObject.GetComponent<SpriteRenderer>();

                if (sprite != null)
                {
                    sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, fade);
                }

                CanvasGroup canvasGroup = transform.gameObject.GetComponent<CanvasGroup>();

                if (canvasGroup != null)
                {
                    canvasGroup.alpha = fade;
                }

                MeshRenderer meshRenderer = transform.gameObject.GetComponent<MeshRenderer>();

                if (meshRenderer != null)
                {
                    meshRenderer.material.color = new Color(
                        meshRenderer.material.color.r,
                        meshRenderer.material.color.g,
                        meshRenderer.material.color.b, fade
                    );
                }
            }

            if (shouldFadeChildren)
            {
                foreach (Transform childTransform in transform)
                {
                    if (childTransform.gameObject.activeSelf)
                    {
                        Fade(childTransform.transform, fade, shouldFadeChildren);
                    }
                }
            }
        }

        /// <summary>
        /// Sets the alpha value of the Text Outline component of this
        /// game object, if any.
        /// </summary>
        /// <param name="fade">The new fade value.</param>
        private void FadeTextOutline(float fade)
        {
            if (textOutline != null)
            {
                textOutline.effectColor = new Color(
                    textOutline.effectColor.r,
                    textOutline.effectColor.g,
                    textOutline.effectColor.b, fade
                );
            }
        }

        /// <summary>
        /// Sets the alpha value of the Text Shadow component of this
        /// game object, if any.
        /// </summary>
        /// <param name="fade">The new fade value.</param>
        private void FadeTextShadow(float fade)
        {
            if (textShadow != null)
            {
                textShadow.effectColor = new Color(
                    textShadow.effectColor.r,
                    textShadow.effectColor.g,
                    textShadow.effectColor.b, fade
                );
            }
        }

        /// <summary>
        /// Gets the alpha value of the Text Outline component of this
        /// game object, if any.
        /// </summary>
        /// <returns>The alpha value of the Text Outline component. If
        /// it does not exist, the return value is 1.</returns>
        private float GetTextOutlineFade()
        {
            float alpha = 1f;

            if (textOutline != null)
            {
                alpha = textOutline.effectColor.a;
            }

            return alpha;
        }

        /// <summary>
        /// Gets the alpha value of the Text Shadow component of this
        /// game object, if any.
        /// </summary>
        /// <returns>The alpha value of the Text Shadow component. If
        /// it does not exist, the return value is 1.</returns>
        private float GetTextShadowFade()
        {
            float alpha = 1f;

            if (textShadow != null)
            {
                alpha = textShadow.effectColor.a;
            }

            return alpha;
        }

        /// <summary>
        /// Gets the alpha value of any graphic component added to this game object.
        /// </summary>
        /// <returns>The alpha value of the Text Shadow component. If
        /// it does not exist, the return value is 1.</returns>
        private float GetFadeValue()
        {
            float alpha = 1f;

            if (image != null)
            {
                alpha = image.color.a;
            }

            if (toggle != null)
            {
                alpha = toggle.GetComponentInChildren<Image>().color.a;
            }

            if (text != null)
            {
                alpha = text.color.a;
            }

            if (rawImage != null)
            {
                alpha = rawImage.color.a;
            }

            if (slider != null)
            {
                alpha = slider.colors.normalColor.a;
            }

            if (canvasGroup != null)
            {
                alpha = canvasGroup.alpha;
            }

            if (spriteRenderer != null)
            {
                alpha = spriteRenderer.color.a;
            }

            if (meshRenderer != null)
            {
                alpha = meshRenderer.material.color.a;
            }

            return alpha;
        }
    }

    #endregion
}