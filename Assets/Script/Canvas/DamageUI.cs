using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class DamageUI : UIBase
{
    const string PLAYER_DAMAGE_SKIN = "Text (TMP) - PlayerDamageSkin";
    const string ENEMY_DAMAGE_SKIN = "Text (TMP) - EnemyDamageSkin";

    readonly Vector2 _offset = new(0, 150);

    PoolObject _playerDamageSkinPrefab;
    PoolObject _enemyDamageSkinPrefab;


    protected override void Awake()
    {
        base.Awake();
        PoolInit();
    }

    public void GetDamageOfPlayer(float damage, Vector3 worldPosition)
    {
        OnDamageSkin(damage, worldPosition, PLAYER_DAMAGE_SKIN);
    }

    public void GetDamageOfEnemy(float damage, Vector3 worldPosition)
    {
        OnDamageSkin(damage, worldPosition, ENEMY_DAMAGE_SKIN);
    }

    void OnDamageSkin(float damage, Vector3 worldPosition, string poolObjectKey)
    {
        PoolObject poolObject = ObjectPool.Instance.Get(poolObjectKey)
                                           .Get();

        poolObject.transform.SetParent(transform, false);
        poolObject.GetComponent<TMP_Text>()
                  .text = damage.ToString();

        RectTransform poolRect = poolObject.GetComponent<RectTransform>();
        CanvasGroup poolCanvasGroup = poolObject.GetComponent<CanvasGroup>();

        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(_canvas.worldCamera, worldPosition);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform,
                                                                screenPoint,
                                                                _canvas.worldCamera,
                                                                out Vector2 anchoredPosition);

        poolRect.anchoredPosition = anchoredPosition;

        Sequence seq = DOTween.Sequence();

        Vector2 endPos = anchoredPosition + _offset;

        seq.Append(
            poolRect.DOAnchorPos(endPos, 0.6f).SetEase(Ease.OutCubic)
        );
        seq.Insert(0.3f,
            poolCanvasGroup.DOFade(0.3f, 0.3f)
        );
        seq.OnComplete(() =>
        {
            poolCanvasGroup.alpha = 1f;
            poolObject.Release();
        });
    }

    void PoolInit()
    {
        _playerDamageSkinPrefab = Resources.Load<PoolObject>(PLAYER_DAMAGE_SKIN);
        ObjectPool.Instance.CreatePool(PLAYER_DAMAGE_SKIN, _playerDamageSkinPrefab, 2);

        _enemyDamageSkinPrefab = Resources.Load<PoolObject>(ENEMY_DAMAGE_SKIN);
        ObjectPool.Instance.CreatePool(ENEMY_DAMAGE_SKIN, _enemyDamageSkinPrefab, 2);
    }
}
