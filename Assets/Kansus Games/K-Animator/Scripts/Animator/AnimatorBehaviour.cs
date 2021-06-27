using KansusGames.KansusAnimator.Core;
using UnityEngine;
using UnityEngine.UI;

namespace KansusGames.KansusAnimator.Animator
{
    /// <summary>
    /// Base component for an animator.
    /// </summary>
    public abstract class AnimatorBehaviour : MonoBehaviour, IAnimator
    {
        #region Fields

        [SerializeField]
        protected AnimatorConfiguration configuration;

        #endregion

        #region IAnimator

        /// <summary>
        /// Executes the "in" animation in the given context.
        /// </summary>
        /// <param name="context">The context of the animation.</param>
        /// <returns>Whether the animation was executed.</returns>
        public bool AnimateIn(AnimationContext context = AnimationContext.Self)
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

            Button button = transform.gameObject.GetComponent<Button>();

            if (button != null)
            {
                button.interactable = false;
            }

            return animated;
        }

        /// <summary>
        /// Executes the "out" animation in the given context.
        /// </summary>
        /// <param name="context">The context of the animation.</param>
        /// <returns>Whether the animation was executed.</returns>
        public bool AnimateOut(AnimationContext context = AnimationContext.Self)
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
        /// Executes the "idle" animation in the given context.
        /// </summary>
        /// <param name="context">The context of the animation.</param>
        /// <returns>Whether the animation was executed.</returns>
        public bool AnimateIdle(AnimationContext context = AnimationContext.Self)
        {
            bool animated = false;

            if (!CanRunAnimation(context))
            {
                return false;
            }

            switch (context)
            {
                case AnimationContext.Self:
                    animated |= AnimateIdle();
                    break;
                case AnimationContext.SelfAndChildren:
                    animated |= AnimateIdle();
                    animated |= AnimatePingPongChildren();
                    break;
                case AnimationContext.Children:
                    animated |= AnimatePingPongChildren();
                    break;
            }

            return animated;
        }

        /// <summary>
        /// Stops the current animation.
        /// </summary>
        /// <param name="context">The context of the animation.</param>
        public void StopAnimation(AnimationContext context = AnimationContext.Self)
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
        /// Stops the current idle animation.
        /// </summary>
        /// <param name="context">The context of the animation.</param>
        public void StopPingPongAnimation(AnimationContext context = AnimationContext.Self)
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
        /// Checks whether this animator is performing any animation.
        /// </summary>
        /// <param name="context">The context of the animation.</param>
        /// <returns>A boolean indicating if any animation is in progress.</returns>
        public bool IsAnimating(AnimationContext context = AnimationContext.Self)
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
                default:
                    return false;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Plays the entrance animation of this game object and its children.
        /// </summary>
        public void PlaySelfAndChildrenInAnimation()
        {
            AnimateIn(AnimationContext.SelfAndChildren);
        }

        /// <summary>
        /// Plays the entrance animation of this game object.
        /// </summary>
        public void PlaySelfInAnimation()
        {
            AnimateIn(AnimationContext.Self);
        }

        /// <summary>
        /// Plays the entrance animation of the children of this game object.
        /// </summary>
        public void PlayChildrenInAnimation()
        {
            AnimateIn(AnimationContext.Children);
        }

        /// <summary>
        /// Plays the exit animation of this game object and its children.
        /// </summary>
        public void PlaySelfAndChildrenOutAnimation()
        {
            AnimateOut(AnimationContext.SelfAndChildren);
        }

        /// <summary>
        /// Plays the exit animation of this game object.
        /// </summary>
        public void PlaySelfOutAnimation()
        {
            AnimateOut(AnimationContext.Self);
        }

        /// <summary>
        /// Plays the exit animation of the children of this game object.
        /// </summary>
        public void PlayChildrenOutAnimation()
        {
            AnimateOut(AnimationContext.Children);
        }

        /// <summary>
        /// Plays the idle animation of this game object and its children.
        /// </summary>
        public void PlaySelfAndChildrenIdleAnimation()
        {
            AnimateIdle(AnimationContext.SelfAndChildren);
        }

        /// <summary>
        /// Plays the idle animation of this game object.
        /// </summary>
        public void PlaySelfIdleAnimation()
        {
            AnimateIdle(AnimationContext.Self);
        }

