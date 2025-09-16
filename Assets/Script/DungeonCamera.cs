using DG.Tweening;
using UnityEngine;

public class DungeonCamera : MonoBehaviour
{
    const float SMOOTHSPEED = 0.125f;

    Transform _player;
    PlayerStat _playerStat;
    Vector3 _offset = new (0, 0, -10);

    [SerializeField] private float _shakeDuration = 0.5f;
    [SerializeField] private float _shakeStrength = 0.5f;

    private float _shakeTimer = 0f;
    private float _currentShakeStrength;


    public void Awake()
    {
        _player = Player.Instance.transform;
        _playerStat = _player.GetComponent<PlayerStat>();
    }

    void OnEnable()
    {
        _playerStat.OnTakeDamage += Shake;
    }

    void OnDisable()
    {
        _playerStat.OnTakeDamage -= Shake;
    }
    private void LateUpdate()
    {
        // 1. 기본 카메라 위치 계산 (항상 실행)
        Vector3 desiredPosition = _player.position + _offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, SMOOTHSPEED);

        // 2. 흔들림 효과가 활성화되어 있는지 확인
        if (_shakeTimer > 0)
        {
            // 남은 시간에 비례하여 흔들림 강도를 점차 줄임 (Fade Out 효과)
            float currentStrength = _currentShakeStrength * (_shakeTimer / _shakeDuration);

            // 2D 게임에 적합한 Random.insideUnitCircle 사용 (Z축은 흔들리지 않음)
            Vector3 shakeOffset = (Vector3)Random.insideUnitCircle * currentStrength;

            // 기본 위치에 흔들림 오프셋을 더함
            smoothedPosition += shakeOffset;

            // 타이머 감소
            _shakeTimer -= Time.deltaTime;
        }

        // 3. 최종 계산된 위치를 한 번만 적용
        transform.position = smoothedPosition;
    }

    void Shake(float strengthMultiplier = 1f)
    {
        _shakeTimer = _shakeDuration;
        _currentShakeStrength = _shakeStrength * strengthMultiplier;
    }
}
