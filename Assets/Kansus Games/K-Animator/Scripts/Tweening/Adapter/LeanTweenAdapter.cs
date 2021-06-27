#if LEANTWEEN

using KansusGames.KansusAnimator.Core;
using System.Collections.Generic;
using UnityEngine;

namespace KansusGames.KansusAnimator.Tweening.Adapter
{
    /// <summary>
    /// Adapter that allows Lean Tween to be used as a tween engine.
    /// </summary>
    public class LeanTweenAdapter : ITweener
    {
        #region Fields

        private static LeanTweenAdapter _instance;

        private Dictionary<EaseType, LeanTweenType> tweenTypeDictionary;

        #endregion

        #region Properties

        /// <summary>
        /// Singleton instance of this class.
        /// </summary>
        public static LeanTweenAdapter Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new LeanTweenAdapter();

                return _instance;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        private LeanTweenAdapter()
        {
            InitTweenTypeDictionary();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Initializes the dictionary that maps a KAnimator.EaseType to its corresponding LeanTweenType.
        /// </summary>
        private void InitTweenTypeDictionary()
        {
            tweenTypeDictionary = new Dictionary<EaseType, LeanTweenType>();

            tweenTypeDictionary[EaseType.InQuad] = LeanTweenType.easeInQuad;
            tweenTypeDictionary[EaseType.OutQuad] = LeanTweenType.easeOutQuad;
            tweenTypeDictionary[EaseType.InOutQuad] = LeanTweenType.easeInOutQuad;
            tweenTypeDictionary[EaseType.InCubic] = LeanTweenType.easeInCubic;
            tweenTypeDictionary[EaseType.OutCubic] = LeanTweenType.easeOutCubic;
            tweenTypeDictionary[EaseType.InOutCubic] = LeanTweenType.easeInOutCubic;
            tweenTypeDictionary[EaseType.OutQuart] = LeanTweenType.easeOutQuart;
            tweenTypeDictionary[EaseType.InOutQuart] = LeanTweenType.easeInOutQuart;
            tweenTypeDictionary[EaseType.InQuint] = LeanTweenType.easeInQuint;
            tweenTypeDictionary[EaseType.OutQuint] = LeanTweenType.easeOutQuint;
            tweenTypeDictionary[EaseType.InOutQuint] = LeanTweenType.easeInOutQuint;
            tweenTypeDictionary[EaseType.InSine] = LeanTweenType.easeInSine;
            tweenTypeDictionary[EaseType.OutSine] = LeanTweenType.easeOutSine;
            tweenTypeDictionary[EaseType.InOutSine] = LeanTweenType.easeInOutSine;
            tweenTypeDictionary[EaseType.InExpo] = LeanTweenType.easeInExpo;
            tweenTypeDictionary[EaseType.OutExpo] = LeanTweenType.easeOutExpo;
            tweenTypeDictionary[EaseType.InOutExpo] = LeanTweenType.easeInOutExpo;
            tweenTypeDictionary[EaseType.InCirc] = LeanTweenType.easeInCirc;
            tweenTypeDictionary[EaseType.OutCirc] = LeanTweenType.easeOutCirc;
            tweenTypeDictionary[EaseType.InOutCirc] = LeanTweenType.easeInOutCirc;
            tweenTypeDictionary[EaseType.linear] = LeanTweenType.linear;
            tweenTypeDictionary[EaseType.InBounce] = LeanTweenType.easeInBounce;
            tweenTypeDictionary[EaseType.OutBounce] = LeanTweenType.easeOutBounce;
            tweenTypeDictionary[EaseType.InOutBounce] = LeanTweenType.easeInOutBounce;
            tweenTypeDictionary[EaseType.InBack] = LeanTweenType.easeInBack;
            tweenTypeDictionary[EaseType.OutBack] = LeanTweenType.easeOutBack;
            tweenTypeDictionary[EaseType.InOutBack] = LeanTweenType.easeInOutBack;
            tweenTypeDictionary[EaseType.InElastic] = LeanTweenType.easeInElastic;
            tweenTypeDictionary[EaseType.OutElastic] = LeanTweenType.easeOutElastic;
            tweenTypeDictionary[EaseType.InOutElastic] = LeanTweenType.easeInOutElastic;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Animates a value from one value to another.
        /// </summary>
        /// <param name="data">The tween data.</param>
        /// <returns>The started tween reference.</returns>
        public Tween StartValueTo(ValueToAnimation data)
        {
            LeanTweenType easeType = tweenTypeDictionary[data.EaseType];

            LTDescr item = LeanTween.value(data.GameObject, data.OnUpdate, 0f, 1f, data.Duration)
                .setDelay(data.Delay)
                .setEase(easeType)
                .setOnComplete(data.OnComplete)
                .setOnStart(data.OnStart)
                .setUseEstimatedTime(true);

            if (data.PingPongLoop)
            {
                item.setLoopPingPong();
            }

            return new Tween(item);
        }

        /// <summary>
        /// Stops the given tween.
        /// </summary>
        /// <param name="tween">The tween data.</param>
        public void Stop(Tween tween)
        {
            if (tween.Data is LTDescr)
            {
                LeanTween.cancel((tween.Data as LTDescr).uniqueId);
            }
            else
            {
                Debug.LogWarning("Can't cancel tween due to unexpected data.");
            }
        }

        #endregion
    }
}

#endif