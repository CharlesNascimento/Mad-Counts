using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdMobsAds : MonoBehaviour
{

    private BannerView bannerView;

    private string appID = "";
    private string bannerID = "";
    private string intID = "";

    void Awake()
    {
        //inicializa o AdMob
        MobileAds.Initialize(appID);
    }

    public void ShowBannerAds()
    {
        RequestBanner();
    }

    public void ShowIntAds()
    {
        RequestInt();
    }

    private void RequestBanner()
    {
        //instancia um novo banner de tamanho normal, embaixo centralizado
        bannerView = new BannerView(bannerID, AdSize.Banner, AdPosition.Bottom);
        //requisita ao google para criar uma nova janela de banner
        AdRequest request = new AdRequest.Builder().Build();

        bannerView.LoadAd(request);
    }

    private void RequestInt()
    {
        //criar um novo anuncia de tela cheia
        InterstitialAd intAd = new InterstitialAd(intID);
        //requisita ao google para criar uma nova janela de tela cheia de anuncio
        AdRequest request = new AdRequest.Builder().Build();

        intAd.LoadAd(request);
    }

}