using KansusAnimator.Animations;

namespace KansusAnimator
{
    /// <summary>
    /// Listens for events during the animation process.
    /// </summary>
    public interface IAnimationObserver
    {
        /// <summary>
        /// Callback invoked when an "in" animation has just started.
        /// </summary>
        /// <param name="animation">The animation.</param>
        void InAnimationStarted(Animation animation);

        /// <summary>
        /// Callback invoked when an "in" animation has just finished.
        /// </summary>
        /// <param name="animation">The animation.</param>
        void InAnimationFinished(Animation animation);

        /// <summary>
        /// Callback invoked when an "in" animation is stopped before finishing.
        /// </summary>
        /// <param name="animation">The animation.</param>
        void InAnimationStopped(Animation animation);

        /// <summary>
        /// Callback invoked when an "out" animation has just started.
        /// </summary>
        /// <param name="animation">The animation.</param>
        void OutAnimationStarted(Animation animation);

        /// <summary>
        /// Callback invoked when an "out" animation has just finished.
        /// </summary>
        /// <param name="animation">The animation.</param>
        void OutAnimationFinished(Animation animation);

        /// <summary>
        /// Callback invoked when an "out" animation is stopped before finishing.
        /// </summary>
        /// <param name="animation">The animation.</param>
        void OutAnimationStopped(Animation animation);

        /// <summary>
        /// Callback invoked when a "ping-pong" animation has just started.
        /// </summary>
        /// <param name="animation">The animation.</param>
        void PingPongAnimationStarted(PingPongAnimation animation);

        /// <summary>
        /// Callback invoked when an "ping-pong" animation has just finished.
        /// </summary>
        /// <param name="animation">The animation.</param>
        void PingPongAnimationFinished(PingPongAnimation animation);

        /// <summary>
        /// Callback invoked when a "ping-pong" animation is stopped before finishing.
        /// </summary>
        /// <param name="animation">The animation.</param>
        void PingPongAnimationStopped(PingPongAnimation animation);
    }
}
