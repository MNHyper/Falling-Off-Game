using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banner : MonoBehaviour
{
#if UNITY_ANDROID
    private const string AD_UNIT_ID = "ca-app-pub-4600640691459471/9556554997";
#elif UNITY_IPHONE
        private const string AD_UNIT_ID = "ca-app-pub-3940256099942544/2934735716";
#else
        private const string AD_UNIT_ID = "unused";
#endif
    private BannerView bannerView;
    void Start()
    {
        AdSize adSize = new AdSize(250, 250);
        bannerView = new BannerView(AD_UNIT_ID, adSize, AdPosition.Bottom);

        bannerView.LoadAd(new AdRequest());

    }

}
