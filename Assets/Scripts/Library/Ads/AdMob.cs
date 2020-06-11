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
    public GameObject getKeysPanel;
    public Text loadingAddText;

    private readonly string appID = Constants._AdMobAppIDIos;

    private readonly string bannerID = Constants._AdMobBannerIDIos;
    private readonly string interstitialID = Constants._AdMobInterstitialIDIos;
    private readonly string rewardedID = Constants._AdMobRewardedIDIos;

    //private BannerView bannerView;
    private InterstitialAd interstitialAd;
    private RewardedAd rewardedAd;

    //private int interstitialAdCounter = 0;
    //private int rewardedAdCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("started AdMob Start() method!");

        MobileAds.Initialize(appID);

        this.rewardedAd = new RewardedAd(rewardedID);
        this.interstitialAd = new InterstitialAd(interstitialID);

        //// Banner Ads
        //RequestBanner();
        //ShowBannerAd();
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    #region Banner methods
    //public void RequestBanner()
    //{
    //    // Create a 320x50 banner at the top of the screen.
    //    this.bannerView = new BannerView(bannerID, AdSize.Banner, AdPosition.Bottom);

    //    // Called when the user returned from the app after an ad click.
    //    this.bannerView.OnAdClosed += this.HandleOnBannerAdClosed;
    //}

    //public void ShowBannerAd()
    //{
    //    AdRequest request = new AdRequest.Builder().Build();
    //    this.bannerView.LoadAd(request);
    //}

    //public void HideBannerAd()
    //{
    //    this.bannerView.Destroy();
    //}
    #endregion

    #region Interstitial methods
    public void RequestInterstitial()
    {

        //Debug.Log("Before RequestInterstitial method, interstitial loaded = " + this.interstitialAd.IsLoaded());
        AdRequest request = new AdRequest.Builder().Build();
        if (!this.interstitialAd.IsLoaded())
            this.interstitialAd.LoadAd(request);
        //Debug.Log("After RequestInterstitial method, interstitial loaded = " + this.interstitialAd.IsLoaded());

        // Called when the ad is closed.
        this.interstitialAd.OnAdClosed += this.HandleOnInterstitialAdClosed;
    }

    public void ShowInterstitialAd()
    {
        //Debug.Log("Before ShowInterstitialAd method, Interstitial loaded = " + this.interstitialAd.IsLoaded());
        if (this.interstitialAd.IsLoaded())
        {
            this.interstitialAd.Show();
            GameManager.interstitialAdCounter = 0;
        }
    }
    #endregion

    #region Rewarded Ads methods
    public void RequestRewardedAd()
    {
        //Debug.Log("Before RequestRewardedAd method, RewardedAd loaded = " + this.rewardedAd.IsLoaded());
        if (!this.rewardedAd.IsLoaded())
        {
            AdRequest request = new AdRequest.Builder().Build();
            this.rewardedAd.LoadAd(request);
        }
        //Debug.Log("After RequestRewardedAd method, RewardedAd loaded = " + this.rewardedAd.IsLoaded());

        // Called when the user should be rewarded for interacting with the ad.
        this.rewardedAd.OnUserEarnedReward += this.HandleUserEarnedReward;
        // Called when the ad is closed.
        this.rewardedAd.OnAdClosed += this.HandleRewardedAdClosed;
    }

    public void ShowVideoRewadAd()
    {
        //Debug.Log("Rewarded is loaded = " + rewardedAd.IsLoaded());
        if (this.rewardedAd.IsLoaded())
        {
            GameManager.interstitialAdCounter = 0;
            this.rewardedAd.Show();
        }
        else
        {
            loadingAddText.text = "Your Ad will start shortly!";

            RewardedAd testRewardedAd = new RewardedAd(Constants._AdMobRewardedTestID);
            AdRequest requestTest = new AdRequest.Builder().Build();
            testRewardedAd.LoadAd(requestTest);

            Thread.Sleep(2000);
            if(testRewardedAd.IsLoaded())
                testRewardedAd.Show();
            else
            {
                Thread.Sleep(2000);
                if (testRewardedAd.IsLoaded())
                    testRewardedAd.Show();
            }
        }
    }
    #endregion

    #region Events handling methods

    //public void HandleOnBannerAdClosed(object sender, EventArgs args)
    //{
    //}

    public void HandleOnInterstitialAdClosed(object sender, EventArgs args)
    {
        //Debug.Log("On Interstitial Ad closed...");
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        //Debug.Log("On Rewarded Ad closed...");
        if (getKeysPanel != null) getKeysPanel.SetActive(false);
        this.RequestRewardedAd();
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        GameManager.rewardedKeys = (int)amount;

        //Debug.Log("Type is: " + type + " and amount = " + amount.ToString());
        //Debug.Log($"Type is: {type} and amount = {amount.ToString()}");

        collectText.text = $"Congratulations. you've earned {amount.ToString()} {type}!\n\nCollect them now and start playing...";

        collectKeysPanel.SetActive(true);
        noKeysPanel.SetActive(false);
        gameItemsPanel.SetActive(false);
    }
    #endregion
}
