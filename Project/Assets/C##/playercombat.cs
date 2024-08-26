using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class playercombat : MonoBehaviour
{
    public Animator animator;
    public Transform attackpoint;
    public float attackRange = 2.5f;
    public LayerMask enemyLayers;
    public int attackdamage = 40;
    void Update()
    {
       if(Input.GetKeyDown(KeyCode.Q)) 
       {
        Attack();
       }
    }
    void Attack()
    {
        //play an attack animation
        animator.SetTrigger("attack");

        //Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackpoint.position, attackRange, enemyLayers);
            // Check if any enemies were hit
        if (hitEnemies.Length > 0)
        {
            Debug.Log("Enemies detected: " + hitEnemies.Length);
        }
        else
        {
            Debug.Log("No enemies hit");
        }

        //Damage them
        foreach(Collider2D enemy in hitEnemies)
        {       
            Debug.Log("Enemy position: " + enemy.transform.position);
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                Debug.Log("Hit enemy: " + enemy.name);
                enemyScript.Takedamage(attackdamage);            
            }
            else
            {
            Debug.Log("Enemy does not have the Enemy script: " + enemy.name);            
            }
        }
   
    }
    void OnDrawGizmosSelected()
    {
        if (attackpoint == null)
        return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackpoint.position, attackRange);
    }
}
