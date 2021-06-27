﻿using GoogleMobileAds.Api;
using KansusGames.KansusAds.Core;
using System;
using UnityEngine;

namespace KansusGames.KansusAds.Adapter.AdMob
{
    /// <summary>
    /// Represents an interstitial advertisement by the AdMob network.
    /// </summary>
    public class AdMobInterstitialAd : IInterstitialAd
    {
        #region Fields

        private readonly string placementId;
        private readonly Func<AdRequest.Builder> adRequestBuilderFactory;
        private InterstitialAd interstitialAd;

        #endregion

        #region Initialization

        /// <summary>
        /// Creates an instance of this class.
        /// </summary>
        /// <param name="placementId">The placement id for this ad.</param>
        /// <param name="adRequestBuilderFactory">A factory to create ad request builders.</param>
        public AdMobInterstitialAd(string placementId, Func<AdRequest.Builder> adRequestBuilderFactory)
        {
            this.placementId = placementId;
            this.adRequestBuilderFactory = adRequestBuilderFactory;
        }

        #endregion

        #region IInterstitialAd

        public bool IsLoaded()
        {
            return interstitialAd != null && interstitialAd.IsLoaded();
        }

        public void Load(Action onLoad = null, Action<string> onFail = null)
        {
            interstitialAd = new InterstitialAd(placementId);

            interstitialAd.OnAdLoaded += (sender, args) =>
            {
                Debug.Log("AdMob interstitial loaded successfully");
                onLoad?.Invoke();
            };

            interstitialAd.OnAdFailedToLoad += (sender, args) =>
            {
                Debug.LogWarning("Failed to load AdMob interstitial: " + args.LoadAdError.GetMessage());
                onFail?.Invoke(args.LoadAdError.GetMessage());
            };

            AdRequest request = adRequestBuilderFactory().Build();

            interstitialAd.LoadAd(request);
        }

        public void Show(Action onClose = null, Action<string> onFail = null)
        {
            if (interstitialAd == null || !interstitialAd.IsLoaded())
            {
                Debug.LogWarning("AdMob interstitial ad not loaded");
                return;
            }

            interstitialAd.OnAdFailedToLoad += (sender, args) =>
            {
                Debug.Log("Failed to show AdMob interstitial ad");
                onFail?.Invoke(args.LoadAdError.GetMessage());
            };

            interstitialAd.OnAdClosed += (sender, args) =>
            {
                Debug.Log("AdMob interstitial ad closed");
                onClose?.Invoke();
            };

            interstitialAd.Show();
        }

        #endregion
    }
}