using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleHandler : MonoBehaviour
{
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
}
