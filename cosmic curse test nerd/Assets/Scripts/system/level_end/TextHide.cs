using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextHide : MonoBehaviour
{
    private void OnEnable()
    {
        LevelEnd.OnLevelEnd += TextDisappear;
    }
    private void OnDisable()
    {
        LevelEnd.OnLevelEnd -= TextDisappear;
    }
    // Start is called before the first frame update
    void Start()
    {
        LevelEnd.OnLevelEnd += TextDisappear;
        this.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void TextDisappear()
    {
        this.gameObject.SetActive(false);
    }
}
