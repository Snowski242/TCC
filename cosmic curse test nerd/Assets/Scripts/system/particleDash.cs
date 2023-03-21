using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleDash : Playercontroller
{

    private SpriteRenderer spRend;

    // Start is called before the first frame update
    void Start()
    {
        spRend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentlyDashing)
        {
            spRend.enabled ^= true;
        }

    }
}
