using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton

    private static GameManager _instance;

    public static GameManager Instance => _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    #endregion

    public event Action<int> OnLifeLost;

    public GameObject gameOverScreen;
    public GameObject gameCompletedScreen;

    public int AvailableLives = 3;
    public int Lives { get; set; }

    public bool isGameStarted { get; set; }

    private void Start()
    {
        this.Lives = this.AvailableLives;
        Screen.SetResolution(540, 960, false);
        BallHandler.OnBallDeath += OnBallDeath;
        BrickHandler.OnBrickDestruction += OnBrickDestruction;
    }

    private void OnBrickDestruction(BrickHandler obj)
    {
        if (BricksManager.Instance.remainingBricks.Count <= 0)
        {
            BallsManager.Instance.ResetBalls();
            GameManager.Instance.isGameStarted = false;
            BricksManager.Instance.LoadNextLevel();
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnBallDeath(BallHandler ball)
    {
        if (BallsManager.Instance.Balls.Count <= 0)
        {
            this.Lives--;

            if (this.Lives < 1)
            {
                gameOverScreen.SetActive(true);
            }
            else
            {
                OnLifeLost?.Invoke(this.Lives);
                BallsManager.Instance.ResetBalls();
                isGameStarted = false;
                BricksManager.Instance.LoadLevel(BricksManager.Instance.CurrentLevel);
            }
        }
    }

    public void ShowVictoryScreen()
    {
        gameCompletedScreen.SetActive(true);
    }

    private void OnDisable()
    {
        BallHandler.OnBallDeath -= OnBallDeath;
    }
}
