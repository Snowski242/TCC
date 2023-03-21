using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class enemybehavior : MonoBehaviour
{

    [SerializeField] float health, maxHealth = 3f;

    [SerializeField] protected float moveSpeed = 5f;
    [SerializeField] protected float redSpeed = 3f;
    protected Rigidbody2D rb;
    protected Transform target;
    protected Vector2 moveDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        target = GameObject.Find("Square").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = (target.position - transform.position);
        moveDirection = direction;
    }

    protected virtual void OnTriggerEnter2D()
    {
        if (target)
        {
            rb.velocity = new Vector2(moveDirection.x, moveDirection.y) * moveSpeed;
        }
    }
}
