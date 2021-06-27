namespace KansusGames.KansusAnimator.Core
{
    /// <summary>
    /// Defines an animator.
    /// </summary>
    public interface IAnimator
    {
        /// <summary>
        /// Executes the entrance animation of this animator, if any.
        /// </summary>
        /// <param name="context">The context to which the animation shoud be applied to.</param>
        /// <returns>Whether the animation was executed.</returns>
        bool AnimateIn(AnimationContext context = AnimationContext.Self);

        /// <summary>
        /// Executes the exit animation of this animator, if any.
        /// </summary>
        /// <param name="context">The context to which the animation shoud be applied to.</param>
        /// <returns>Whether the animation was executed.</returns>
        bool AnimateOut(AnimationContext context = AnimationContext.Self);

        /// <summary>
        /// Executes the idle animation of this animator, if any.
        /// </summary>
        /// <param name="context">The context to which the animation shoud be applied to.</param>
        /// <returns>Whether the animation was executed.</returns>
        bool AnimateIdle(AnimationContext context = AnimationContext.Self);

        /// <summary>
        /// Stops the current animation.
        /// <param name="context">The context in which to stop the animation.</param>
        /// </summary>
        void StopAnimation(AnimationContext context = AnimationContext.Self);

        /// <summary>
        /// Stops the current idle animation.
        /// </summary>
        /// <param name="context">The context in which to stop the animation.</param>
        void StopPingPongAnimation(AnimationContext context = AnimationContext.Self);

        /// <summary>
        /// Checks whether this animator is performing any animation.
        /// </summary>
        /// <param name="context">The animation context in which to check.</param>
        /// <returns>A boolean indicating if any animation is in progress.</returns>
        bool IsAnimating(AnimationContext context = AnimationContext.Self);
    }
}
