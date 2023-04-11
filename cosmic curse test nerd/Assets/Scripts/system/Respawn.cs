using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Respawn : MonoBehaviour
{
    public Transform RespawnLocation;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Respawn")
        {

            transform.position = RespawnLocation.position;
            coinText.GettingHurt();

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Respawn")
            coinText.score = coinText.score - 50;
    }


}
