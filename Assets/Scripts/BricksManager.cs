using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BricksManager : MonoBehaviour
{
    #region Singleton

    private static BricksManager _instance;

    public static BricksManager Instance => _instance;

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

    public static event Action OnLevelLoaded;

    private int maxRows = 17;
    private int maxCols = 12;
    private GameObject bricksContainer;
    public int CurrentLevel;
    private float initialBrickSpawnPositionX = -1.96f;
    private float initialBrickSpawnPositionY = 3.325f;
    private float shiftAmount = 0.365f;

    public List<BrickHandler> remainingBricks { get; set; }

    public List<int[,]> LevelsData { get; set; }

    public int initialBricksCount { get; set; }

    public Sprite[] sprites;

    public BrickHandler brickPrefab;

    public Color[] BrickColors;

    private void Start()
    {
        this.bricksContainer = new GameObject("BricksContainer");
        this.LevelsData = this.LoadLevelsData();
        this.GenerateBricks();
    }

    public void LoadNextLevel()
    {
        this.CurrentLevel++;

        if (this.CurrentLevel >= this.LevelsData.Count)
        {
            GameManager.Instance.ShowVictoryScreen();
        }
        else
        {
            this.LoadLevel(this.CurrentLevel);
        }
    }

    public void LoadLevel(int level)
    {
        this.CurrentLevel = level;
        this.ClearRemainingBricks();
        this.GenerateBricks();
    }

    private void ClearRemainingBricks()
    {
        foreach (BrickHandler brick in this.remainingBricks.ToList())
        {
            Destroy(brick.gameObject);
        }
    }

    private void GenerateBricks()
    {
        this.remainingBricks = new List<BrickHandler>();
        int[,] currentLevelData = this.LevelsData[this.CurrentLevel];
        float currentSpawnX = initialBrickSpawnPositionX;
        float currentSpawnY = initialBrickSpawnPositionY;
        float zShift = 0;

        for (int row = 0; row < this.maxRows; row++)
        {
            for (int col = 0; col < this.maxCols; col++)
            {
                int brickType = currentLevelData[row, col];

                if (brickType > 0)
                {
                    BrickHandler newBrick = Instantiate(brickPrefab, new Vector3(currentSpawnX, currentSpawnY, 0 - zShift), Quaternion.identity) as BrickHandler;
                    newBrick.Init(bricksContainer.transform, this.sprites[brickType - 1], this.BrickColors[brickType], brickType);

                    this.remainingBricks.Add(newBrick);
                    zShift += 0.0001f;
                }

                currentSpawnX += shiftAmount;
                if (col + 1 == this.maxCols)
                {
                    currentSpawnX = initialBrickSpawnPositionX;
                }
            }

            currentSpawnY -= shiftAmount;
        }

        this.initialBricksCount = this.remainingBricks.Count;
        OnLevelLoaded?.Invoke();
    }

    private List<int[,]> LoadLevelsData()
    {
        TextAsset text = Resources.Load("levels") as TextAsset;

        string[] rows = text.text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

        List<int[,]> levelsData = new List<int[,]>();
        int[,] currentLevel = new int[maxRows, maxCols];
        int currentRow = 0;

        for (int row = 0; row < rows.Length; row++)
        {
            string line = rows[row];

            if (line.IndexOf("--") == -1)
            {
                string[] bricks = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int col = 0; col < bricks.Length; col++)
                {
                    currentLevel[currentRow, col] = int.Parse(bricks[col]);
                }

                currentRow++;
            }
            else
            {
                currentRow = 0;
                levelsData.Add(currentLevel);
                currentLevel = new int[maxRows, maxCols];
            }
        }

        return levelsData;
    }
}
