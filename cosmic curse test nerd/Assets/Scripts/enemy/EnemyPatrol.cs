using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using System;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Point Settings")]
    public GameObject pointA;
    public GameObject pointB;


    private Rigidbody2D rb;
    private Transform trans;
    public Player player;

    [Header("Speed Settings")]
    public float rapidSpeed;



    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    [SerializeField] private GameObject[] waypoints;
    private int currentWaypointIndex = 0;



    private void Update()
    {
        if (Vector2.Distance(waypoints[currentWaypointIndex].transform.position, transform.position) < .1f)
        {
            currentWaypointIndex++;
            Flip();
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position, Time.deltaTime * rapidSpeed);
    }

    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player" && player.canBeHit)
        {
            player.KBCounter = player.KBTotaltime;



            if(collision.transform.position.x <= transform.position.x)
            {
                player.KnockFromRight = true;
            }
            if (collision.transform.position.x >= transform.position.x)
            {
                player.KnockFromRight = false;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Restrict")
        {
            Flip();
            trans = pointB.transform;
        }
        if (collision.gameObject.tag == "RestrictB")
        {
            Flip();
            trans = pointA.transform;
        }
    }
    private void Slowdown()
    {
        rapidSpeed = 6;
    }
    private void FastAgain()
    {
        rapidSpeed = 22;
    }
}
