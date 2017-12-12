using System;
using UnityEngine;

namespace KansusAnimator
{
    /// <summary>
    /// Holds the parameters of a "value to" animation.
    /// </summary>
    public class ValueToAnimation
    {
        #region Fields

        private GameObject gameObject;
        private Action<float> onUpdate;
        private float from;
        private float to;
        private float time;
        private float delay;
        private EaseType easeType;
        private bool pingPongLoop;
        private Action onComplete;

        #endregion

        #region Properties

        /// <summary>
        /// The game object to be animated.
        /// </summary>
        public GameObject GameObject
        {
            get
            {
                return gameObject;
            }

            set
            {
                gameObject = value;
            }
        }

        /// <summary>
        /// The start value of the animation.
        /// </summary>
        public float From
        {
            get
            {
                return from;
            }

            set
            {
                from = value;
            }
        }

        /// <summary>
        /// The end value of the animation.
        /// </summary>
        public float To
        {
            get
            {
                return to;
            }

            set
            {
                to = value;
            }
        }

        /// <summary>
        /// The total time of the animation.
        /// </summary>
        public float Duration
        {
            get
            {
                return time;
            }

            set
            {
                time = value;
            }
        }

        /// <summary>
        /// The delay before the animation starts.
        /// </summary>
        public float Delay
        {
            get
            {
                return delay;
            }

            set
            {
                delay = value;
            }
        }

        /// <summary>
        /// The type of easing applied to the animation.
        /// </summary>
        public EaseType EaseType
        {
            get
            {
                return easeType;
            }

            set
            {
                easeType = value;
            }
        }

        /// <summary>
        /// Whether the animation should loop in ping pong style.
        /// </summary>
        public bool PingPongLoop
        {
            get
            {
                return pingPongLoop;
            }

            set
            {
                pingPongLoop = value;
            }
        }

        /// <summary>
        /// The callback that will be invoked in every update of the animation.
        /// </summary>
        public Action<float> OnUpdate
        {
            get
            {
                return onUpdate;
            }

            set
            {
                onUpdate = value;
            }
        }

        /// <summary>
        /// The callback that will be invoked when the animation finishes.
        /// </summary>
        public Action OnComplete
        {
            get
            {
                return onComplete;
            }

            set
            {
                onComplete = value;
            }
        }

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
            this.gameObject = gameObject;
            this.onUpdate = onUpdate;
            this.from = from;
            this.to = to;
            this.time = time;
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
            this.delay = delay;
            return this;
        }

        /// <summary>
        /// Sets the type of easing applied to the animation.
        /// </summary>
        /// <param name="easeType"></param>
        /// <returns>Self reference.</returns>
        public ValueToAnimation SetEaseType(EaseType easeType)
        {
            this.easeType = easeType;
            return this;
        }

        /// <summary>
        /// Sets whether the animation should loop in ping pong style.
        /// </summary>
        /// <param name="pingPongLoop">Whether the animation should loop in
        /// ping pong style.</param>
        /// <returns>Self reference.</returns>
        public ValueToAnimation SetPingPongLoop(bool pingPongLoop)
        {
            this.pingPongLoop = pingPongLoop;
            return this;
        }

        /// <summary>
        /// Sets the callback that will be invoked when the animation finishes.
        /// </summary>
        /// <param name="action">The callback.</param>
        /// <returns>Self reference.</returns>
        public ValueToAnimation SetOnComplete(Action action)
        {
            this.onComplete = action;
            return this;
        } 

        #endregion
    }
}