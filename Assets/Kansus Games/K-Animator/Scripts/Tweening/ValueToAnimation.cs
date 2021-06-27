using KansusGames.KansusAnimator.Core;
using System;
using UnityEngine;

namespace KansusGames.KansusAnimator.Tweening
{
    /// <summary>
    /// Holds the parameters of a "value to" animation.
    /// </summary>
    public class ValueToAnimation
    {
        #region Properties

        /// <summary>
        /// The game object to be animated.
        /// </summary>
        public GameObject GameObject { get; set; }

        /// <summary>
        /// The start value of the animation.
        /// </summary>
        public float From { get; set; }

        /// <summary>
        /// The end value of the animation.
        /// </summary>
        public float To { get; set; }

        /// <summary>
        /// The total time of the animation.
        /// </summary>
        public float Duration { get; set; }

        /// <summary>
        /// The delay before the animation starts.
        /// </summary>
        public float Delay { get; set; }

        /// <summary>
        /// The type of easing applied to the animation.
        /// </summary>
        public EaseType EaseType { get; set; }

        /// <summary>
        /// Whether the animation should loop in idle style.
        /// </summary>
        public bool PingPongLoop { get; set; }

        /// <summary>
        /// The callback that will be invoked when the animation starts.
        /// </summary>
        public Action OnStart { get; set; }

        /// <summary>
        /// The callback that will be invoked in every update of the animation.
        /// </summary>
        public Action<float> OnUpdate { get; set; }

        /// <summary>
        /// The callback that will be invoked when the animation finishes.
        /// </summary>
        public Action OnComplete { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a representation of a "value to" tween animation.
        /// </summary>
        /// <param name="gameObject">The game object to be animated.</param>
        /// <param name="onUpdate">The callback that will be invoked in every update of the animation</param>
        /// <param name="from">The start value of the animation.</param>
        /// <param name="to">The end value of the animation.</param>
        /// <param name="time">The total time of the animation.</param>
        public ValueToAnimation(GameObject gameObject, Action<float> onUpdate, float from, float to, float time)
        {
            GameObject = gameObject;
            OnUpdate = onUpdate;
            From = from;
            To = to;
            Duration = time;
        }

        #endregion

        #region Builder methods

        /// <summary>
        /// Sets the delay before the animation starts.
        /// </summary>
        /// <param name="delay"></param>
        /// <returns></returns>
        public ValueToAnimation SetDelay(float delay)
        {
            Delay = delay;
            return this;
        }

        /// <summary>
        /// Sets the type of easing applied to the animation.
        /// </summary>
        /// <param name="easeType"></param>
        /// <returns>Self reference.</returns>
        public ValueToAnimation SetEaseType(EaseType easeType)
        {
            EaseType = easeType;
            return this;
        }

        /// <summary>
        /// Sets whether the animation should loop in idle style.
        /// </summary>
        /// <param name="pingPongLoop">Whether the animation should loop in
        /// idle style.</param>
        /// <returns>Self reference.</returns>
        public ValueToAnimation SetPingPongLoop(bool pingPongLoop)
        {
            PingPongLoop = pingPongLoop;
            return this;
        }

        /// <summary>
        /// Sets the callback that will be invoked when the animation finishes.
        /// </summary>
        /// <param name="action">The callback.</param>
        /// <returns>Self reference.</returns>
        public ValueToAnimation SetOnStart(Action action)
        {
            OnStart = action;
            return this;
        }

        /// <summary>
        /// Sets the callback that will be invoked when the animation finishes.
        /// </summary>
        /// <param name="action">The callback.</param>
        /// <returns>Self reference.</returns>
        public ValueToAnimation SetOnComplete(Action action)
        {
            OnComplete = action;
            return this;
        }

        #endregion
    }
}