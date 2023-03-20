using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class health : MonoBehaviour
{
    public int maxHealth = 3;
    public static int currenthp;
    // Start is called before the first frame update
    void Start()
    {
        currenthp = maxHealth;
    }
    public void Damaged(int amount)
    {
        currenthp -= amount;

        if(currenthp <= 0)
        {
            //dead lol
            Reset();
        }
    }

   private void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

}
