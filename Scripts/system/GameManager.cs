using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static ranking Rank;

    public static event Action rankUp;



    void Awake()
    {
        instance = this;
    }

    public static void RankIncrease()
    {
        rankUp?.Invoke();
    }

    public enum ranking
    {
        S,
        A,
        B,
        C,
        D
    }
}
