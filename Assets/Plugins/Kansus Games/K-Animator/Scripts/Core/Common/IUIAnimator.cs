namespace KansusAnimator
{
    /// <summary>
    /// Defines an animator.
    /// </summary>
    public interface IAnimator
    {
        /// <summary>
        /// Executes the "in" animation of this animator, if any.
        /// </summary>
        /// <returns>Whether the animation was executed.</returns>
        bool AnimateIn();

        /// <summary>
        /// Executes the "out" animation of this animator, if any.
        /// </summary>
        /// <returns>Whether the animation was executed.</returns>
        bool AnimateOut();

        /// <summary>
        /// Executes the "ping pong" animation of this animator, if any.
        /// </summary>
        /// <returns>Whether the animation was executed.</returns>
        bool AnimatePingPong();

        /// <summary>
        /// Stops the current animation.
        /// </summary>
        void StopAnimation();

        /// <summary>
        /// Stops the current "ping pong" animation.
        /// </summary>
        void StopPingPongAnimation();

        /// <summary>
        /// Checks whether this animator is performing any animation.
        /// </summary>
        /// <returns>A boolean indicating if any animation is in progress.</returns>
        bool IsAnimating();
    }
}