using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class coinText : MonoBehaviour
{
    public static int score;
    public bool passedScore;
    public bool gotShard;
    public static TextMeshProUGUI CoinText;
    public static Animator oloi;
    public StarIndex starIndex;
    public StarCoin starCoin;


   // Start is called before the first frame update
    void Start()
    {
        CoinText = GetComponent<TextMeshProUGUI>();
        oloi = GetComponent<Animator>();
        passedScore = false;
        gotShard = false;
        score = 0;
        rankingSystem.thingsDone = 0;
    }

    private void Update()
    {
        CoinText.text = $"{score}";

        if (score >= rankingSystem.scoreThreshold && !passedScore)
        {
            passedScore = true;
            rankingSystem.RankNumUp();
        }

        if (score < rankingSystem.scoreThreshold && passedScore)
        {
            passedScore = false;
            rankingSystem.RankNumDwn();
        }

        if (score < 0) score = 0;
    }
    private void OnEnable()
    {
        Coin.OnCoinCollected += IncrementCoinCount;
        StarCoin.OnStarCoinCollected += StarCoinIncrement;
        ModerateEnemyDead.ModerateEnemDead += BigIncrementCount;
        MaliceShard.OnShardCollect += ShardGet;
    }

    private void OnDisable()
    {
        Coin.OnCoinCollected -= IncrementCoinCount;
        StarCoin.OnStarCoinCollected -= StarCoinIncrement;
        ModerateEnemyDead.ModerateEnemDead -= BigIncrementCount;
        MaliceShard.OnShardCollect -= ShardGet;
    }

    public void IncrementCoinCount()
    {
        score = score + 10;




    }


    public void StarCoinIncrement() 
    { 
        score = score + 1000;

        //if (starIndex.StarCoinNumber == 1 && starCoin.Index == 1)
        //{
        //    GameManager.PP_Coin1 = true;
        //}
        //else if (starIndex.StarCoinNumber == 2 && starCoin.Index == 2)
        //{
        //    GameManager.PP_Coin2 = true;
        //}
        //else if (starIndex.StarCoinNumber == 3 && starCoin.Index == 3)
        //{
        //    GameManager.PP_Coin3 = true;
        //}
        //else if (starIndex.StarCoinNumber == 4 && starCoin.Index == 4)
        //{
        //    GameManager.PP_Coin4 = true;
        //}
        //else if (starIndex.StarCoinNumber == 5 && starCoin.Index == 5)
        //{
        //    GameManager.PP_Coin5 = true;
        //}

    }
    public void BigIncrementCount()
    {
        score = score + 40;



        CoinText.text = $"{score}";

        if (score >= rankingSystem.scoreThreshold && !passedScore)
        {
            passedScore = true;
            rankingSystem.RankNumUp();
        }

        if (score < rankingSystem.scoreThreshold && passedScore)
        {
            passedScore = false;
            rankingSystem.RankNumDwn();
        }
    }

    public void ShardGet()
    {
        gotShard = true;
        rankingSystem.RankNumUp();
    }

    public static void GettingHurt()
    {
        oloi.SetTrigger("hurt");



    }

}
