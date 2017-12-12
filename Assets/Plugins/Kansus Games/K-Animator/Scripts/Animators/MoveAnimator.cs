using KansusAnimator.Animations;
using UnityEngine;
using UnityEngine.UI;

namespace KansusAnimator.Animators
{
    /// <summary>
    /// Component that animates the position of its game object.
    /// </summary>
    [Disallow(typeof(KAnimator))]
    [DisallowMultipleComponent]
    public class MoveAnimator : ValueAnimator<MoveInAnimation, MoveOutAnimation, MovePingPongAnimation>
    {
        #region Fields - Bounds

        private Bounds gameObjectBounds;
        private Canvas parentCanvas;

        #endregion

        #region Fields - Container preset positions

        private Transform containerTransform;

        private Vector3 containerBottomCenter;
        private Vector3 containerBottomLeft;
        private Vector3 containerBottomRight;
        private Vector3 containerMiddleCenter;
        private Vector3 containerMiddleLeft;
        private Vector3 containerMiddleRight;
        private Vector3 containerTopCenter;
        private Vector3 containerTopLeft;
        private Vector3 containerTopRight;

        #endregion

        #region Fields - Animation

        private Vector3 initialPosition;

        #endregion

        #region Fields - Cached 

        private CanvasRenderer canvasRenderer;
        private Toggle toggle;
        private Slider slider;

        private new Renderer renderer;
        private new Collider collider;

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
                    inAnimation = Instantiate(inAnimation) as MoveInAnimation;
                }

                if (outAnimation != null)
                {
                    outAnimation = Instantiate(outAnimation) as MoveOutAnimation;
                }

