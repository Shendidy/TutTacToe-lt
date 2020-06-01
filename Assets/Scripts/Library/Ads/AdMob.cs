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
    public GameObject collectKeysPanel;
    public GameObject gameItemsPanel;
    public GameObject noKeysPanel;

    private string appID = "ca-app-pub-6785760956809900~7030560283";

    private string bannerID = "ca-app-pub-3940256099942544/6300978111";
    private string interstitialID = "ca-app-pub-3940256099942544/8691691433";
    private string rewardedID = "ca-app-pub-3940256099942544/5224354917";

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
        // Called when an ad request failed to show.
        rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
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

        // Called when an ad request has successfully loaded.
        bannerView.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        bannerView.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is clicked.
        bannerView.OnAdOpening += HandleOnAdOpened;
        // Called when the user returned from the app after an ad click.
        bannerView.OnAdClosed += HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        bannerView.OnAdLeavingApplication += HandleOnAdLeavingApplication;
    }

    public void ShowBannerAd()
    {
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        bannerView.LoadAd(request);
    }

    public void HideBannerAd()
    {
        bannerView.Hide();
    }
    #endregion

    #region Interstitial methods
    public void RequestInterstitial()
    {
        interstitialAd = new InterstitialAd(interstitialID);

        // Called when an ad request has successfully loaded.
        interstitialAd.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        interstitialAd.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is shown.
        interstitialAd.OnAdOpening += HandleOnAdOpened;
        // Called when the ad is closed.
        interstitialAd.OnAdClosed += HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        interstitialAd.OnAdLeavingApplication += HandleOnAdLeavingApplication;

        AdRequest request = new AdRequest.Builder().Build();
        interstitialAd.LoadAd(request);
    }

    public void ShowInterstitialAd()
    {
        int counter = 0;

        while (counter < 3)
        {
            if (interstitialAd.IsLoaded())
            {
                interstitialAd.Show();
                break;
            }
            else
            {
                counter++;
                RequestInterstitial();
                Thread.Sleep(1000);
                ShowInterstitialAd();
            }
        }
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
        int counter = 0;

        while (counter < 3)
        {
            if (rewardedAd.IsLoaded())
            {
                rewardedAd.Show();
                break;
            }
            else
            {
                counter++;
                RequestRewardedAd();
                Thread.Sleep(1000);
                ShowVideoRewadAd();
            }
        }
    }
    #endregion

    #region Events handling methods
    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        //text.text = "Banner loaded!";
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        //text.text = "Banner failed to load!";
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        RequestInterstitial();
        RequestBanner();
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        RequestInterstitial();
        RequestBanner();
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
    }


    // Rewarded Ad events handling methods
    //public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    //{
    //    string type = args.Type;
    //    double amount = args.Amount;

    //    GameManager.keysTotal += (int)amount;
    //    keyCount.text = GameManager.keysTotal.ToString();
    //    gameStatus.text = "You received " + amount.ToString() + " keys!";

    //    GameDataManager.SaveGameData(new GameData((int)amount, DateTime.UtcNow));
    //}


    // Revents handling methods

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        ShowVideoRewadAd();
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

        noKeysPanel.SetActive(false);
        collectKeysPanel.SetActive(true);
        gameItemsPanel.SetActive(false);
    }
    #endregion
}
