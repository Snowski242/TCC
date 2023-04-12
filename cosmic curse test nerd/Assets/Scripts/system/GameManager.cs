using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static ranking Rank;

    public static event Action rankUp;

    //Plum Plains Coins
    public static bool PP_Coin1;
    public static bool PP_Coin2;
    public static bool PP_Coin3;
    public static bool PP_Coin4;
    public static bool PP_Coin5;

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
