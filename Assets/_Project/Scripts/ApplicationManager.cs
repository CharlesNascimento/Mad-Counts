using GoogleMobileAds.Api.Mediation.AdColony;
using GoogleMobileAds.Api.Mediation.Chartboost;
using GoogleMobileAds.Api.Mediation.UnityAds;
using KansusGames.KansusAds.Adapter.AdMob;
using KansusGames.KansusAds.Manager;
using System.Collections.Generic;
using UnityEngine;

namespace KansusGames.MadCounts
{
    public class ApplicationManager : MonoBehaviour
    {
        public static ApplicationManager Instance { get; private set; }

        [SerializeField]
        private AdManagerSettings adManagerSettings;

        public AdManager AdManager { get; set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);

            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            InitAdManager();
        }

        private void Start()
        {
            Application.targetFrameRate = 60;
        }

        private void InitAdManager()
        {
            AdManager = new AdManager(new AdMobAdNetwork(), adManagerSettings);

            var collectUserData = false;

            UnityAds.SetGDPRConsentMetaData(collectUserData);
            AdColonyAppOptions.SetGDPRRequired(true);
            AdColonyAppOptions.SetGDPRConsentString(collectUserData ? "1" : "0");
            Chartboost.AddDataUseConsent(collectUserData ? CBCCPADataUseConsent.OptInSale : CBCCPADataUseConsent.OptOutSale);

            AdManager.Initialize(collectUserData, new AdMobExtras(true, new List<string>() { "02BD293B625083918FF7F7A0FD59E626" }));
        }
    }
}
