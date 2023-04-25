using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkSounds : MonoBehaviour
{
    public AudioSource footstepsSound;
    public AudioSource footrunstepsSound;



    public void playclip()

    {
        footstepsSound.Play();
    }

    public void playrunclip()

    {
        footrunstepsSound.Play();
    }
}
