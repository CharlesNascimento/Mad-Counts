using System;

namespace KansusAnimator
{
    /// <summary>
    /// Represents an existing tween animation.
    /// </summary>
    public class Tween
    {
        /// <summary>
        /// The data that defines this tween.
        /// </summary>
        public Object Data { get; private set; }


        /// <summary>
        /// Creates a new tween representation.
        /// </summary>
        /// <param name="data">The data that defines this tween.</param>
        public Tween(Object data)
        {
            Data = data;
        }
    }
}
