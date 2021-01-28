using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleHandler : MonoBehaviour
{
    #region Singleton

    private static PaddleHandler _instance;

    public static PaddleHandler Instance => _instance;

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
    Camera mainCamera;

    [SerializeField]
    SpriteRenderer spriteRenderer;

    Vector3 paddleInitialPos;

    float defaultLeftClamp = 135f;
    float defaultRightClamp = 410f;

    float defaultPaddleWidth = 200f;

    private void Start()
    {
        paddleInitialPos = this.transform.position;
    }

    private void Update()
    {
        PaddleMovement();
    }

    private void PaddleMovement()
    {
        float paddleShift = (defaultPaddleWidth - ((defaultPaddleWidth / 2) * this.spriteRenderer.size.x)) / 2;
        float paddleClampLeft = defaultLeftClamp - paddleShift;
        float paddleClampRight = defaultRightClamp - paddleShift;
        float clampedposition = Mathf.Clamp(Input.mousePosition.x, paddleClampLeft, paddleClampRight);
        float mousePositionX = mainCamera.ScreenToWorldPoint(new Vector3(clampedposition, 0, 0)).x;
        this.transform.position = new Vector3(mousePositionX, paddleInitialPos.y, paddleInitialPos.z);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            Rigidbody2D ballRigidBody = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector3 hitPoint = collision.contacts[0].point;
            Vector3 paddleCenter = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y);

            ballRigidBody.velocity = Vector2.zero;

            float difference = paddleCenter.x - hitPoint.x;

            if (hitPoint.x < paddleCenter.x)
            {
                ballRigidBody.AddForce(new Vector2(-(Mathf.Abs(difference * 200)), BallsManager.Instance.initialBallSpeed));
            }
            else
            {
                ballRigidBody.AddForce(new Vector2((Mathf.Abs(difference * 200)), BallsManager.Instance.initialBallSpeed));
            }
        }
    }
}
