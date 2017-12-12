using System;
using UnityEngine;

namespace KansusAnimator
{
    /// <summary>
    /// Extends the GameObject class.
    /// </summary>
    public static class GameObjectExtension
    {
        /// <summary>
        /// Adds a component to the given game object and allows
        /// an initialization code to be provided.
        /// </summary>
        /// <typeparam name="T">The type of the added component.</typeparam>
        /// <param name="gameObject">The game object.</param>
        /// <param name="initAction">An action executed before the "Awake" MonoBehaviour method.</param>
        /// <returns></returns>
        public static T AddComponent<T>(this GameObject gameObject, Action<T> initAction) where T : Component
        {
            using (gameObject.Deactivate())
            {
                T component = gameObject.AddComponent<T>();

                if (initAction != null)
                {
                    initAction(component);
                }

                return component;
            }
        }

        /// <summary>
        /// Create a new GameObjectDeactivateSection for the given game object.
        /// </summary>
        /// <param name="gameObject">The game object.</param>
        /// <returns>The new GameObjectDeactivateSection.</returns>
        public static IDisposable Deactivate(this GameObject gameObject)
        {
            return new GameObjectDeactivateSection(gameObject);
        }
    }

    /// <summary>
    /// A disposable section which deactivates a game object and 
    /// restores its original state when disposed.
    /// </summary>
    public class GameObjectDeactivateSection : IDisposable
    {
        private GameObject gameObject;
        private bool oldState;

        /// <summary>
        /// Creates a new GameObjectDeactivateSection.
        /// </summary>
        /// <param name="gameObject">The target game object.</param>
        public GameObjectDeactivateSection(GameObject gameObject)
        {
            this.gameObject = gameObject;
            oldState = this.gameObject.activeSelf;
            this.gameObject.SetActive(false);
        }

        /// <summary>
        /// Called at the end of the section.
        /// </summary>
        public void Dispose()
        {
            gameObject.SetActive(oldState);
        }
    }
}