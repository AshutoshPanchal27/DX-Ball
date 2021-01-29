using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI TargetText;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI RemainingLivesText;

    private string scoreString;

    public int Score { get; set; }

    private void Awake()
    {
        BrickHandler.OnBrickDestruction += OnBrickDestruction;
        BricksManager.OnLevelLoaded += OnLevelLoaded;
    }

    private void Start()
    {
        GameManager.Instance.OnLifeLost += OnLifeLost;
        OnLifeLost(GameManager.Instance.AvailableLives);
    }

    private void OnLifeLost(int remainingLives)
    {
        RemainingLivesText.text = $"LIVES: {remainingLives}";
        this.Score = 0;
        scoreString = this.Score.ToString().PadLeft(5, '0');
        ScoreText.text = $"SCORE:{Environment.NewLine}{scoreString}";
    }

    private void OnLevelLoaded()
    {
        UpdateRemainingBricksText();
        UpdateScoreText(0);
    }

    private void UpdateScoreText(int increment)
    {
        this.Score += increment;
        scoreString = this.Score.ToString().PadLeft(5, '0');
        ScoreText.text = $"SCORE:{Environment.NewLine}{scoreString}";
    }

    private void OnBrickDestruction(BrickHandler obj)
    {
        UpdateRemainingBricksText();
        UpdateScoreText(10);
    }

    private void UpdateRemainingBricksText()
    {
        TargetText.text = $"TARGET:{Environment.NewLine}{BricksManager.Instance.remainingBricks.Count} / {BricksManager.Instance.initialBricksCount}";
    }

    private void OnDisable()
    {
        BrickHandler.OnBrickDestruction -= OnBrickDestruction;
        BricksManager.OnLevelLoaded -= OnLevelLoaded;
        GameManager.Instance.OnLifeLost -= OnLifeLost;
    }
}
