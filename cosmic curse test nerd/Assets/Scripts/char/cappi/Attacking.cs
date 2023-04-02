using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacking : MonoBehaviour

{
    public Playercontroller controller;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

 

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow) && Playercontroller.currentlyDashing && Playercontroller.isAttacking)
        {
            if (!pauseMenu.isPaused)
            {

                Attack();
            }
        }

        if (Input.GetKey(KeyCode.LeftArrow) && Playercontroller.currentlyDashing && Playercontroller.isAttacking)
        {
            if (!pauseMenu.isPaused)
            {

                Attack();
            }
        }



    }

    void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {
            Debug.Log("We hit" + enemy.name);
            enemy.GetComponent<ModerateEnemyDead>().takeDamage();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
