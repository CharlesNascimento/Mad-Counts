namespace KansusAnimator
{
    /// <summary>
    /// Protocol that generalizes a tween system.
    /// </summary>
    public interface ITweener
    {
        /// <summary>
        /// Tweens a value from "From" to "To".
        /// </summary>
        /// <param name="data">The tween data.</param>
        /// <returns>The started tween reference.</returns>
        Tween StartValueTo(ValueToAnimation data);

        /// <summary>
        /// Stops the given tween.
        /// </summary>
        /// <param name="tween">The tween.</param>
        void Stop(Tween tween);
    }
}