using UnityEngine;
using UnityEngine.UI;
using System;
using GoogleMobileAds.Api;

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

    private string appID;
    private string bannerID;
    private string interstitialID;
    private string rewardedID;

    private BannerView bannerView;
    private InterstitialAd interstitialAd;
    private RewardedAd rewardedAd;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.mobileSystem = "ios"; // "ios", "android", or "test"

        switch (GameManager.mobileSystem)
        {
            case "ios":
                appID = Constants._AdMobAppIDIos;
                bannerID = Constants._AdMobBannerIDIos;
                interstitialID = Constants._AdMobInterstitialIDIos;
                rewardedID = Constants._AdMobRewardedIDIos;
                break;
            case "android":
                appID = Constants._AdMobAppIDAndroid;
                bannerID = Constants._AdMobBannerIDAndroid;
                interstitialID = Constants._AdMobInterstitialIDAndroid;
                rewardedID = Constants._AdMobRewardedIDAndroid;
                break;
            case "test":
                appID = Constants._AdMobAppIDAndroid;
                bannerID = Constants._AdMobBannerIDTest;
                interstitialID = Constants._AdMobInterstitialIDTest;
                rewardedID = Constants._AdMobRewardedIDTest;
                break;
            default:
                break;
        }

        MobileAds.Initialize(appID);

        this.rewardedAd = new RewardedAd(rewardedID);
        this.interstitialAd = new InterstitialAd(interstitialID);

        // Banner Ads
        RequestBanner();
        ShowBannerAd();
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
        this.bannerView = new BannerView(bannerID, AdSize.Banner, AdPosition.Bottom);

        // Called when the user returned from the app after an ad click.
        //this.bannerView.OnAdClosed += this.HandleOnBannerAdClosed;
    }

    public void ShowBannerAd()
    {
        AdRequest request = new AdRequest.Builder().Build();
        this.bannerView.LoadAd(request);
    }

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
            //AudioListener.pause = true;
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
            //AudioListener.pause = true;
            GameManager.interstitialAdCounter = 0;
            this.rewardedAd.Show();
        }
        else
        {
            HandleRewardedAdCouldntLoad();
        }
    }
    #endregion

    #region Events handling methods

    //public void HandleOnBannerAdClosed(object sender, EventArgs args)
    //{
    //}

    public void HandleOnInterstitialAdClosed(object sender, EventArgs args)
    {
        //AudioListener.pause = false;
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        //AudioListener.pause = false;
        if (getKeysPanel != null) getKeysPanel.SetActive(false);
        this.RequestRewardedAd();
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        //AudioListener.pause = false;
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

    public void HandleRewardedAdCouldntLoad()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            collectText.text = "Error. Check internet connection!\n\nInternet connection is required to load the short ad and earn your additional keys";
        }
        else
        {
            //AudioListener.pause = false;
            GameManager.rewardedKeys = Constants._RefillOnRewardedAd;

            collectText.text = $"Congratulations. you've earned {Constants._RefillOnRewardedAd.ToString()} keys!\n\nCollect them now and start playing...";

            collectKeysPanel.SetActive(true);
            getKeysPanel.SetActive(false);
            noKeysPanel.SetActive(false);
            gameItemsPanel.SetActive(false);
        }
    }
    #endregion
}
