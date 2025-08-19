using GoogleMobileAds.Api;
using System.Threading.Tasks;
using UnityEngine;

public class AdManager : Singleton<AdManager>
{
    RewardedAd _rewardedAd;
    public RewardedAd RewardedAd => _rewardedAd;

    string _rewardAdId;


    protected override void Awake()
    {
        base.Awake();
#if UNITY_EDITOR
        _rewardAdId = "adUnitId";
#else
                _rewardAdId = "ca-app-pub-5639813524802030/8692399306";
#endif

        MobileAds.Initialize(initStatus => { });
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
}
