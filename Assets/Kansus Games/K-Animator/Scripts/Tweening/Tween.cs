namespace KansusGames.KansusAnimator.Tweening
{
    /// <summary>
    /// Represents an existing tween animation.
    /// </summary>
    public class Tween
    {
        /// <summary>
        /// The data that defines this tween.
        /// </summary>
        public object Data { get; private set; }


        /// <summary>
        /// Creates a new tween representation.
        /// </summary>
        /// <param name="data">The data that defines this tween.</param>
        public Tween(object data)
        {
            Data = data;
        }
    }
}
