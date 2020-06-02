using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using GoogleMobileAds.Api;
using System.Threading;

public class AdMob : MonoBehaviour
{
    public static AdMob instance;
    public Text keyCount;
    public Text gameStatus;
    public Text collectText;
    public GameObject collectKeysPanel;
    public GameObject gameItemsPanel;
    public GameObject noKeysPanel;

    private readonly string appID = Constants.adMobAppID;

    private readonly string bannerID = Constants.adMobBannerID;
    private readonly string interstitialID = Constants.adMobInterstitialID;
    private readonly string rewardedID = Constants.adMobRewardedID;

    private BannerView bannerView;
    private InterstitialAd interstitialAd;
    private RewardedAd rewardedAd;

    // Start is called before the first frame update
    void Start()
    {
        MobileAds.Initialize(appID);
        // Banner Ads
        RequestBanner();
        ShowBannerAd();

        // Interstitial Ads
        RequestInterstitial();

        // Rewarded Video Ads
        rewardedAd = new RewardedAd(rewardedID);
        RequestRewardedAd();

        // Called when the user should be rewarded for interacting with the ad.
        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        rewardedAd.OnAdClosed += HandleRewardedAdClosed;
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    #region Banner methods
    public void RequestBanner()
    {
        // Create a 320x50 banner at the top of the screen.
        bannerView = new BannerView(bannerID, AdSize.Banner, AdPosition.Bottom);

        // Called when the user returned from the app after an ad click.
        bannerView.OnAdClosed += HandleOnBannerAdClosed;
    }

    public void ShowBannerAd()
    {
        AdRequest request = new AdRequest.Builder().Build();
        bannerView.LoadAd(request);
    }
    #endregion

    #region Interstitial methods
    public void RequestInterstitial()
    {
        interstitialAd = new InterstitialAd(interstitialID);

        AdRequest request = new AdRequest.Builder().Build();
        interstitialAd.LoadAd(request);

        // Called when the ad is closed.
        interstitialAd.OnAdClosed += HandleOnInterstitialAdClosed;
    }

    public void ShowInterstitialAd()
    {
        if (interstitialAd.IsLoaded())
            interstitialAd.Show();
    }
    #endregion

    #region Rewarded Ads methods
    public void RequestRewardedAd()
    {
        AdRequest request = new AdRequest.Builder().Build();
        rewardedAd.LoadAd(request);
    }

    public void ShowVideoRewadAd()
    {
        if (rewardedAd.IsLoaded())
            rewardedAd.Show();
    }
    #endregion

    #region Events handling methods

    public void HandleOnBannerAdClosed(object sender, EventArgs args)
    {
        RequestBanner();
    }

    public void HandleOnInterstitialAdClosed(object sender, EventArgs args)
    {
        RequestInterstitial();
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        RequestRewardedAd();
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        GameManager.rewardedKeys = (int)amount;

        collectText.text = "Congratulations. you've earned " + amount.ToString() + " keys!\n\nCollect them now and start playing...";

        collectKeysPanel.SetActive(true);
        noKeysPanel.SetActive(false);
        gameItemsPanel.SetActive(false);
    }
    #endregion
}
