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
    public TextMeshProUGUI CoinText;

   // Start is called before the first frame update
    void Start()
    {
        passedScore = false;
        gotShard = false;
        score = 0;
        rankingSystem.thingsDone = 0;
    }

    private void OnEnable()
    {
        Coin.OnCoinCollected += IncrementCoinCount;
        ModerateEnemyDead.ModerateEnemDead += BigIncrementCount;
        MaliceShard.OnShardCollect += ShardGet;
    }

    private void OnDisable()
    {
        Coin.OnCoinCollected -= IncrementCoinCount;
        ModerateEnemyDead.ModerateEnemDead -= BigIncrementCount;
        MaliceShard.OnShardCollect -= ShardGet;
    }

    public void IncrementCoinCount()
    {
        score = score + 10;



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

}
