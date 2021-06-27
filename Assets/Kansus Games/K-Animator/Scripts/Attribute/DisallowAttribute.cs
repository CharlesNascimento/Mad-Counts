using System;

namespace KansusGames.KansusAnimator.Attribute
{
    /// <summary>
    /// Allows a component to specify other components that
    /// can't coexist with it in the same game object.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DisallowAttribute : System.Attribute
    {
        public readonly Type[] components;

        /// <summary>
        /// Creates a new instance of this class.
        /// </summary>
        /// <param name="disallowed">An array of disallowed components.</param>
        public DisallowAttribute(params Type[] disallowed)
        {
            components = disallowed;
        }
    }
}
