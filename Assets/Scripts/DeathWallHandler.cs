using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathWallHandler : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ball")
        {
            BallHandler ball = collision.GetComponent<BallHandler>();
            BallsManager.Instance.Balls.Remove(ball);
            ball.Die();
        }
    }
}
