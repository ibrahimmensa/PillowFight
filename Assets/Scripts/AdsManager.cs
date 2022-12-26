using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using DanielLochner.Assets.SimpleScrollSnap;

public enum RewardType
{
    DOUBLE_REWARD,
    FREE_COINS,
    TIMER_MODE_EXTENSION,
    PILLOW_PURCHASING,
    PILLOW_UPGRADING
}

public class AdsManager : Singleton<AdsManager>,IUnityAdsInitializationListener, IUnityAdsLoadListener,IUnityAdsShowListener
{
    public string androidGameId;
    public string iosGameId;

    public string androidInterstitiatlGameId;
    public string iosInterstitiatlGameId;

    public string androidRewardedGameId;
    public string iosRewardedGameId;

    public string androidBannerGameId;
    public string iosBannerGameId;

    string gameId;
    string interstitialAdId;
    string rewardedAdId;
    string bannerAdId;
    public bool testMode = true;

    public RewardType rewardType;

    SimpleScrollSnap pillowScrollView;

    private void Awake()
    {
        InitializeAds();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeAds()
    {
        gameId = (Application.platform == RuntimePlatform.IPhonePlayer) ? iosGameId : androidGameId;
        Advertisement.Initialize(gameId, testMode, this);
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete");
        LoadBannerAd();
        LoadInterstitialAd();
        LoadRewardedAd();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log("Unity Ads initialization failed: " +error.ToString()+message);
    }

    public void LoadInterstitialAd()
    {
        interstitialAdId = (Application.platform == RuntimePlatform.IPhonePlayer) ? iosInterstitiatlGameId : androidInterstitiatlGameId;
        Advertisement.Load(interstitialAdId, this);
    }

    public void ShowInterstitialAd()
    {
        Advertisement.Show(interstitialAdId, this);
    }

    public void ShowInterstitialAdWithDelay()
    {
        Invoke("ShowInterstitialAd", 1f);
    }

    public void LoadRewardedAd()
    {
        rewardedAdId = (Application.platform == RuntimePlatform.IPhonePlayer) ? iosRewardedGameId : androidRewardedGameId;
        Advertisement.Load(rewardedAdId, this);
    }

    public void ShowRewardedAd(string type)
    {
        if (type == "DoubleReward")
            rewardType = RewardType.DOUBLE_REWARD;
        else if (type == "FreeCoins")
            rewardType = RewardType.FREE_COINS;
        else if (type == "ExtraTime")
            rewardType = RewardType.TIMER_MODE_EXTENSION;
        Advertisement.Show(rewardedAdId, this);
    }

    public void ShowRewardedAdToBuyPillow(SimpleScrollSnap scrollView)
    {
        rewardType = RewardType.PILLOW_PURCHASING;
        pillowScrollView = scrollView;
        Advertisement.Show(rewardedAdId, this);
    }

    public void ShowRewardedAdToUpgradePillow(SimpleScrollSnap scrollView)
    {
        rewardType = RewardType.PILLOW_UPGRADING;
        pillowScrollView = scrollView;
        Advertisement.Show(rewardedAdId, this);
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("OnUnityAdsAdLoaded"); 
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log("Unity Ad not loaded: " + error.ToString() + message);
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log("OnUnityAdsShowFailure");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log("OnUnityAdsShowStart");
        Advertisement.Banner.Hide();
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log("OnUnityAdsShowClick");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        Debug.Log("OnUnityAdsShowComplete:"+showCompletionState);

        if (placementId == interstitialAdId)
            LoadInterstitialAd();
        else if (placementId == rewardedAdId)
            LoadRewardedAd();

        if (placementId == rewardedAdId && showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {
            Debug.Log("Reward"); 
            if (rewardType == RewardType.DOUBLE_REWARD)
            {
                int totalCoins = PlayerPrefs.GetInt("Coins", 0) + 500;
                PlayerPrefs.SetInt("Coins", totalCoins);
                UIManager.Instance.UpdateCoinsStatus(PlayerPrefs.GetInt("Coins"));
            }
            else if (rewardType == RewardType.FREE_COINS)
            {
                int totalCoins = PlayerPrefs.GetInt("Coins", 0) + 500;
                PlayerPrefs.SetInt("Coins", totalCoins);
                UIManager.Instance.UpdateCoinsStatus(PlayerPrefs.GetInt("Coins"));
            }
            else if (rewardType == RewardType.TIMER_MODE_EXTENSION)
            {
                GameManager.Instance.TimerModeExtension();
            }
            else if (rewardType == RewardType.PILLOW_PURCHASING)
            {
                UIManager.Instance.OnClickBuyWithAds(pillowScrollView);
            }
            else if (rewardType == RewardType.PILLOW_UPGRADING)
            {
                UIManager.Instance.OnClickUpgradeWithAds(pillowScrollView);
            }
        }
        Advertisement.Banner.Show(bannerAdId);
    }
#region BANNER_ADS

    public void LoadBannerAd()
    {
        Advertisement.Banner.SetPosition(BannerPosition.TOP_CENTER);
        bannerAdId = (Application.platform == RuntimePlatform.IPhonePlayer) ? iosBannerGameId : androidBannerGameId;
        Advertisement.Banner.Load(bannerAdId,
            new BannerLoadOptions
            {
                loadCallback = OnBannerLoaded,
                errorCallback = OnBannerLoadError
            }
            );
    }

    void OnBannerLoaded()
    {
        Advertisement.Banner.Show(bannerAdId);
    }

    void OnBannerLoadError(string message)
    {
        Debug.Log(message);
    }

    #endregion
}
