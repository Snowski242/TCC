using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class flyingEnemy : MonoBehaviour
{

    public float speed;
    public bool chase;
    private GameObject player;
    public Transform startingPoint;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");  
    }

    void Update()
    {
        if (player == null)
            return;
        if (chase == true)
            Chase();
        else
            ReturnStartPosition();

        Flip();
    }

    void Chase()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    private void ReturnStartPosition()
    {
        transform.position = Vector2.MoveTowards(transform.position, startingPoint.position, speed * Time.deltaTime);
    }

    void Flip()
    {
        if (transform.position.x > player.transform.position.x)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else
            transform.rotation = Quaternion.Euler(0, 180, 0);
    }
}