        /// <summary>
        /// Plays the idle animation of the children of this game object.
        /// </summary>
        public void PlayChildrenIdleAnimation()
        {
            AnimateIdle(AnimationContext.Children);
        }

        /// <summary>
        /// Stops the entrance or exit animation of this game object and its children.
        /// </summary>
        public void StopSelfAndChildrenAnimation()
        {
            StopAnimation(AnimationContext.SelfAndChildren);
        }

        /// <summary>
        /// Stops the entrance or exit animation of this game object.
        /// </summary>
        public void StopSelfAnimation()
        {
            StopAnimation(AnimationContext.Self);
        }

        /// <summary>
        /// Stops the entrance or exit animation of the children of this game object.
        /// </summary>
        public void StopChildrenAnimation()
        {
            StopAnimation(AnimationContext.Children);
        }

        /// <summary>
        /// Stops the idle animation of this game object and its children.
        /// </summary>
        public void StopSelfAndChildrenIdleAnimation()
        {
            StopPingPongAnimation(AnimationContext.SelfAndChildren);
        }

        /// <summary>
        /// Stops the idle animation of this game object.
        /// </summary>
        public void StopSelfIdleAnimation()
        {
            StopPingPongAnimation(AnimationContext.Self);
        }

        /// <summary>
        /// Stops the idle animation of the children of this game object.
        /// </summary>
        public void StopChildrenIdleAnimation()
        {
            StopPingPongAnimation(AnimationContext.Children);
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Executes the "in" animation of this animator, if any.
        /// </summary>
        /// <returns>Whether the animation was executed.</returns>
        protected abstract bool AnimateIn();

        /// <summary>
        /// Executes the "out" animation of this animator, if any.
        /// </summary>
        /// <returns>Whether the animation was executed.</returns>
        protected abstract bool AnimateOut();

        /// <summary>
        /// Executes the "idle" animation of this animator, if any.
        /// </summary>
        /// <returns>Whether the animation was executed.</returns>
        protected abstract bool AnimateIdle();

        /// <summary>
        /// Checks whether this animator is performing any animation.
        /// </summary>
        /// <returns>A boolean indicating if any animation is in progress.</returns>
        protected abstract bool IsAnimating();

        /// <summary>
        /// Stops the current animation.
        /// </summary>
        protected abstract void StopAnimation();

        /// <summary>
        /// Stops the current idle animation.
        /// </summary>
        /// <param name="context">The context of the animation.</param>
        protected abstract void StopPingPongAnimation();

        /// <summary>
        /// Gets all the animators from the children of this game object
        /// with the same type as this animator.
        /// </summary>
        /// <returns>The animators from the children of this game object
        /// with the same type as this animator.</returns>
        protected abstract AnimatorBehaviour[] GetChildAnimators();

        /// <summary>
        /// Starts the "in" animation of the children animators.
        /// </summary>
        /// <returns>Whether any animation was executed.</returns>
        protected bool AnimateInChildren()
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
        /// Starts the "out" animation of the children animators.
        /// </summary>
        /// <returns>Whether any animation was executed.</returns>
        protected bool AnimateOutChildren()
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
        /// Starts the "idle" animation of the children animators.
        /// </summary>
        /// <returns>Whether any animation was executed.</returns>
        protected bool AnimatePingPongChildren()
        {
            bool animated = false;

            if (gameObject.transform.childCount > 0)
            {
                AnimatorBehaviour[] animators = GetChildAnimators();

                foreach (AnimatorBehaviour component in animators)
                {
                    if (component != null && component.enabled)
                    {
                        animated |= component.AnimateIdle();
                    }
                }
            }

            return animated;
        }

        /// <summary>
        /// Checks whether any child animator is performing any animation.
        /// </summary>
        /// <returns>A boolean indicating if any child animation is in progress.</returns>
        protected bool IsChildrenAnimating()
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

        /// <summary>
        /// Stops any running animation in the children animators.
        /// </summary>
        protected void StopChildrenAnimations()
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
        /// Stops any running animation in the children animators.
        /// </summary>
        protected void StopChildrenPingPongAnimations()
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
                switch (configuration.interruptionBehaviour)
                {
                    case AnimationInterruptionBehaviour.GiveUp:
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
    }
}