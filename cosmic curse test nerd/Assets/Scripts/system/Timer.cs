using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public Animator animator;

    public static float timeValue;

    private void OnEnable()
    {
        Player.TimeIncrease += IncreaseTime;
    }

    private void OnDisable()
    {
        Player.TimeIncrease -= IncreaseTime;
    }

    private void Update()
    {
        if(timeValue > -1)
        {
            timeValue += Time.deltaTime;
        }

        DisplayTime(timeValue);
    }


    void DisplayTime(float timeToDisplay)
    {
        if(timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        float milliseconds = timeToDisplay % 1 * 1000;

        timeText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);


    }

    private void IncreaseTime()
    {
        animator.SetTrigger("hurt");
        timeValue = timeValue + 3;
    }
}
