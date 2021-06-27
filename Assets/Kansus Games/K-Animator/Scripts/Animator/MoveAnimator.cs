using KansusGames.KansusAnimator.Animation;
using KansusGames.KansusAnimator.Attribute;
using KansusGames.KansusAnimator.Core;
using UnityEngine;
using UnityEngine.UI;

namespace KansusGames.KansusAnimator.Animator
{
    /// <summary>
    /// Component that animates the position of its game object.
    /// </summary>
    [Disallow(typeof(KAnimator))]
    [DisallowMultipleComponent]
    public class MoveAnimator : ValueAnimator<MoveInAnimation, MoveOutAnimation, MoveIdleAnimation>
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

            if (!initializedFromInit && idleAnimation != null)
            {
                idleAnimation.StartPosition += transform.localPosition;
                idleAnimation.EndPosition += transform.localPosition;
            }

            initialPosition = transform.localPosition;

            canvasRenderer = gameObject.GetComponent<CanvasRenderer>();
            toggle = gameObject.GetComponent<Toggle>();
            slider = gameObject.GetComponent<Slider>();

            renderer = GetComponent<Renderer>();
            collider = GetComponent<Collider>();

            CalculatePresetPositions();

            if (configuration.startOut)
            {
                PrepareForInAnimation();
            }
        }

        #endregion

        #region Public

        /// <summary>
        /// Performs the initial setups for the entrance animation to run.
        /// </summary>
        public override void PrepareForInAnimation()
        {
            if (inAnimation == null || isInAnimationDone)
            {
                return;
            }

            Transform parentTransform;

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

        /// <summary>
        /// Resets the state of the exit animation.
        /// </summary>
        public override void PrepareForOutAnimation()
        {
            if (outAnimation == null || isOutAnimationDone)
            {
                return;
            }

            Transform parentTransform;

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
                    break;
                case PresetPosition.LocalPosition:
                    outAnimation.EndPosition = new Vector3(outAnimation.Position.x, outAnimation.Position.y, transform.localPosition.z);
                    break;
                case PresetPosition.UpperScreenEdge:
                    outAnimation.EndPosition = parentTransform.InverseTransformPoint(containerTopCenter);
                    outAnimation.EndPosition = new Vector3(initialPosition.x, outAnimation.EndPosition.y, initialPosition.z);
                    break;
                case PresetPosition.LeftScreenEdge:
                    outAnimation.EndPosition = parentTransform.InverseTransformPoint(containerMiddleLeft);
                    outAnimation.EndPosition = new Vector3(outAnimation.EndPosition.x, initialPosition.y, initialPosition.z);
                    break;
                case PresetPosition.RightScreenEdge:
                    outAnimation.EndPosition = parentTransform.InverseTransformPoint(containerMiddleRight);
                    outAnimation.EndPosition = new Vector3(outAnimation.EndPosition.x, initialPosition.y, initialPosition.z);
                    break;
                case PresetPosition.BottomScreenEdge:
                    outAnimation.EndPosition = parentTransform.InverseTransformPoint(containerBottomCenter);
                    outAnimation.EndPosition = new Vector3(initialPosition.x, outAnimation.EndPosition.y, initialPosition.z);
                    break;
                case PresetPosition.UpperLeft:
                    outAnimation.EndPosition = parentTransform.InverseTransformPoint(containerTopLeft);
                    outAnimation.EndPosition = new Vector3(outAnimation.EndPosition.x, outAnimation.EndPosition.y, transform.localPosition.z);
                    break;
                case PresetPosition.UpperCenter:
                    outAnimation.EndPosition = parentTransform.InverseTransformPoint(containerTopCenter);
                    outAnimation.EndPosition = new Vector3(outAnimation.EndPosition.x, outAnimation.EndPosition.y, transform.localPosition.z);
                    break;
                case PresetPosition.UpperRight:
                    outAnimation.EndPosition = parentTransform.InverseTransformPoint(containerTopRight);
                    outAnimation.EndPosition = new Vector3(outAnimation.EndPosition.x, outAnimation.EndPosition.y, transform.localPosition.z);
                    break;
                case PresetPosition.MiddleLeft:
                    outAnimation.EndPosition = parentTransform.InverseTransformPoint(containerMiddleLeft);
                    outAnimation.EndPosition = new Vector3(outAnimation.EndPosition.x, outAnimation.EndPosition.y, transform.localPosition.z);
                    break;
                case PresetPosition.MiddleCenter:
                    outAnimation.EndPosition = parentTransform.InverseTransformPoint(containerMiddleCenter);
                    outAnimation.EndPosition = new Vector3(outAnimation.EndPosition.x, outAnimation.EndPosition.y, transform.localPosition.z);
                    break;
                case PresetPosition.MiddleRight:
                    outAnimation.EndPosition = parentTransform.InverseTransformPoint(containerMiddleRight);
                    outAnimation.EndPosition = new Vector3(outAnimation.EndPosition.x, outAnimation.EndPosition.y, transform.localPosition.z);
                    break;
                case PresetPosition.BottomLeft:
                    outAnimation.EndPosition = parentTransform.InverseTransformPoint(containerBottomLeft);
                    outAnimation.EndPosition = new Vector3(outAnimation.EndPosition.x, outAnimation.EndPosition.y, transform.localPosition.z);
                    break;
                case PresetPosition.BottomCenter:
                    outAnimation.EndPosition = parentTransform.InverseTransformPoint(containerBottomCenter);
                    outAnimation.EndPosition = new Vector3(outAnimation.EndPosition.x, outAnimation.EndPosition.y, transform.localPosition.z);
                    break;
                case PresetPosition.BottomRight:
                    outAnimation.EndPosition = parentTransform.InverseTransformPoint(containerBottomRight);
                    outAnimation.EndPosition = new Vector3(outAnimation.EndPosition.x, outAnimation.EndPosition.y, transform.localPosition.z);
                    break;
                case PresetPosition.SelfPosition:
                    outAnimation.EndPosition = initialPosition;
                    break;
            }

            transform.localPosition = initialPosition;
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
                if (slider != null || toggle != null || (canvasRenderer != null))
                {
                    RectTransform rTransform = gameObject.GetComponent<RectTransform>();
                    bounds.size = new Vector3(rTransform.rect.width, rTransform.rect.height, 0f);
                }
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

            if (IsUIElement())
            {
                if (parentCanvas == null)
                {
                    parentCanvas = GetParentCanvas();
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

        /// <summary>
        /// Returns the parent canvas of the given transform.
        /// </summary>
        /// <returns>The parent canvas.</returns>
        private Canvas GetParentCanvas()
        {
            for (Transform parent = transform.parent; parent != null; parent = parent.parent)
            {
                Canvas canvas = parent.gameObject.GetComponent<Canvas>();

                if (canvas != null)
                {
                    return canvas;
                }
            }

            return null;
        }

        #endregion

        #region Animation callbacks

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

            var distance = inAnimation.EndPosition - inAnimation.StartPosition;
            transform.localPosition = inAnimation.StartPosition + (distance * value);

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

            var distance = outAnimation.EndPosition - outAnimation.StartPosition;
            transform.localPosition = outAnimation.StartPosition + (distance * value);

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
        /// Callback invoked when an idle animation progresses.
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

            var distance = idleAnimation.EndPosition - idleAnimation.StartPosition;
            transform.localPosition = idleAnimation.StartPosition + (distance * value);

            idleAnimation.Position = transform.localPosition;
        }

        /// <summary>
        /// Callback invoked when an exit animation is about to start.
        /// </summary>
        protected override void OutAnimationWillBegin()
        {
            base.OutAnimationWillBegin();
            outAnimation.StartPosition = transform.localPosition;
        }

        /// <summary>
        /// Callback invoked when an idle animation is about to start.
        /// </summary>
        protected override void IdleAnimationWillBegin()
        {
            if (idleAnimation.MovePositionSpace == MoveContext.Self)
            {
                idleAnimation.StartPosition = transform.localPosition + idleAnimation.StartPosition;
                idleAnimation.EndPosition = transform.localPosition + idleAnimation.EndPosition;
            }
            else if (idleAnimation.MovePositionSpace == MoveContext.World)
            {
                var parent = transform.parent;

                if (parent != null)
                {
                    idleAnimation.StartPosition = parent.InverseTransformPoint(idleAnimation.StartPosition);
                    idleAnimation.EndPosition = parent.InverseTransformPoint(idleAnimation.EndPosition);
                }
            }

            idleAnimation.Position = transform.localPosition;
        }

        #endregion
    }
}