                if (pingPongAnimation != null)
                {
                    pingPongAnimation = Instantiate(pingPongAnimation) as MovePingPongAnimation;
                    pingPongAnimation.StartPosition += transform.localPosition;
                    pingPongAnimation.EndPosition += transform.localPosition;
                }
            }

            initialPosition = transform.localPosition;

            canvasRenderer = gameObject.GetComponent<CanvasRenderer>();
            toggle = gameObject.GetComponent<Toggle>();
            slider = gameObject.GetComponent<Slider>();

            renderer = GetComponent<Renderer>();
            collider = GetComponent<Collider>();

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
            if (gameObject != null && inAnimation != null && inAnimation.IsEnabled && !inAnimation.IsDone)
            {
                CalculatePresetPositions();
                Transform parentTransform = null;

                if (transform.parent != null)
                {
                    parentTransform = transform.parent.transform;
                }
                else
                {
                    parentTransform = transform;
                }

                switch (inAnimation.MoveFrom)
                {
                    case PresetPosition.ParentPosition:
                        if (transform.parent != null)
                        {
                            inAnimation.StartPosition = new Vector3(0f, 0f, transform.localPosition.z);
                        }
                        break;

                    case PresetPosition.LocalPosition:
                        inAnimation.StartPosition = new Vector3(inAnimation.Position.x, inAnimation.Position.y, transform.localPosition.z);
                        break;

                    case PresetPosition.UpperScreenEdge:
                        inAnimation.StartPosition = parentTransform.InverseTransformPoint(containerTopCenter);
                        inAnimation.StartPosition = new Vector3(transform.localPosition.x, inAnimation.StartPosition.y, transform.localPosition.z);
                        break;

                    case PresetPosition.LeftScreenEdge:
                        inAnimation.StartPosition = parentTransform.InverseTransformPoint(containerMiddleLeft);
                        inAnimation.StartPosition = new Vector3(inAnimation.StartPosition.x, transform.localPosition.y, transform.localPosition.z);
                        break;

                    case PresetPosition.RightScreenEdge:
                        inAnimation.StartPosition = parentTransform.InverseTransformPoint(containerMiddleRight);
                        inAnimation.StartPosition = new Vector3(inAnimation.StartPosition.x, transform.localPosition.y, transform.localPosition.z);
                        break;

                    case PresetPosition.BottomScreenEdge:
                        inAnimation.StartPosition = parentTransform.InverseTransformPoint(containerBottomCenter);
                        inAnimation.StartPosition = new Vector3(transform.localPosition.x, inAnimation.StartPosition.y, transform.localPosition.z);
                        break;

                    case PresetPosition.UpperLeft:
                        inAnimation.StartPosition = parentTransform.InverseTransformPoint(containerTopLeft);
                        inAnimation.StartPosition = new Vector3(inAnimation.StartPosition.x, inAnimation.StartPosition.y, transform.localPosition.z);
                        break;

                    case PresetPosition.UpperCenter:
                        inAnimation.StartPosition = parentTransform.InverseTransformPoint(containerTopCenter);
                        inAnimation.StartPosition = new Vector3(inAnimation.StartPosition.x, inAnimation.StartPosition.y, transform.localPosition.z);
                        break;

                    case PresetPosition.UpperRight:
                        inAnimation.StartPosition = parentTransform.InverseTransformPoint(containerTopRight);
                        inAnimation.StartPosition = new Vector3(inAnimation.StartPosition.x, inAnimation.StartPosition.y, transform.localPosition.z);
                        break;

                    case PresetPosition.MiddleLeft:
                        inAnimation.StartPosition = parentTransform.InverseTransformPoint(containerMiddleLeft);
                        inAnimation.StartPosition = new Vector3(inAnimation.StartPosition.x, inAnimation.StartPosition.y, transform.localPosition.z);
                        break;

                    case PresetPosition.MiddleCenter:
                        inAnimation.StartPosition = parentTransform.InverseTransformPoint(containerMiddleCenter);
                        inAnimation.StartPosition = new Vector3(inAnimation.StartPosition.x, inAnimation.StartPosition.y, transform.localPosition.z);
                        break;

                    case PresetPosition.MiddleRight:
                        inAnimation.StartPosition = parentTransform.InverseTransformPoint(containerMiddleRight);
                        inAnimation.StartPosition = new Vector3(inAnimation.StartPosition.x, inAnimation.StartPosition.y, transform.localPosition.z);
                        break;

                    case PresetPosition.BottomLeft:
                        inAnimation.StartPosition = parentTransform.InverseTransformPoint(containerBottomLeft);
                        inAnimation.StartPosition = new Vector3(inAnimation.StartPosition.x, inAnimation.StartPosition.y, transform.localPosition.z);
                        break;

                    case PresetPosition.BottomCenter:
                        inAnimation.StartPosition = parentTransform.InverseTransformPoint(containerBottomCenter);
                        inAnimation.StartPosition = new Vector3(inAnimation.StartPosition.x, inAnimation.StartPosition.y, transform.localPosition.z);
                        break;

                    case PresetPosition.BottomRight:
                        inAnimation.StartPosition = parentTransform.InverseTransformPoint(containerBottomRight);
                        inAnimation.StartPosition = new Vector3(inAnimation.StartPosition.x, inAnimation.StartPosition.y, transform.localPosition.z);
                        break;

                    case PresetPosition.SelfPosition:
                        inAnimation.StartPosition = initialPosition;
                        break;
                }

                transform.localPosition = inAnimation.StartPosition;
                inAnimation.EndPosition = initialPosition;
            }
        }

        /// <summary>
        /// Resets the state of the "out" animation.
        /// </summary>
        public override void ResetOut()
        {
            if (gameObject != null && outAnimation != null && outAnimation.IsEnabled && !outAnimation.IsDone)
            {
                CalculatePresetPositions();
                Transform parentTransform = null;

                if (transform.parent != null)
                {
                    parentTransform = transform.parent.transform;
                }
                else
                {
                    parentTransform = transform;
                }

                switch (outAnimation.MoveTo)
                {
                    case PresetPosition.ParentPosition:
                        if (transform.parent != null)
                        {
                            outAnimation.EndPosition = new Vector3(0f, 0f, transform.localPosition.z);
                        }
                        return;

                    case PresetPosition.LocalPosition:
                        outAnimation.EndPosition = new Vector3(outAnimation.Position.x, outAnimation.Position.y, transform.localPosition.z);
                        return;

                    case PresetPosition.UpperScreenEdge:

                        outAnimation.EndPosition = parentTransform.InverseTransformPoint(containerTopCenter);
                        outAnimation.EndPosition = new Vector3(initialPosition.x, outAnimation.EndPosition.y, initialPosition.z);
                        return;

                    case PresetPosition.LeftScreenEdge:
                        outAnimation.EndPosition = parentTransform.InverseTransformPoint(containerMiddleLeft);
                        outAnimation.EndPosition = new Vector3(outAnimation.EndPosition.x, initialPosition.y, initialPosition.z);
                        return;

                    case PresetPosition.RightScreenEdge:
                        outAnimation.EndPosition = parentTransform.InverseTransformPoint(containerMiddleRight);
                        outAnimation.EndPosition = new Vector3(outAnimation.EndPosition.x, initialPosition.y, initialPosition.z);
                        return;

                    case PresetPosition.BottomScreenEdge:
                        outAnimation.EndPosition = parentTransform.InverseTransformPoint(containerBottomCenter);
                        outAnimation.EndPosition = new Vector3(initialPosition.x, outAnimation.EndPosition.y, initialPosition.z);
                        return;

                    case PresetPosition.UpperLeft:
                        outAnimation.EndPosition = parentTransform.InverseTransformPoint(containerTopLeft);
                        outAnimation.EndPosition = new Vector3(outAnimation.EndPosition.x, outAnimation.EndPosition.y, transform.localPosition.z);
                        return;

                    case PresetPosition.UpperCenter:
                        outAnimation.EndPosition = parentTransform.InverseTransformPoint(containerTopCenter);
                        outAnimation.EndPosition = new Vector3(outAnimation.EndPosition.x, outAnimation.EndPosition.y, transform.localPosition.z);
                        return;

                    case PresetPosition.UpperRight:
                        outAnimation.EndPosition = parentTransform.InverseTransformPoint(containerTopRight);
                        outAnimation.EndPosition = new Vector3(outAnimation.EndPosition.x, outAnimation.EndPosition.y, transform.localPosition.z);
                        return;

                    case PresetPosition.MiddleLeft:
                        outAnimation.EndPosition = parentTransform.InverseTransformPoint(containerMiddleLeft);
                        outAnimation.EndPosition = new Vector3(outAnimation.EndPosition.x, outAnimation.EndPosition.y, transform.localPosition.z);
                        return;

                    case PresetPosition.MiddleCenter:
                        outAnimation.EndPosition = parentTransform.InverseTransformPoint(containerMiddleCenter);
                        outAnimation.EndPosition = new Vector3(outAnimation.EndPosition.x, outAnimation.EndPosition.y, transform.localPosition.z);
                        return;

                    case PresetPosition.MiddleRight:
                        outAnimation.EndPosition = parentTransform.InverseTransformPoint(containerMiddleRight);
                        outAnimation.EndPosition = new Vector3(outAnimation.EndPosition.x, outAnimation.EndPosition.y, transform.localPosition.z);
                        return;

                    case PresetPosition.BottomLeft:
                        outAnimation.EndPosition = parentTransform.InverseTransformPoint(containerBottomLeft);
                        outAnimation.EndPosition = new Vector3(outAnimation.EndPosition.x, outAnimation.EndPosition.y, transform.localPosition.z);
                        return;

                    case PresetPosition.BottomCenter:
                        outAnimation.EndPosition = parentTransform.InverseTransformPoint(containerBottomCenter);
                        outAnimation.EndPosition = new Vector3(outAnimation.EndPosition.x, outAnimation.EndPosition.y, transform.localPosition.z);
                        return;

                    case PresetPosition.BottomRight:
                        outAnimation.EndPosition = parentTransform.InverseTransformPoint(containerBottomRight);
                        outAnimation.EndPosition = new Vector3(outAnimation.EndPosition.x, outAnimation.EndPosition.y, transform.localPosition.z);
                        return;

                    case PresetPosition.SelfPosition:
                        outAnimation.EndPosition = initialPosition;
                        return;
                }

                transform.localPosition = initialPosition;
            }
        }

        #endregion

        #region Bounds calculation

        /// <summary>
        /// Calculates the preset positions for this game object.
        /// </summary>
        private void CalculatePresetPositions()
        {
            gameObjectBounds = CalculateGameObjectBounds();
            Bounds containerBounds = CalculateContainerBounds();

            // Top left
            float topLeftX = -containerBounds.extents.x - gameObjectBounds.size.x * 1.5f;
            float topLeftY = containerBounds.extents.y + gameObjectBounds.size.y * 1.5f;
            Vector3 topLeftPosition = new Vector3(topLeftX, topLeftY, 0f);
            containerTopLeft = containerTransform.TransformPoint(topLeftPosition);

            // Top center
            float topCenterY = containerBounds.extents.y + gameObjectBounds.size.y * 1.5f;
            Vector3 topCenterPosition = new Vector3(0f, topCenterY, 0f);
            containerTopCenter = containerTransform.TransformPoint(topCenterPosition);

            // Top right
            float topRightX = containerBounds.extents.x + gameObjectBounds.size.x * 1.5f;
            float topRightY = containerBounds.extents.y + gameObjectBounds.size.y * 1.5f;
            Vector3 topRightPosition = new Vector3(topRightX, topRightY, 0f);
            containerTopRight = containerTransform.TransformPoint(topRightPosition);

            // Middle Left
            float middleLeftX = -containerBounds.extents.x - gameObjectBounds.size.x * 1.5f;
            Vector3 middleLeftPosition = new Vector3(middleLeftX, 0f, 0f);
            containerMiddleLeft = containerTransform.TransformPoint(middleLeftPosition);

            // Middle center
            Vector3 middleCenterPosition = new Vector3(0f, 0f, 0f);
            containerMiddleCenter = containerTransform.TransformPoint(middleCenterPosition);

            // Middle right
            float middleRightX = containerBounds.extents.x + gameObjectBounds.size.x * 1.5f;
            Vector3 middleRightPosition = new Vector3(middleRightX, 0f, 0f);
            containerMiddleRight = containerTransform.TransformPoint(middleRightPosition);

            // Bottom left
            float bottomLeftX = -containerBounds.extents.x - gameObjectBounds.size.x * 1.5f;
            float bottomLeftY = -containerBounds.extents.y - gameObjectBounds.size.y * 1.5f;
            Vector3 bottomLeftPosition = new Vector3(bottomLeftX, bottomLeftY, 0f);
            containerBottomLeft = containerTransform.TransformPoint(bottomLeftPosition);

            // Bottom center
            float bottomCenterY = -containerBounds.extents.y - gameObjectBounds.size.y * 1.5f;
            Vector3 bottomCenterPosition = new Vector3(0f, bottomCenterY, 0f);
            containerBottomCenter = containerTransform.TransformPoint(bottomCenterPosition);

            // Bottom right
            float bottomRightX = containerBounds.extents.x + gameObjectBounds.size.x * 1.5f;
            float bottomRightY = -containerBounds.extents.y - gameObjectBounds.size.y * 1.5f;
            Vector3 bottomRightPosition = new Vector3(bottomRightX, bottomRightY, 0f);
            containerBottomRight = containerTransform.TransformPoint(bottomRightPosition);
        }

        /// <summary>
        /// Calculates the bounds of this game object.
        /// </summary>
        /// <returns>The bounds of this game object.</returns>
        private Bounds CalculateGameObjectBounds()
        {
            Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);

            if (IsUIElement())
            {
                RectTransform rTransform = gameObject.GetComponent<RectTransform>();
                bounds.size = new Vector3(rTransform.rect.width, rTransform.rect.height, 0f);
            }
            else
            {
                if (renderer != null)
                {
                    bounds = renderer.bounds;
                }
                else
                {
                    if (collider != null)
                    {
                        bounds = collider.bounds;
                    }
                }
            }

            return bounds;
        }

        /// <summary>
        /// Calculates the bounds of the container of this game object. A container may be a canvas,
        /// in case this game object is an UI element, or an orthographic camera, otherwise.
        /// </summary>
        /// <returns>The bounds of the container of this game object.</returns>
        private Bounds CalculateContainerBounds()
        {
            Bounds bounds = new Bounds();

            if (gameObject != null)
            {
                if (IsUIElement())
                {
                    if (parentCanvas == null)
                    {
                        parentCanvas = AnimationManager.Instance.GetParentCanvas(transform);
                    }

                    if (parentCanvas != null)
                    {
                        RectTransform parentCanvasRectTransform = parentCanvas.GetComponent<RectTransform>();

                        bounds.extents = new Vector3(
                            (parentCanvasRectTransform.rect.width / 2f) + gameObjectBounds.size.x,
                            (parentCanvasRectTransform.rect.height / 2f) + gameObjectBounds.size.y
                        );

                        containerTransform = parentCanvasRectTransform;
                    }
                }
                else
                {
                    float verticalHightSeen = Camera.main.orthographicSize * 2.0f;
                    float HorizontalHeightSeen = verticalHightSeen * Screen.width / Screen.height;

                    bounds.extents = new Vector3(
                        (HorizontalHeightSeen / 2f) + gameObjectBounds.size.x,
                        (verticalHightSeen / 2f) + gameObjectBounds.size.y
                    );

                    containerTransform = Camera.main.transform;
                }
            }

            return bounds;
        }

        /// <summary>
        /// Checks whether this game object is an UI element.
        /// </summary>
        /// <returns>Whether this game object is an UI element.</returns>
        private bool IsUIElement()
        {
            return gameObject.transform is RectTransform;
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
                float x = inAnimation.StartPosition.x + ((inAnimation.EndPosition.x - inAnimation.StartPosition.x) * value);
                float y = inAnimation.StartPosition.y + ((inAnimation.EndPosition.y - inAnimation.StartPosition.y) * value);
                float z = inAnimation.StartPosition.z + ((inAnimation.EndPosition.z - inAnimation.StartPosition.z) * value);
                transform.localPosition = new Vector3(x, y, z);

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
            if (gameObject != null && gameObject.activeSelf && enabled && transform != null)
            {
                float x = outAnimation.StartPosition.x + ((outAnimation.EndPosition.x - outAnimation.StartPosition.x) * value);
                float y = outAnimation.StartPosition.y + ((outAnimation.EndPosition.y - outAnimation.StartPosition.y) * value);
                float z = outAnimation.StartPosition.z + ((outAnimation.EndPosition.z - outAnimation.StartPosition.z) * value);
                transform.localPosition = new Vector3(x, y, z);

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

                float newPositionX = pingPongAnimation.StartPosition.x
                    + ((pingPongAnimation.EndPosition.x - pingPongAnimation.StartPosition.x) * value);
                float newPositionY = pingPongAnimation.StartPosition.y
                    + ((pingPongAnimation.EndPosition.y - pingPongAnimation.StartPosition.y) * value);
                float newPositionZ = pingPongAnimation.StartPosition.z
                    + ((pingPongAnimation.EndPosition.z - pingPongAnimation.StartPosition.z) * value);

                transform.localPosition = new Vector3(newPositionX, newPositionY, newPositionZ);

                pingPongAnimation.Position = transform.localPosition;
            }
        }

        /// <summary>
        /// Callback invoked when an "out" animation is about to start.
        /// </summary>
        protected override void OutAnimationWillBegin()
        {
            outAnimation.StartPosition = transform.localPosition;
        }

        /// <summary>
        /// Callback invoked when a "ping pong" animation is about to start.
        /// </summary>
        protected override void PingPongAnimationWillBegin()
        {
            pingPongAnimation.Position = transform.localPosition;
        }

        #endregion
    }
}