#if ITWEEN

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KansusAnimator.Adapters
{
    /// <summary>
    /// Adapter that allows iTween to be used as a tween engine.
    /// </summary>
    public class ITweenAdapter : ITweener
    {
        #region Fields

        private static ITweenAdapter instance;

        private Dictionary<EaseType, iTween.EaseType> tweenTypeDictionary;

        private ulong tweenIdentifier = 0;

        #endregion

        #region Properties

        /// <summary>
        /// Singleton instance of this class.
        /// </summary>
        public static ITweenAdapter Instance
        {
            get
            {
                if (instance == null)
                    instance = new ITweenAdapter();

                return instance;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        private ITweenAdapter()
        {
            InitTweenTypeDictionary();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Tweens a value from "From" to "To".
        /// </summary>
        /// <param name="data">The tween data.</param>
        /// <returns>The started tween reference.</returns>
        public Tween StartValueTo(ValueToAnimation data)
        {
            string id = tweenIdentifier.ToString();
            iTween.EaseType easeType = tweenTypeDictionary[data.EaseType];

            Hashtable args = new Hashtable();
            args["name"] = id;
            args["onupdatetarget"] = data.GameObject;
            args["onupdate"] = data.OnUpdate;
            args["from"] = 0f;
            args["to"] = 1f;
            args["time"] = data.Duration;
            args["delay"] = data.Delay;
            args["easetype"] = easeType;
            args["oncompletetarget"] = data.GameObject;
            args["oncomplete"] = data.OnComplete;

            if (data.PingPongLoop)
            {
                args["looptype"] = iTween.LoopType.pingPong;
            }

            iTween.ValueTo(data.GameObject, args);

            tweenIdentifier++;
            return new Tween(id);
        }

        /// <summary>
        /// Stops the given tween.
        /// </summary>
        /// <param name="tween">The tween data.</param>
        public void Stop(Tween tween)
        {
            if (tween.Data is string)
            {
                iTween.StopByName(tween.Data as string);
            }
            else
            {
                Debug.LogWarning("Can't cancel tween due to unexpected data.");
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Initializes the dictionary that maps a KAnimator.EaseType to its corresponding iTween.EaseType.
        /// </summary>
        private void InitTweenTypeDictionary()
        {
            tweenTypeDictionary = new Dictionary<EaseType, iTween.EaseType>();

            tweenTypeDictionary[EaseType.InQuad] = iTween.EaseType.easeInQuad;
            tweenTypeDictionary[EaseType.OutQuad] = iTween.EaseType.easeOutQuad;
            tweenTypeDictionary[EaseType.InOutQuad] = iTween.EaseType.easeInOutQuad;
            tweenTypeDictionary[EaseType.InCubic] = iTween.EaseType.easeInCubic;
            tweenTypeDictionary[EaseType.OutCubic] = iTween.EaseType.easeOutCubic;
            tweenTypeDictionary[EaseType.InOutCubic] = iTween.EaseType.easeInOutCubic;
            tweenTypeDictionary[EaseType.OutQuart] = iTween.EaseType.easeOutQuart;
            tweenTypeDictionary[EaseType.InOutQuart] = iTween.EaseType.easeInOutQuart;
            tweenTypeDictionary[EaseType.InQuint] = iTween.EaseType.easeInQuint;
            tweenTypeDictionary[EaseType.OutQuint] = iTween.EaseType.easeOutQuint;
            tweenTypeDictionary[EaseType.InOutQuint] = iTween.EaseType.easeInOutQuint;
            tweenTypeDictionary[EaseType.InSine] = iTween.EaseType.easeInSine;
            tweenTypeDictionary[EaseType.OutSine] = iTween.EaseType.easeOutSine;
            tweenTypeDictionary[EaseType.InOutSine] = iTween.EaseType.easeInOutSine;
            tweenTypeDictionary[EaseType.InExpo] = iTween.EaseType.easeInExpo;
            tweenTypeDictionary[EaseType.OutExpo] = iTween.EaseType.easeOutExpo;
            tweenTypeDictionary[EaseType.InOutExpo] = iTween.EaseType.easeInOutExpo;
            tweenTypeDictionary[EaseType.InCirc] = iTween.EaseType.easeInCirc;
            tweenTypeDictionary[EaseType.OutCirc] = iTween.EaseType.easeOutCirc;
            tweenTypeDictionary[EaseType.InOutCirc] = iTween.EaseType.easeInOutCirc;
            tweenTypeDictionary[EaseType.linear] = iTween.EaseType.linear;
            tweenTypeDictionary[EaseType.spring] = iTween.EaseType.spring;
            tweenTypeDictionary[EaseType.InBounce] = iTween.EaseType.easeInBounce;
            tweenTypeDictionary[EaseType.OutBounce] = iTween.EaseType.easeOutBounce;
            tweenTypeDictionary[EaseType.InOutBounce] = iTween.EaseType.easeInOutBounce;
            tweenTypeDictionary[EaseType.InBack] = iTween.EaseType.easeInBack;
            tweenTypeDictionary[EaseType.OutBack] = iTween.EaseType.easeOutBack;
            tweenTypeDictionary[EaseType.InOutBack] = iTween.EaseType.easeInOutBack;
            tweenTypeDictionary[EaseType.InElastic] = iTween.EaseType.easeInElastic;
            tweenTypeDictionary[EaseType.OutElastic] = iTween.EaseType.easeOutElastic;
            tweenTypeDictionary[EaseType.InOutElastic] = iTween.EaseType.easeInOutElastic;
        }

        #endregion
    }
}

#endif