using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    //The maximum health the enemy can have. 
    private int currentHealth;
    //Tracks the enemy's current health
    private Transform target;
    
    public float enemyMoveSpeed = 2f;
    public float followDistance = 10f;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private bool isDead = false;
    private bool isAttacking = false;

    public Health playerHealth; // Reference to the Player's Health script
    public float cooldownTime = 0.75f; // Cooldown time after an attack
    private float nextAttackTime = 0f; // The time at which the enemy can attack again
    void Start()
    {
        currentHealth = maxHealth;
        // Initialize the current health to the maximum health
        
        target = GameObject.FindGameObjectWithTag("Player").transform;
        // Find the player object by tag and store its transform component
        
        animator = GetComponent<Animator>(); // Corrected line
        
        spriteRenderer = GetComponent<SpriteRenderer>(); // Corrected line
        

         // Check if the SpriteRenderer is missing, and log an error if it is
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component is missing from this game object.");
        }
        // If an animator is found, play the idle animation
        if (animator != null)
        {
            animator.Play("goblin_Idle");
        }
    }

    void Update()
    {
        if (!isDead && !isAttacking)// If the enemy is not dead and not attacking, follow the player
        {
            FollowPlayer();
        }
    }

    // Method to make the enemy follow the player
    void FollowPlayer()
    {
        // Calculate the distance to the player
        float distanceToPlayer = Vector2.Distance(transform.position, target.position);
        
        // If the player is within the follow distance
        if (distanceToPlayer < followDistance)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, target.position, enemyMoveSpeed * Time.deltaTime);

            if (spriteRenderer != null)
            {
                spriteRenderer.flipX = direction.x < 0;
            }

            if (distanceToPlayer <= 1.5f) // Attack range
            {
                
                Attack();
            }
            else
            {
                animator.SetBool("isWalking", true);
            }
        }
        else
        {
            // Stop the walking animation if the player is out of range   
            animator.SetBool("isWalking", false);
        }
    }

    void Attack()
    {
        // If the enemy is dead, do nothing
        if (isDead) return;
         // Set the attacking state to true
        isAttacking = true;
        animator.SetBool("isWalking", false); // Stop walking
        animator.SetTrigger("attack");
        
        nextAttackTime = Time.time + cooldownTime;

        // Call the player's TakeDamage method when the attack animation is finished
        Invoke("ApplyDamageToPlayer", 0.5f); // Adjust the delay based on attack animation timing
    }
    // Method to apply damage to the player
    void ApplyDamageToPlayer()
    {
        if (playerHealth != null)// reduce their health
        {
            playerHealth.Takedamage(1); // Adjust damage as needed
        }
        // Reset the attacking state
        isAttacking = false;
    }

    public void Takedamage(int damage)// Method to handle taking damage
    {
        if (isDead) return;// If the enemy is dead, do nothing

        currentHealth -= damage;// Reduce the current health by the damage amount

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            animator.SetTrigger("Hurt");
        }
    }

    void Die()
    {
        isDead = true;// Set the dead state to true
        animator.SetTrigger("Die");
        StartCoroutine(DestroyAfterAnimation());
    }

    IEnumerator DestroyAfterAnimation()// Coroutine to destroy the enemy after the death animation finishes
    {
        yield return new WaitForEndOfFrame();

        if (animator != null)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("goblin_Die"))
            {
                float dieAnimationLength = stateInfo.length;
                Destroy(gameObject, dieAnimationLength);
            }
            else
            {
                Destroy(gameObject, 1f); // Fallback in case of error
            }
        }
    }

    public void OnAttackFinished()
    {
        isAttacking = false; // Reset the attack state
    }
}

