using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    public GameObject winText;

    public static event Action OnLevelEnd;
    public static bool finishedLevel;
    // Start is called before the first frame update
    void Start()
    {
        finishedLevel = false;
}

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            finishedLevel = true;
            winText.SetActive(true);
            OnLevelEnd?.Invoke();
        }
    }
}
