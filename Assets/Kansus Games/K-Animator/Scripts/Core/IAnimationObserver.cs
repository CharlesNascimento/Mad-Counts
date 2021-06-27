using KansusGames.KansusAnimator.Animation.Base;

namespace KansusGames.KansusAnimator.Core
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
        void InAnimationStarted(Animation.Base.Animation animation);

        /// <summary>
        /// Callback invoked when an "in" animation has just finished.
        /// </summary>
        /// <param name="animation">The animation.</param>
        void InAnimationFinished(Animation.Base.Animation animation);

        /// <summary>
        /// Callback invoked when an "in" animation is stopped before finishing.
        /// </summary>
        /// <param name="animation">The animation.</param>
        void InAnimationStopped(Animation.Base.Animation animation);

        /// <summary>
        /// Callback invoked when an "out" animation has just started.
        /// </summary>
        /// <param name="animation">The animation.</param>
        void OutAnimationStarted(Animation.Base.Animation animation);

        /// <summary>
        /// Callback invoked when an "out" animation has just finished.
        /// </summary>
        /// <param name="animation">The animation.</param>
        void OutAnimationFinished(Animation.Base.Animation animation);

        /// <summary>
        /// Callback invoked when an "out" animation is stopped before finishing.
        /// </summary>
        /// <param name="animation">The animation.</param>
        void OutAnimationStopped(Animation.Base.Animation animation);

        /// <summary>
        /// Callback invoked when a "idle" animation has just started.
        /// </summary>
        /// <param name="animation">The animation.</param>
        void PingPongAnimationStarted(IdleAnimation animation);

        /// <summary>
        /// Callback invoked when an "idle" animation has just finished.
        /// </summary>
        /// <param name="animation">The animation.</param>
        void PingPongAnimationFinished(IdleAnimation animation);

        /// <summary>
        /// Callback invoked when a "idle" animation is stopped before finishing.
        /// </summary>
        /// <param name="animation">The animation.</param>
        void PingPongAnimationStopped(IdleAnimation animation);
    }
}
