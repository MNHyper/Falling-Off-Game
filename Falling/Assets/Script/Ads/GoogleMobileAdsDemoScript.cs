using GoogleMobileAds;
using GoogleMobileAds.Api;
using GoogleMobileAds.Api.AdManager;
using System.Collections.Generic;
using UnityEngine;

public class GoogleMobileAdsDemoScript : MonoBehaviour
{
#if UNITY_ANDROID
    private const string AD_UNIT_ID = "ca-app-pub-2413838992303643/7685190880";
    private const string AD_UNIT_LOSE_ID = "ca-app-pub-2413838992303643/9897055719";
#elif UNITY_IPHONE
    private const string AD_UNIT_ID = "ca-app-pub-2413838992303643/7685190880";
    private const string AD_UNIT_LOSE_ID = "ca-app-pub-2413838992303643/9897055719";
#else
    private const string AD_UNIT_ID = "ca-app-pub-2413838992303643/7685190880";
    private const string AD_UNIT_LOSE_ID = "ca-app-pub-2413838992303643/9897055719";
#endif

    private BannerView bannerView;
    private InterstitialAd interstitialAd;

    public void Start()
    {
        // Initialize Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            CreateBannerView();
            LoadBannerView();
            LoadInterstitialAd();
        });
    }

    private void CreateBannerView()
    {
        // [START create_banner_view]
        // Create a 320x50 banner at top of the screen.
        bannerView = new BannerView(AD_UNIT_ID, AdSize.Banner, AdPosition.Bottom);
        // [END create_banner_view]
    }

    private void LoadBannerView()
    {
        // [START load_banner_view]
        // Send a request to load an ad into the banner view.
        bannerView.LoadAd(new AdRequest());
        // [END load_banner_view]
    }

    private void LoadInterstitialAd()
    {
        // [START load_interstitial_ad]
        // Clean up the old ad before loading a new one.
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        Debug.Log("Loading the interstitial ad.");

        // Create our request used to load the ad.
        var adRequest = new AdRequest();

        // Send the request to load the ad.
        InterstitialAd.Load(AD_UNIT_LOSE_ID, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                // If error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Interstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                interstitialAd = ad;
                RegisterEventHandlers(interstitialAd);
            });
        // [END load_interstitial_ad]
    }

    private void RegisterEventHandlers(InterstitialAd interstitialAd)
    {
        // Raised when the ad is estimated to have earned money.
        interstitialAd.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(System.String.Format("Interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        interstitialAd.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        interstitialAd.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        interstitialAd.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad full screen content closed.");
            // Reload the ad so that we can show another as soon as possible.
            LoadInterstitialAd();
        };
        // Raised when the ad failed to open full screen content.
        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);
            // Reload the ad so that we can show another as soon as possible.
            LoadInterstitialAd();
        };
    }

    /// <summary>
    /// Call this function when the player loses to show a fullscreen interstitial ad
    /// </summary>
    public void ShowLoseAd()
    {
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            Debug.Log("Showing interstitial ad.");
            interstitialAd.Show();
        }
        else
        {
            Debug.LogError("Interstitial ad is not ready yet.");
            // Optionally reload the ad
            LoadInterstitialAd();
        }
    }

    private void DestroyBannerView()
    {
        // [START destroy_banner_view]
        if (bannerView != null)
        {
            // Always destroy the banner view when no longer needed.
            bannerView.Destroy();
            bannerView = null;
        }
        // [END destroy_banner_view]
    }

    private void DestroyInterstitialAd()
    {
        if (interstitialAd != null)
        {
            Debug.Log("Destroying interstitial ad.");
            interstitialAd.Destroy();
            interstitialAd = null;
        }
    }

    public void OnDestroy()
    {
        DestroyBannerView();
        DestroyInterstitialAd();
    }
}