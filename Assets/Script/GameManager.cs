using System;
using System.Collections;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public const float GAME_TIME = 120f;

    float _playTime;
    public float playTime
    {
        get => _playTime;
        set
        {
            _playTime = value;
            OnPlayTimeChange?.Invoke(value);
        }
    }

    int _score;

    public int Score
    {
        get => _score;
        set
        {
            _score = value;
            OnScoreChange?.Invoke(Score);

            if (value > BestScore)
            {
                BestScore = value;
            }
        }
    }

    public int BestScore
    {
        get
        {
            if (!PlayerPrefs.HasKey("BastScore"))
            {
                PlayerPrefs.SetInt("BastScore", 0);
            }

            return PlayerPrefs.GetInt("BastScore");
        }
        set
        {
            PlayerPrefs.SetInt("BastScore", value);
            OnBestScoreChange?.Invoke(value);
        }
    }

    public Action OnGameStart;
    public Action OnGameEnd;
    public Action<int> OnScoreChange;
    public Action<int> OnBestScoreChange;
    public Action<float> OnPlayTimeChange;


    protected override void Awake()
    {
        base.Awake();
        OnGameStart += () =>
        {
            Score = 0;
        };
    }

    public void GameStart()
    {
        OnGameStart?.Invoke();
        StartCoroutine(C_Game());
    }

    public void GameEnd()
    {
        OnGameEnd?.Invoke();
    }

    IEnumerator C_Game()
    {
        playTime = GAME_TIME;

        while (playTime > 0)
        {
            playTime -= Time.deltaTime;
            yield return null;
        }

        GameEnd();
    }
}
