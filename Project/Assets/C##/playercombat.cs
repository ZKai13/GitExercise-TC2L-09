using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    public Transform attackPoint;
    public float attackRange = 2.5f;
    public LayerMask enemyLayers;
    public int lightAttackDamage = 20;
    public int heavyAttackDamage = 40;
    public float heavyAttackThreshold = 1f;

    public float lightAttackStaminaCost = 10f;
    public float heavyAttackStaminaCost = 25f;

    public Staminasystem staminaSystem;
    private float holdTime = 0f;
    private bool isAttacking = false;
    private bool attackButtonHeld = false;

    void Start()
    {
        staminaSystem = GetComponent<Staminasystem>();
        if (staminaSystem == null)
        {
            Debug.LogError("Staminasystem not found on this GameObject!");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!isAttacking)
            {
                holdTime = 0f;
                isAttacking = true;
                attackButtonHeld = true;
            }
        }

        if (attackButtonHeld)
        {
            holdTime += Time.deltaTime;
            Debug.Log("Hold Time: " + holdTime);
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            if (isAttacking)
            {
                if (holdTime >= heavyAttackThreshold && staminaSystem.ConsumeStamina(heavyAttackStaminaCost))
                {
                    HeavyAttack();
                }
                else if (staminaSystem.ConsumeStamina(lightAttackStaminaCost))
                {
                    LightAttack();
                }

                isAttacking = false;
                attackButtonHeld = false;
                holdTime = 0f;
            }
        }
    }

    void LightAttack()
    {
        animator.SetBool("isHeavyAttack", false);
        animator.SetTrigger("attack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.Takedamage(lightAttackDamage);
            }
        }
    }

    void HeavyAttack()
    {
        animator.SetBool("isHeavyAttack", true);
        animator.SetTrigger("attack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.Takedamage(heavyAttackDamage);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
