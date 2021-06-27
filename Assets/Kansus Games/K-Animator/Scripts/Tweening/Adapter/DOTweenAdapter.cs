#if DOTWEEN

using KansusGames.KansusAnimator.Core;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace KansusGames.KansusAnimator.Tweening.Adapter
{
    /// <summary>
    /// Adapter that allows DOTween to be used as a tween engine.
    /// </summary>
    public class DOTweenAdapter : ITweener
    {
#region Fields

        private static DOTweenAdapter _instance;

        private Dictionary<EaseType, Ease> tweenTypeDictionary;

#endregion

#region Properties

        /// <summary>
        /// Singleton instance of this class.
        /// </summary>
        public static DOTweenAdapter Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DOTweenAdapter();

                return _instance;
            }
        }

#endregion

#region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        private DOTweenAdapter()
        {
            InitTweenTypeDictionary();
        }

#endregion

#region Private methods

        /// <summary>
        /// Initializes the dictionary that maps a KAnimator.EaseType to its corresponding Ease.
        /// </summary>
        private void InitTweenTypeDictionary()
        {
            tweenTypeDictionary = new Dictionary<EaseType, Ease>();

            tweenTypeDictionary[EaseType.InQuad] = Ease.InQuad;
            tweenTypeDictionary[EaseType.OutQuad] = Ease.OutQuad;
            tweenTypeDictionary[EaseType.InOutQuad] = Ease.InOutQuad;
            tweenTypeDictionary[EaseType.InCubic] = Ease.InCubic;
            tweenTypeDictionary[EaseType.OutCubic] = Ease.OutCubic;
            tweenTypeDictionary[EaseType.InOutCubic] = Ease.InOutCubic;
            tweenTypeDictionary[EaseType.OutQuart] = Ease.OutQuart;
            tweenTypeDictionary[EaseType.InOutQuart] = Ease.InOutQuart;
            tweenTypeDictionary[EaseType.InQuint] = Ease.InQuint;
            tweenTypeDictionary[EaseType.OutQuint] = Ease.OutQuint;
            tweenTypeDictionary[EaseType.InOutQuint] = Ease.InOutQuint;
            tweenTypeDictionary[EaseType.InSine] = Ease.InSine;
            tweenTypeDictionary[EaseType.OutSine] = Ease.OutSine;
            tweenTypeDictionary[EaseType.InOutSine] = Ease.InOutSine;
            tweenTypeDictionary[EaseType.InExpo] = Ease.InExpo;
            tweenTypeDictionary[EaseType.OutExpo] = Ease.OutExpo;
            tweenTypeDictionary[EaseType.InOutExpo] = Ease.InOutExpo;
            tweenTypeDictionary[EaseType.InCirc] = Ease.InCirc;
            tweenTypeDictionary[EaseType.OutCirc] = Ease.OutCirc;
            tweenTypeDictionary[EaseType.InOutCirc] = Ease.InOutCirc;
            tweenTypeDictionary[EaseType.linear] = Ease.Linear;
            tweenTypeDictionary[EaseType.InBounce] = Ease.InBounce;
            tweenTypeDictionary[EaseType.OutBounce] = Ease.OutBounce;
            tweenTypeDictionary[EaseType.InOutBounce] = Ease.InOutBounce;
            tweenTypeDictionary[EaseType.InBack] = Ease.InBack;
            tweenTypeDictionary[EaseType.OutBack] = Ease.OutBack;
            tweenTypeDictionary[EaseType.InOutBack] = Ease.InOutBack;
            tweenTypeDictionary[EaseType.InElastic] = Ease.InElastic;
            tweenTypeDictionary[EaseType.OutElastic] = Ease.OutElastic;
            tweenTypeDictionary[EaseType.InOutElastic] = Ease.InOutElastic;
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
            Ease easeType = tweenTypeDictionary[data.EaseType];

            float value = 0;
            Tweener t = DOTween.To(
                getter: () => value,
                setter: x => value = x,
                endValue: 1f,
                duration: data.Duration
            )
            .SetDelay(data.Delay)
            .SetEase(easeType)
            .SetLoops(data.PingPongLoop ? -1 : 0, LoopType.Yoyo)
            .OnStart(() => data.OnStart())
            .OnUpdate(() => data.OnUpdate(value))
            .OnComplete(() => data.OnComplete());

            return new Tween(t);
        }

        /// <summary>
        /// Stops the given tween.
        /// </summary>
        /// <param name="tween">The tween data.</param>
        public void Stop(Tween tween)
        {
            if (tween.Data is Tweener)
            {
                Tweener tweener = tween.Data as Tweener;
                tweener.Kill(false);
            }
            else
            {
                Debug.LogWarning("Can't stop tween due to unexpected data.");
            }
        }

#endregion
    }
}

#endif