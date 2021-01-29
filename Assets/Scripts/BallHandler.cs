using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallHandler : MonoBehaviour
{
    public static event Action<BallHandler> OnBallDeath;
    public void Die()
    {
        OnBallDeath?.Invoke(this);
        Destroy(gameObject, 1); // 1 is the One Second Delay
    }
}
