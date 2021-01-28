using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class BrickHandler : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public int Hitpoints = 1;
    public ParticleSystem DestroyEffect;

    public static event Action<BrickHandler> OnBrickDestruction;

    private void Start()
    {
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        BallHandler ball = collision.gameObject.GetComponent<BallHandler>();
        ApplyCollisionLogic(ball);
    }

    private void ApplyCollisionLogic(BallHandler ball)
    {
        this.Hitpoints--;

        if (this.Hitpoints <= 0)
        {
            OnBrickDestruction?.Invoke(this);
            SpawnDestroyEffect();
            Destroy(this.gameObject);
        }
        else
        {

        }
    }

    private void SpawnDestroyEffect()
    {
        Vector3 brickPos = gameObject.transform.position;
        Vector3 spawnPos = new Vector3(brickPos.x, brickPos.y, brickPos.z - 0.2f);
        GameObject effect = Instantiate(DestroyEffect.gameObject, spawnPos, Quaternion.identity);

        MainModule mainModule = effect.GetComponent<ParticleSystem>().main;
        mainModule.startColor = this.spriteRenderer.color;
        Destroy(effect, DestroyEffect.main.startLifetime.constant);
    }
}
