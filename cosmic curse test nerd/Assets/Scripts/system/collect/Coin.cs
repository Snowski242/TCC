using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Coin : MonoBehaviour, ICollectable
{
    public AudioClip soundEffect;

    public static event Action OnCoinCollected;
    Rigidbody2D rb;

    bool hasTarget;
    Vector3 targetPosition;
    [SerializeField] float moveSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Collect()
    {
        Debug.Log("Coincollect");
        OnCoinCollected?.Invoke();

        AudioSource.PlayClipAtPoint(soundEffect, transform.position);

        Destroy(gameObject);


    }

    private void FixedUpdate()
    {
        if(hasTarget)
        {
            Vector3 targetDirection = (targetPosition - transform.position).normalized;
            rb.velocity = new Vector3(targetDirection.x, targetDirection.y, targetDirection.z) * moveSpeed;
        }
    }

    public void SetTarget(Vector3 position)
    {
        targetPosition = position;
        hasTarget = true;
    }
}
