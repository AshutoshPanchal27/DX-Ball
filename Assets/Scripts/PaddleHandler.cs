using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleHandler : MonoBehaviour
{
    [SerializeField]
    Camera mainCamera;

    Vector3 paddleInitialPos;

    float paddleClampLeft = 135f;
    float paddleClampRight = 410f;

    private void Start()
    {
        paddleInitialPos = this.transform.position;
        paddleClampLeft = -1.443f;
        paddleClampRight = 1.509f;
    }

    private void Update()
    {
        PaddleMovement();
    }

    private void PaddleMovement()
    {
        float mousePositionX = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, 0, 0)).x;
        this.transform.position = new Vector3(mousePositionX, paddleInitialPos.y, paddleInitialPos.z);
    }
}
