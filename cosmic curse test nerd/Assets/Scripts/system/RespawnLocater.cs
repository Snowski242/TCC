using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnLocater : MonoBehaviour
{
    public Transform objectToFollow;
    public Vector3 offset;
    void Update()
    {
        if (Player.isGrounded && Player.bodys.velocity.y > 0) 
        {
            transform.position = objectToFollow.position + offset;
            transform.rotation = objectToFollow.rotation;

            if (objectToFollow != null) 
            {
                return;
            }
        }

    }

}
