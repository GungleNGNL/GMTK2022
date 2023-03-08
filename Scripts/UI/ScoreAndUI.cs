using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreAndUI : Singleton<ScoreAndUI>
{
    [SerializeField] TextMeshProUGUI scoreText;

    [SerializeField] float score;
    [SerializeField] float scoreScaler = 1;
    public float addScoreTimer = 5f;
    float currentTime= 0;
    bool gameOver;

    private void Awake()
    {
        gameOver = false;
        scoreText = GetComponent<TextMeshProUGUI>();
        score = 0;
    }

    private void Start()
    {
        currentTime = 0;
    }

    private void Update()
    {
        if (gameOver) return;
        scoreText.text = "" + (int)score;
        //scoreScaler += 0.2f * Time.deltaTime

        currentTime += Time.deltaTime;
        if (currentTime >= addScoreTimer)
        {
            currentTime = 0;
            score += 1 * scoreScaler;
        }

    }

    public void EnemyScorer(int type) // 1/2
    {
        score += 100 * type;
    }

    public int GameOver()
    {
        gameOver = true;
        return (int)score;
    }

    //int score = ScoreAndUI.Instance.GameOver();
}
