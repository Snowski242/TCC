using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StarCoin : MonoBehaviour, ICollectable
{

    public AudioClip soundEffect;

    public static event Action OnStarCoinCollected;


    public static event Action OneStarCollected;
    public static event Action TwoStarCollected;
    public static event Action ThreeStarCollected;
    public static event Action FourStarCollected;  
    public static event Action FiveStarCollected;



    Rigidbody2D rb;



[SerializeField] public int Index;
    public StarIndex starIndex;

    bool hasTarget;
    Vector3 targetPosition;
    [SerializeField] float moveSpeed;

    public GameObject coinPopup1000;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    private void Start()
    {

    }

    public void Collect()
    {
        Debug.Log("SuperCoin!!");
        OnStarCoinCollected?.Invoke();
        Instantiate(coinPopup1000, transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(soundEffect, transform.position);

        Destroy(gameObject);

        if(Index == 1)
        {
            OneStarCollected?.Invoke();
        }
        if (Index == 2)
        {
            TwoStarCollected?.Invoke();
        }
        if (Index == 3)
        {
            ThreeStarCollected?.Invoke();
        }
        if (Index == 4)
        {
            FourStarCollected?.Invoke();
        }
        if (Index == 5)
        {
            FiveStarCollected?.Invoke();
        }


    }

    private void FixedUpdate()
    {
        if (hasTarget)
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

