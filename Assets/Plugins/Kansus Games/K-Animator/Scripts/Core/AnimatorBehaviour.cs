using UnityEngine;

namespace KansusAnimator
{
    /// <summary>
    /// Base component for an animator.
    /// </summary>
    public abstract class AnimatorBehaviour : MonoBehaviour, IAnimator
    {
        #region Animation

        /// <summary>
        /// Executes the "in" animation of this animator, if any.
        /// </summary>
        /// <returns>Whether the animation was executed.</returns>
        public abstract bool AnimateIn();

        /// <summary>
        /// Executes the "in" animation in the given context.
        /// </summary>
        /// <param name="context">The context of the animation.</param>
        /// <returns>Whether the animation was executed.</returns>
        public bool AnimateIn(AnimationContext context)
        {
            bool animated = false;

            if (!CanRunAnimation(context))
            {
                return false;
            }

            switch (context)
            {
                case AnimationContext.Self:
                    animated |= AnimateIn();
                    break;
                case AnimationContext.SelfAndChildren:
                    animated |= AnimateIn();
                    animated |= AnimateInChildren();
                    break;
                case AnimationContext.Children:
                    animated |= AnimateInChildren();
                    break;
            }

            return animated;
        }

        /// <summary>
        /// Starts the "in" animation of the children animators.
        /// </summary>
        /// <returns>Whether any animation was executed.</returns>
        public bool AnimateInChildren()
        {
            bool animated = false;

            if (gameObject.transform.childCount > 0)
            {
                AnimatorBehaviour[] animators = GetChildAnimators();

                foreach (AnimatorBehaviour animator in animators)
                {
                    if (animator != null && animator.enabled)
                    {
                        animated |= animator.AnimateIn();
                    }
                }
            }

            return animated;
        }

        /// <summary>
        /// Executes the "out" animation of this animator, if any.
        /// </summary>
        /// <returns>Whether the animation was executed.</returns>
        public abstract bool AnimateOut();

        /// <summary>
        /// Executes the "out" animation in the given context.
        /// </summary>
        /// <param name="context">The context of the animation.</param>
        /// <returns>Whether the animation was executed.</returns>
        public bool AnimateOut(AnimationContext context)
        {
            bool animated = false;

            if (!CanRunAnimation(context))
            {
                return false;
            }

            switch (context)
            {
                case AnimationContext.Self:
                    animated |= AnimateOut();
                    break;
                case AnimationContext.SelfAndChildren:
                    animated |= AnimateOut();
                    animated |= AnimateOutChildren();
                    break;
                case AnimationContext.Children:
                    animated |= AnimateOutChildren();
                    break;
            }

            return animated;
        }

        /// <summary>
        /// Starts the "out" animation of the children animators.
        /// </summary>
        /// <returns>Whether any animation was executed.</returns>
        public bool AnimateOutChildren()
        {
            bool animated = false;

            if (gameObject.transform.childCount > 0)
            {
                AnimatorBehaviour[] animators = GetChildAnimators();

                foreach (AnimatorBehaviour animator in animators)
                {
                    if (animator != null && animator.enabled)
                    {
                        animated |= animator.AnimateOut();
                    }
                }
            }

            return animated;
        }

        /// <summary>
        /// Executes the "ping pong" animation of this animator, if any.
        /// </summary>
        /// <returns>Whether the animation was executed.</returns>
        public abstract bool AnimatePingPong();

        /// <summary>
        /// Executes the "ping pong" animation in the given context.
        /// </summary>
        /// <param name="context">The context of the animation.</param>
        /// <returns>Whether the animation was executed.</returns>
        public bool AnimatePingPong(AnimationContext context)
        {
            bool animated = false;

            if (!CanRunAnimation(context))
            {
                return false;
            }

            switch (context)
            {
                case AnimationContext.Self:
                    animated |= AnimatePingPong();
                    break;
                case AnimationContext.SelfAndChildren:
                    animated |= AnimatePingPong();
                    animated |= AnimatePingPongChildren();
                    break;
                case AnimationContext.Children:
                    animated |= AnimatePingPongChildren();
                    break;
            }

            return animated;
        }

        /// <summary>
        /// Starts the "ping pong" animation of the children animators.
        /// </summary>
        /// <returns>Whether any animation was executed.</returns>
        public bool AnimatePingPongChildren()
        {
            bool animated = false;

            if (gameObject.transform.childCount > 0)
            {
                AnimatorBehaviour[] animators = GetChildAnimators();

                foreach (AnimatorBehaviour component in animators)
                {
                    if (component != null && component.enabled)
                    {
                        animated |= component.AnimatePingPong();
                    }
                }
            }

            return animated;
        }

        #endregion

        #region Is animating

        /// <summary>
        /// Checks whether this animator is performing any animation.
        /// </summary>
        /// <returns>A boolean indicating if any animation is in progress.</returns>
        public abstract bool IsAnimating();

        /// <summary>
        /// Checks whether this animator is performing any animation.
        /// </summary>
        /// <param name="context">The context of the animation.</param>
        /// <returns>A boolean indicating if any animation is in progress.</returns>
        public bool IsAnimating(AnimationContext context)
        {
            switch (context)
            {
                case AnimationContext.Self:
                    return IsAnimating();
                case AnimationContext.SelfAndChildren:
                    bool isAnimating = IsAnimating();
                    isAnimating |= IsChildrenAnimating();
                    return isAnimating;
                case AnimationContext.Children:
                    return IsChildrenAnimating();
            }

            return IsChildrenAnimating() || IsAnimating();
        }

