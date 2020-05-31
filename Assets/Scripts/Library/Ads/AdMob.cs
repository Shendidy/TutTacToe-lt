using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using GoogleMobileAds.Api;

public class AdMob : MonoBehaviour
{
    public static AdMob instance;
    //public Text text;
    //public Text counter;

    private string appID = "ca-app-pub-6785760956809900~7030560283";

    private string bannerID = "ca-app-pub-3940256099942544/6300978111";
    private string interstitialID = "ca-app-pub-3940256099942544/8691691433";
    private string rewardedID = "ca-app-pub-3940256099942544/5224354917";

    private BannerView bannerView;
    private InterstitialAd interstitialAd;
    private RewardBasedVideoAd rewardBasedVideoAd;

    // Start is called before the first frame update
    void Start()
    {
        MobileAds.Initialize(appID);
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public void RequestBanner()
    {
        // Create a 320x50 banner at the top of the screen.
        this.bannerView = new BannerView(bannerID, AdSize.Banner, AdPosition.Bottom);

        // Called when an ad request has successfully loaded.
        this.bannerView.OnAdLoaded += this.HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.bannerView.OnAdFailedToLoad += this.HandleOnAdFailedToLoad;
        // Called when an ad is clicked.
        this.bannerView.OnAdOpening += this.HandleOnAdOpened;
        // Called when the user returned from the app after an ad click.
        this.bannerView.OnAdClosed += this.HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        this.bannerView.OnAdLeavingApplication += this.HandleOnAdLeavingApplication;
    }

    public void ShowBannerAd()
    {
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        this.bannerView.LoadAd(request);
    }

    public void HideBannerAd()
    {
        this.bannerView.Hide();
    }

    public void RequestInterstitial()
    {
        this.interstitialAd = new InterstitialAd(interstitialID);

        // Called when an ad request has successfully loaded.
        this.interstitialAd.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.interstitialAd.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is shown.
        this.interstitialAd.OnAdOpening += HandleOnAdOpened;
        // Called when the ad is closed.
        this.interstitialAd.OnAdClosed += HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        this.interstitialAd.OnAdLeavingApplication += HandleOnAdLeavingApplication;

        AdRequest request = new AdRequest.Builder().Build();
        this.interstitialAd.LoadAd(request);
    }

    public void ShowInterstitialAd()
    {
        if (this.interstitialAd.IsLoaded())
        {
            this.interstitialAd.Show();
        }
    }

    public void RequestRewardBasedVideo()
    {
        this.rewardBasedVideoAd = RewardBasedVideoAd.Instance;

        AdRequest request = new AdRequest.Builder().Build();
        this.rewardBasedVideoAd.LoadAd(request, rewardedID);


        // Called when the user should be rewarded for watching a video.
        rewardBasedVideoAd.OnAdRewarded += HandleRewardBasedVideoRewarded;
    }

    public void ShowVideoRewadAd()
    {
        if (rewardBasedVideoAd.IsLoaded())
        {
            rewardBasedVideoAd.Show();
        }
    }


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
        MonoBehaviour.print("HandleAdOpened event received");
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeavingApplication event received");
    }

    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;

        GameManager.keysTotal += (int)amount;

        //counter.text = (int.Parse(counter.text) + amount).ToString();
        //text.text = "User rewarded with: " + amount.ToString() + " " + type;
    }
}
