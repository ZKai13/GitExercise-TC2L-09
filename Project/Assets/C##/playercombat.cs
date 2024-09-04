using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    // Public variables
    public Animator animator; // Reference to the Animator component to control animations
    public Transform attackPoint; // The point from which the player's attacks are calculated
    public float attackRange = 2.5f; // The range of the player's attack
    public LayerMask enemyLayers; // The layers that represent enemies
    public int lightAttackDamage = 20; // Damage dealt by a light attack
    public int heavyAttackDamage = 40; // Damage dealt by a heavy attack
    public float heavyAttackThreshold = 1f; // The threshold of hold time to differentiate between a light and heavy attack

    public float lightAttackStaminaCost = 10f; // Stamina cost for a light attack
    public float heavyAttackStaminaCost = 25f; // Stamina cost for a heavy attack

    public Staminasystem staminaSystem; // Reference to the player's stamina system

    // Private variables
    private float holdTime = 0f; // The duration the attack button has been held
    private bool isAttacking = false; // Tracks if the player is currently attacking
    private bool attackButtonHeld = false; // Tracks if the attack button is being held down

    // Start is called before the first frame update
    void Start()
    {
        // Get the Staminasystem component attached to the player
        staminaSystem = GetComponent<Staminasystem>();

        // If the stamina system is not found, log an error
        if (staminaSystem == null)
        {
            Debug.LogError("Staminasystem not found on this GameObject!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the attack button (Q) is pressed
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // If the player is not currently attacking, start tracking the hold time
            if (!isAttacking)
            {
                holdTime = 0f;
                isAttacking = true;
                attackButtonHeld = true;
            }
        }

        // If the attack button is being held down, increase the hold time
        if (attackButtonHeld)
        {
            holdTime += Time.deltaTime;
            Debug.Log("Hold Time: " + holdTime); // Debug log to track hold time
        }

        // Check if the attack button (Q) is released
        if (Input.GetKeyUp(KeyCode.Q))
        {
            // If the player is currently attacking
            if (isAttacking)
            {
                // If the hold time exceeds the heavy attack threshold and there is enough stamina, perform a heavy attack
                if (holdTime >= heavyAttackThreshold && staminaSystem.ConsumeStamina(heavyAttackStaminaCost))
                {
                    HeavyAttack();
                }
                // Otherwise, if there is enough stamina, perform a light attack
                else if (staminaSystem.ConsumeStamina(lightAttackStaminaCost))
                {
                    LightAttack();
                }

                // Reset attack states after the attack
                isAttacking = false;
                attackButtonHeld = false;
                holdTime = 0f;
            }
        }
    }

    // Method to perform a light attack
    void LightAttack()
    {
        // Set the animation for a light attack
        animator.SetBool("isHeavyAttack", false);
        animator.SetTrigger("attack");

        // Detect enemies in range of the attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // Apply damage to each enemy hit
        foreach (Collider2D enemy in hitEnemies)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.Takedamage(lightAttackDamage);
            }
        }
    }

    // Method to perform a heavy attack
    void HeavyAttack()
    {
        // Set the animation for a heavy attack
        animator.SetBool("isHeavyAttack", true);
        animator.SetTrigger("attack");

        // Detect enemies in range of the attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // Apply damage to each enemy hit
        foreach (Collider2D enemy in hitEnemies)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.Takedamage(heavyAttackDamage);
            }
        }
    }

    // Visualize the attack range in the Unity Editor
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        // Draw a red wireframe sphere to represent the attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
