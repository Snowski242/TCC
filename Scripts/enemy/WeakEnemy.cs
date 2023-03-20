using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakEnemy : MonoBehaviour
{

    [SerializeField] float deathTimer = 0.2f;

    bool  isSquashed;
    bool movingLeft;

    float flipTimer = 0;
    float speed = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        

 


    }

    // Update is called once per frame
    void Update()
    {
        if (!isSquashed)
        {
            Move();
        }

        if (isSquashed)
        {
            Destroy(gameObject, deathTimer);
        }

        if (flipTimer <= Time.realtimeSinceStartup)
        {
            transform.Rotate(new Vector3(0, 1, 0), 180);
            flipTimer = Time.realtimeSinceStartup + 0.25f;
        }
    }

    void Move()
    {
        if (movingLeft)
        {
            transform.position += Vector3.left * Time.deltaTime * speed;
        }
        else
        {
            transform.position += Vector3.right * Time.deltaTime * speed;
        }
    }

    public bool GetIsSquashed()
    {
        return isSquashed;
    }

    public void SetIsSquashed(bool squashed)
    {
        isSquashed = squashed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        movingLeft = !movingLeft;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SetIsSquashed(true);
    }
}
