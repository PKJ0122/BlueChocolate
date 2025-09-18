using GoogleMobileAds.Api;
using System.Threading.Tasks;

public class AdManager : SingletonMonoBase<AdManager>
{
    RewardedAd _rewardedAd;
    BannerView _bannerView;

    string _rewardAdId;
    string _bannerViewId;


    protected override void Awake()
    {
        base.Awake();
#if UNITY_EDITOR
        _rewardAdId = "adUnitId";
        _bannerViewId = "adUnitId";
#else
                _rewardAdId = "ca-app-pub-5639813524802030/8692399306";
                _bannerViewId = "ca-app-pub-5639813524802030/4724661029";
#endif

        MobileAds.Initialize(initStatus => { });

        RequestBanner();
    }

    public async Task<AdShowResult> LoadAndShowAd()
    {
        TaskCompletionSource<bool> loadTcs = new();
        AdRequest adRequest = new();

        RewardedAd.Load(_rewardAdId, adRequest, (ad, error) =>
        {
            if (error != null || ad == null)
            {
                _rewardedAd = null;
                loadTcs.SetResult(false);
                return;
            }

            _rewardedAd = ad;
            loadTcs.SetResult(true);
        });

        bool isLoadSuccess = await loadTcs.Task;

        if (!isLoadSuccess)
        {
            return AdShowResult.Failed;
        }

        TaskCompletionSource<AdShowResult> showTcs = new();

        RegisterAdEventHandlers(_rewardedAd, showTcs);

        _rewardedAd.Show(reward =>
        {
            showTcs.TrySetResult(AdShowResult.Success);
        });

        AdShowResult result = await showTcs.Task;

        _rewardedAd.Destroy();
        _rewardedAd = null;

        return result;
    }

    private void RegisterAdEventHandlers(RewardedAd ad, TaskCompletionSource<AdShowResult> showTcs)
    {
        ad.OnAdFullScreenContentFailed += (error) =>
        {
            showTcs.TrySetResult(AdShowResult.Failed);
        };

        ad.OnAdFullScreenContentClosed += () =>
        {
            showTcs.TrySetResult(AdShowResult.Canceled);
        };
    }

    void RequestBanner()
    {
        _bannerView?.Destroy();
        AdSize adSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
        _bannerView = new BannerView(_bannerViewId, adSize, AdPosition.Bottom);

        AdRequest request = new();
        _bannerView.LoadAd(request);
    }
}
