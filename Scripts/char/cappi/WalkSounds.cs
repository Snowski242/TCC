using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkSounds : MonoBehaviour
{
    public AudioSource footstepsSound;



    public void playclip()

    {
        footstepsSound.Play();
    }

}
