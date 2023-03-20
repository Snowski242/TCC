using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MaliceShard : MonoBehaviour, ICollectable
{

    public static event Action OnShardCollect;
    public AudioClip soundEffect;
    public void Collect()
    {
        OnShardCollect?.Invoke();

        AudioSource.PlayClipAtPoint(soundEffect, transform.position);

        Destroy(gameObject);
    }

}
