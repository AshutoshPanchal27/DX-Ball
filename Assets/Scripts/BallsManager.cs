using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BallsManager : MonoBehaviour
{

    #region Singleton

    private static BallsManager _instance;

    public static BallsManager Instance => _instance;

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

    [SerializeField]
    private BallHandler ballPrefab;

    private BallHandler initialBall;

    private Rigidbody2D initialBallRigidBody;

    public float initialBallSpeed = 250f;

    public List<BallHandler> Balls { get; set; }

    private void Start()
    {
        InitBall();
    }

    private void Update()
    {
        if (!GameManager.Instance.isGameStarted)
        {
            Vector3 paddlePosition = PaddleHandler.Instance.gameObject.transform.position;
            Vector3 ballPosition = new Vector3(paddlePosition.x, paddlePosition.y + .27f, 0);
            initialBall.transform.position = ballPosition;
        }

        if (Input.GetMouseButtonDown(0) && !GameManager.Instance.isGameStarted)
        {
            initialBallRigidBody.isKinematic = false;
            initialBallRigidBody.AddForce(new Vector2(0, initialBallSpeed));
            GameManager.Instance.isGameStarted = true;
        }
    }

    internal void ResetBalls()
    {
        foreach (var ball in this.Balls.ToList())
        {
            Destroy(ball.gameObject);
        }

        InitBall();
    }

    private void InitBall()
    {
        Vector3 paddlePosition = PaddleHandler.Instance.gameObject.transform.position;
        Vector3 startingPosition = new Vector3(paddlePosition.x, paddlePosition.y + .27f, 0);
        initialBall = Instantiate(ballPrefab, startingPosition, Quaternion.identity);
        initialBallRigidBody = initialBall.GetComponent<Rigidbody2D>();

        this.Balls = new List<BallHandler>
        {
            initialBall
        };

    }
}
