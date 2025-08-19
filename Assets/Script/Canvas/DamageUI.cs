using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class DamageUI : UIBase
{
    PoolObject _damagePrefab;
    IObjectPool<PoolObject> _damagePool;


    protected override void Awake()
    {
        base.Awake();
        Enemy.Instance.TakeDamaged += OnDamage;
        poolInit();
    }

    public void OnDamage(float damage)
    {
        PoolObject poolObject = _damagePool.Get();
        poolObject.GetComponent<TMP_Text>()
                  .text = damage.ToString();

        RectTransform poolRect = poolObject.GetComponent<RectTransform>();
        CanvasGroup poolCanvasGroup = poolObject.GetComponent<CanvasGroup>();

        Vector3 targetPos = Enemy.Instance.transform.position;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(targetPos);

        Vector2 localPos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvas.transform as RectTransform,
            screenPos,
            _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main,
            out localPos);

        poolRect.localPosition = localPos;

        Sequence seq = DOTween.Sequence();

        Vector2 endPos = localPos + new Vector2(0, 150);

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

    void poolInit()
    {
        _damagePrefab = Resources.Load<PoolObject>("Text (TMP) - DamageSkin");

        _damagePool = new ObjectPool<PoolObject>(
            createFunc: () =>
            {
                PoolObject damageObject = Instantiate(_damagePrefab);
                damageObject.transform.SetParent(transform,false);
                damageObject.SetPool(_damagePool);
                return damageObject;
            },
            actionOnGet: (damage) =>
            {
                damage.gameObject.SetActive(true);
            },
            actionOnRelease: (damage) =>
            {
                damage.gameObject.SetActive(false);
            },
            actionOnDestroy: (damage) =>
            {
                Destroy(damage.gameObject);
            },
            collectionCheck: false,
            defaultCapacity: 5,
            maxSize: 10
            );
    }
}
