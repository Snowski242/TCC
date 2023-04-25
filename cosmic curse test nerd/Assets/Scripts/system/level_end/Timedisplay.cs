using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timedisplay : MonoBehaviour
{

    public TextMeshProUGUI textMeshPro;

    // Start is called before the first frame update
    void Start()
    {
        textMeshPro.text = "Total time: " + Timer.timeValue;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
