using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringBig : MonoBehaviour
{

    public float bounceStrength = 80f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * bounceStrength, ForceMode2D.Impulse);
            CineMachineShake.Instance.ShakeCamera(2f, .1f);
        }
    }
}
