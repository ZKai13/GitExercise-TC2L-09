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
    public float heavyAttackThreshold = 1f; // Time in seconds to determine heavy attack

    private float holdTime = 0f;
    private bool isAttacking = false;
    private bool attackButtonHeld = false;

    void Update()
    {
        // Check if the Fire1 button is pressed
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!isAttacking)
            //If the player presses Q and is not already attacking (!isAttacking) 
            // The script starts the attack process by resetting holdTime to 0 and setting isAttacking and attackButtonHeld to true
            {
                // Start the attack timer
                holdTime = 0f;
                isAttacking = true;
                attackButtonHeld = true;
            }
        }

        // If the button is being held, start counting the hold time
        if (attackButtonHeld)
        {
            holdTime += Time.deltaTime;// Increase hold time by the time elapsed since the last frame
            Debug.Log("Hold Time: " + holdTime);// Debugging: log the hold time to the console
        }

        // Check if the Fire1 button was released
        if (Input.GetKeyUp(KeyCode.Q))
        {
            if (isAttacking)
            {
                // Determine attack type based on holdTime
                if (holdTime >= heavyAttackThreshold)
                {
                    Debug.Log("Heavy Attack Triggered"); // Debug log
                    HeavyAttack(); // Heavy attack
                }
                else
                {
                    Debug.Log("Light Attack Triggered");
                    LightAttack(); // light attack
                }

                // Reset attack state
                isAttacking = false;// Mark that the attack is no longer in progress
                attackButtonHeld = false;// Reset the button hold state
            }
        }
    }

    void LightAttack()
    {
        // Trigger light attack animation
        animator.SetBool("isHeavyAttack", false);// Ensure heavy attack animation is not triggered
        animator.SetTrigger("attack");//Trigger the "attack" animation

        // Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // Deal damage to each enemy detected
        foreach (Collider2D enemy in hitEnemies)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>(); // Get the Enemy script from the enemy
            if (enemyScript != null)
            {
                enemyScript.Takedamage(lightAttackDamage);// Apply light attack damage
            }
        }
    }

    void HeavyAttack()
    {
        Debug.Log("Performing Heavy Attack"); // Debug log
        // Trigger heavy attack animation
        animator.SetBool("isHeavyAttack", true); // Indicate that the heavy attack animation should play
        animator.SetTrigger("attack");// Trigger the "attack" animation

        // Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // Damage enemies
        foreach (Collider2D enemy in hitEnemies)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();// Get the Enemy script from the enemy
            if (enemyScript != null)
            {
                enemyScript.Takedamage(heavyAttackDamage);// Apply heavy attack damage
            }
        }
    }

    void OnDrawGizmosSelected()// Unity editor feature to visualize the attack range in the Scene view
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);// Draw a wireframe sphere representing attack range
    }
}