        /// <summary>
        /// Checks whether any child animator is performing any animation.
        /// </summary>
        /// <returns>A boolean indicating if any child animation is in progress.</returns>
        private bool IsChildrenAnimating()
        {
            bool isAnimating = false;

            if (gameObject.transform.childCount > 0)
            {
                AnimatorBehaviour[] animators = GetChildAnimators();

                foreach (AnimatorBehaviour animator in animators)
                {
                    if (animator != null && animator.enabled)
                    {
                        isAnimating |= animator.IsAnimating();
                    }
                }
            }

            return isAnimating;
        }

        #endregion

        #region Stop animation

        /// <summary>
        /// Stops the current animation.
        /// </summary>
        public abstract void StopAnimation();

        /// <summary>
        /// Stops the current animation.
        /// </summary>
        /// <param name="context">The context of the animation.</param>
        public void StopAnimation(AnimationContext context)
        {
            switch (context)
            {
                case AnimationContext.Self:
                    StopAnimation();
                    break;
                case AnimationContext.SelfAndChildren:
                    StopAnimation();
                    StopChildrenAnimations();
                    break;
                case AnimationContext.Children:
                    StopChildrenAnimations();
                    break;
            }
        }

        /// <summary>
        /// Stops any running animation in the children animators.
        /// </summary>
        private void StopChildrenAnimations()
        {
            if (gameObject.transform.childCount > 0)
            {
                AnimatorBehaviour[] animators = GetChildAnimators();

                foreach (AnimatorBehaviour animator in animators)
                {
                    if (animator != null && animator.enabled)
                    {
                        animator.StopAnimation();
                    }
                }
            }
        }

        /// <summary>
        /// Gets all the animators from the children of this game object
        /// with the same type as this animator.
        /// </summary>
        /// <returns>The animators from the children of this game object
        /// with the same type as this animator.</returns>
        protected abstract AnimatorBehaviour[] GetChildAnimators();

        /// <summary>
        /// Stops the current ping pong animation.
        /// </summary>
        /// <param name="context">The context of the animation.</param>
        public abstract void StopPingPongAnimation();

        /// <summary>
        /// Stops the current ping-pong animation.
        /// </summary>
        /// <param name="context">The context of the animation.</param>
        public void StopPingPongAnimation(AnimationContext context)
        {
            switch (context)
            {
                case AnimationContext.Self:
                    StopPingPongAnimation();
                    break;
                case AnimationContext.SelfAndChildren:
                    StopPingPongAnimation();
                    StopChildrenPingPongAnimations();
                    break;
                case AnimationContext.Children:
                    StopChildrenPingPongAnimations();
                    break;
            }
        }

        /// <summary>
        /// Stops any running animation in the children animators.
        /// </summary>
        private void StopChildrenPingPongAnimations()
        {
            if (gameObject.transform.childCount > 0)
            {
                AnimatorBehaviour[] animators = GetChildAnimators();

                foreach (AnimatorBehaviour animator in animators)
                {
                    if (animator != null && animator.enabled)
                    {
                        animator.StopPingPongAnimation();
                    }
                }
            }
        }

        #endregion

        #region Validation

        /// <summary>
        /// Checks whether an animation can start now.
        /// </summary>
        /// <param name="context">The AnimationContext of the animation.</param>
        /// <returns>A boolean indicating if an animation can start now.</returns>
        protected bool CanRunAnimation(AnimationContext context)
        {
            bool isValid = true;

            if (IsAnimating(context))
            {
                AnimatorConfiguration configuration = AnimationManager.Instance.Configuration;
                AnimationInterruptionBehaviour interruptionbehaviour = configuration.InterruptionBehavior;

                switch (interruptionbehaviour)
                {
                    case AnimationInterruptionBehaviour.LetItFinish:
                        Debug.LogWarning("Animation not executed because another animation is already running");
                        isValid = false;
                        break;
                    case AnimationInterruptionBehaviour.Interrupt:
                        Debug.LogWarning("Interrupting currently running animation, so another one can start");
                        StopAnimation(context);
                        StopPingPongAnimation(context);
                        isValid = true;
                        break;
                }
            }

            return isValid;
        }

        #endregion

        #region Visible to the inspector

        /// <summary>
        /// A visible to the inspector version of the AnimateIn(Context) method. The animation
        /// context defaults to SelfAndChildren.
        /// </summary>
        public void StartAnimateIn()
        {
            bool result = AnimateIn(AnimationContext.SelfAndChildren);

            if (result == false)
            {
                Debug.LogWarning("AnimateIn animation not executed.");
            }
        }

        /// <summary>
        /// A visible to the inspector version of the AnimateOut(Context) method. The animation
        /// context defaults to SelfAndChildren.
        /// </summary>
        public void StartAnimateOut()
        {
            bool result = AnimateOut(AnimationContext.SelfAndChildren);

            if (result == false)
            {
                Debug.LogWarning("AnimateOut animation not executed.");
            }
        }

        /// <summary>
        /// A visible to the inspector version of the AnimatePingPong(Context method. The animation
        /// context defaults to SelfAndChildren.
        /// </summary>
        public void StartAnimatePingPong()
        {
            bool result = AnimatePingPong(AnimationContext.SelfAndChildren);

            if (result == false)
            {
                Debug.LogWarning("AnimatePingPong animation not executed.");
            }
        }

        /// <summary>
        /// A visible to the inspector version of the StopAnimation(Context) method. The
        /// animation context defaults to SelfAndChildren.
        /// </summary>
        public void StopAnimationInspector()
        {
            StopAnimation(AnimationContext.SelfAndChildren);
        }

        /// <summary>
        /// A visible to the inspector version of the StopPingPongAnimation(Context) method. The
        /// animation context defaults to SelfAndChildren.
        /// </summary>
        public void StopPingPongAnimationInspector()
        {
            StopPingPongAnimation(AnimationContext.SelfAndChildren);
        }

        #endregion
    }
}