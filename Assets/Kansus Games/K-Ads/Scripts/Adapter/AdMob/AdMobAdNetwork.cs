using GoogleMobileAds.Api;
using KansusGames.KansusAds.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using BannerPosition = KansusGames.KansusAds.Core.BannerPosition;

namespace KansusGames.KansusAds.Adapter.AdMob
{
    /// <summary>
    /// AdMob implementation of an ad network.
    /// </summary>
    public class AdMobAdNetwork : IAdNetwork
    {
        #region Fields

        private bool servePersonalizedAds;
        
        private AdMobExtras adMobConfig;

        #endregion

        #region IAdPlatform

        public void Initialize(string appId, bool servePersonalizedAds = true, AdNetworkExtras extras = null,
            Action<bool> onInit = null)
        {
            adMobConfig = extras as AdMobExtras;
            var isChildDirected = adMobConfig?.ChildDirected ?? false;

            Debug.Log("ChildDirected: " + isChildDirected);

            if (isChildDirected)
            {
                var requestConfiguration = new RequestConfiguration.Builder()
                    .SetTagForUnderAgeOfConsent(TagForUnderAgeOfConsent.True)
                    .SetTagForChildDirectedTreatment(TagForChildDirectedTreatment.True)
                    .SetMaxAdContentRating(MaxAdContentRating.G)
                    .SetTestDeviceIds(adMobConfig?.TestDevices ?? new List<string>())
                    .build();

                MobileAds.SetRequestConfiguration(requestConfiguration);
            }

            MobileAds.Initialize(initStatus =>
            {
                onInit?.Invoke(true);
                Debug.Log("MobileAds initialized");
            });

            this.servePersonalizedAds = servePersonalizedAds;
        }

        public IBannerAd CreateBanner(string placementId, BannerPosition adPosition)
        {
            return new AdMobBannerAd(placementId, adPosition, CreateRequestBuilder);
        }

        public IInterstitialAd CreateInterstitial(string placementId)
        {
            return new AdMobInterstitialAd(placementId, CreateRequestBuilder);
        }

        public IRewardedVideoAd CreateRewardedVideoAd(string placementId)
        {
            return new AdMobRewardedVideoAd(placementId, CreateRequestBuilder);
        }

        #endregion

        #region Private methods

        private AdRequest.Builder CreateRequestBuilder()
        {
            AdRequest.Builder requestBuilder = new AdRequest.Builder();

            if (!servePersonalizedAds)
            {
                requestBuilder.AddExtra("npa", "1");
            }

            Debug.Log("ServePersonalizedAds: " + servePersonalizedAds);

            return requestBuilder;
        }

        #endregion
    }
}
