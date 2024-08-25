using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class playercombat : MonoBehaviour
{
    public Animator animator;
    public Transform attackpoint;
    public float attackrange = 0.5f;
    public LayerMask enemyLayers;
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
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackpoint.position, attackrange, enemyLayers);

        //Damage them
        foreach(Collider2D enemy in hitEnemies)
        {
            Debug.Log("We hit!" + enemy.name);
        }
   
    }
    void OnDrawGizmosSelected()
    {
        if (attackpoint == null)
        return;
        Gizmos.DrawWireSphere(attackpoint.position, attackrange);
    }
}
