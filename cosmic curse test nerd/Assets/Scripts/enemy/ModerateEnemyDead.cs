using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ModerateEnemyDead : MonoBehaviour
{
    public int enemyHP;

    public static event Action ModerateEnemDead;


    void Start()
    {
        enemyHP = 20;
    }



    public void takeDamage()
    {
        enemyHP = enemyHP - AttackDamage.BaseDamage;

        if (enemyHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("dead weeper");
        ModerateEnemDead?.Invoke();

        Destroy(gameObject);
    }

  
}
