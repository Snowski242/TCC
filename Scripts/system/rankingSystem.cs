using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static GameManager;

public class rankingSystem : MonoBehaviour
{
    private string rankName;
    public static int thingsDone;
    public Animator animator;
    public static int scoreThreshold = 250;


    private Dictionary<ranking, string> RankLetters = new Dictionary<ranking, string>();
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Rank = GameManager.ranking.D;



        RankLetters.Add(ranking.D, "D");
        RankLetters.Add(ranking.C, "C");
        RankLetters.Add(ranking.B, "B");
        RankLetters.Add(ranking.A, "A");
        RankLetters.Add(ranking.S, "S");

        rankName = RankLetters[ranking.D];
        animator.SetTrigger("D");
        thingsDone = thingsDone - 4;

        if (thingsDone < 0)
        {
            thingsDone = 0;
        }

    }

    private void OnEnable()
    {
        GameManager.rankUp += RankNumUp;
    }

    // Update is called once per frame
    void Update()
    {
        if (thingsDone == 0)
        {
            rankName = RankLetters[ranking.D];
            animator.SetTrigger("D");
        }
        if (thingsDone == 1)
        {
            rankName = RankLetters[ranking.C];
            animator.SetTrigger("C");
        }
        if(thingsDone == 2)
        {
            rankName = RankLetters[ranking.B];
            animator.SetTrigger("B");
        }
        if (thingsDone == 3)
        {
            rankName = RankLetters[ranking.A];
            animator.SetTrigger("A");
        }
        if (thingsDone == 4)
        {
            rankName = RankLetters[ranking.S];
            animator.SetTrigger("S");
        }
    }

    public static void RankNumUp()
    {
        thingsDone = thingsDone + 1;
    }
    public static void RankNumDwn()
    {
        thingsDone = thingsDone - 1;
    }
}
