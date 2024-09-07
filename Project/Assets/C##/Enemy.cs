// using UnityEngine;
// using System.Collections;

// public class Enemy : MonoBehaviour
// {
//     public int maxHealth = 100;
//     //The maximum health the enemy can have. 
//     private int currentHealth;
//     //Tracks the enemy's current health
//     private Transform target;
//     public Transform attackPoint;
    
//     public float attackRange = 2.5f;    
//     public float enemyMoveSpeed = 2f;
//     public float followDistance = 10f;

//     private Animator animator;
//     private SpriteRenderer spriteRenderer;

//     private bool isDead = false;
//     private bool isAttacking = false;

//     public Health playerHealth; // Reference to the Player's Health script
//     public float cooldownTime = 0.75f; // Cooldown time after an attack
//     private float nextAttackTime = 0f; // The time at which the enemy can attack again
//     void Start()
//     {
//         currentHealth = maxHealth;
//         // Initialize the current health to the maximum health
        
//         target = GameObject.FindGameObjectWithTag("Player").transform;
//         // Find the player object by tag and store its transform component
        
//         animator = GetComponent<Animator>(); // Corrected line
        
//         spriteRenderer = GetComponent<SpriteRenderer>(); // Corrected line
        

//          // Check if the SpriteRenderer is missing, and log an error if it is
//         if (spriteRenderer == null)
//         {
//             Debug.LogError("SpriteRenderer component is missing from this game object.");
//         }
//         // If an animator is found, play the idle animation
//         if (animator != null)
//         {
//             animator.Play("goblin_Idle");
//         }
//     }

//     void Update()
//     {
//         if (!isDead && !isAttacking)// If the enemy is not dead and not attacking, follow the player
//         {
//             FollowPlayer();
//         }
//     }

//     // Method to make the enemy follow the player
//     void FollowPlayer()
//     {
//         // Calculate the distance to the player
//         float distanceToPlayer = Vector2.Distance(transform.position, target.position);
        
//         // If the player is within the follow distance
//         if (distanceToPlayer < followDistance)
//         {
//             Vector2 direction = (target.position - transform.position).normalized;
//             transform.position = Vector2.MoveTowards(transform.position, target.position, enemyMoveSpeed * Time.deltaTime);

//             if (spriteRenderer != null)
//             {
//                 spriteRenderer.flipX = direction.x < 0;
//             }

//             if (distanceToPlayer <= 1.5f) // Attack range
//             {
                
//                 Attack();
//             }
//             else
//             {
//                 animator.SetBool("isWalking", true);
//             }
//         }
//         else
//         {
//             // Stop the walking animation if the player is out of range   
//             animator.SetBool("isWalking", false);
//         }
//     }

//     void Attack()
//     {
//         // If the enemy is dead, do nothing
//         if (isDead) return;
//          // Set the attacking state to true
//         isAttacking = true;
//         animator.SetBool("isWalking", false); // Stop walking
//         animator.SetTrigger("attack");
        
//         nextAttackTime = Time.time + cooldownTime;

//         // Call the player's TakeDamage method when the attack animation is finished
//         Invoke("ApplyDamageToPlayer", 0.5f); // Adjust the delay based on attack animation timing
//     }
//     // Method to apply damage to the player
//     void ApplyDamageToPlayer()
//     {
//         if (playerHealth != null)// reduce their health
//         {
//             playerHealth.Takedamage(1); // Adjust damage as needed
//         }
//         // Reset the attacking state
//         isAttacking = false;
//     }

//     public void Takedamage(int damage)// Method to handle taking damage
//     {
//         if (isDead) return;// If the enemy is dead, do nothing

//         currentHealth -= damage;// Reduce the current health by the damage amount

//         if (currentHealth <= 0)
//         {
//             Die();
//         }
//         else
//         {
//             animator.SetTrigger("Hurt");
//         }
//     }

//     void Die()
//     {
//         isDead = true;// Set the dead state to true
//         animator.SetTrigger("Die");
//         StartCoroutine(DestroyAfterAnimation());
//     }

//     IEnumerator DestroyAfterAnimation()// Coroutine to destroy the enemy after the death animation finishes
//     {
//         yield return new WaitForEndOfFrame();

//         if (animator != null)
//         {
//             AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
//             if (stateInfo.IsName("goblin_Die"))
//             {
//                 float dieAnimationLength = stateInfo.length;
//                 Destroy(gameObject, dieAnimationLength);
//             }
//             else
//             {
//                 Destroy(gameObject, 1f); // Fallback in case of error
//             }
//         }
//     }

//     public void OnAttackFinished()
//     {
//         isAttacking = false; // Reset the attack state
//     }
// }
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Enemy : MonoBehaviour
// {
//     public int maxHealth = 100;
//     private int currentHealth;
//     private Transform target;

//     public float enemyMoveSpeed = 2f;
//     public float followDistance = 10f;
//     public float stunDuration = 2f;

//     private Animator animator;
//     private SpriteRenderer spriteRenderer;

//     private bool isDead = false;
//     private bool isAttacking = false;
//     private bool isStunned = false;

//     public Health playerHealth;
//     public float cooldownTime = 0.75f;
//     private float nextAttackTime = 0f;

//     void Start()
//     {
//         currentHealth = maxHealth;
//         target = GameObject.FindGameObjectWithTag("Player").transform;

//         animator = GetComponent<Animator>();
//         spriteRenderer = GetComponent<SpriteRenderer>();

//         if (spriteRenderer == null)
//         {
//             Debug.LogError("SpriteRenderer component is missing from this game object.");
//         }

//         if (animator != null)
//         {
//             animator.Play("goblin_Idle");
//         }
//     }

//     void Update()
//     {
//         if (!isDead && !isAttacking && !isStunned)
//         {
//             FollowPlayer();
//         }
//     }

//     void FollowPlayer()
//     {
//         float distanceToPlayer = Vector2.Distance(transform.position, target.position);

//         if (distanceToPlayer < followDistance)
//         {
//             Vector2 direction = (target.position - transform.position).normalized;
//             transform.position = Vector2.MoveTowards(transform.position, target.position, enemyMoveSpeed * Time.deltaTime);

//             if (spriteRenderer != null)
//             {
//                 spriteRenderer.flipX = direction.x < 0;
//             }

//             if (distanceToPlayer <= 1.5f)
//             {
//                 Attack();
//             }
//             else
//             {
//                 animator.SetBool("isWalking", true);
//             }
//         }
//         else
//         {
//             animator.SetBool("isWalking", false);
//         }
//     }

//     void Attack()
//     {
//         if (isDead) return;

//         isAttacking = true;
//         animator.SetBool("isWalking", false);
//         animator.SetTrigger("attack");

//         nextAttackTime = Time.time + cooldownTime;
//         Invoke("ApplyDamageToPlayer", 0.5f);
//     }

//     void ApplyDamageToPlayer()
//     {
//         if (playerHealth != null)
//         {
//             playerHealth.Takedamage(1);
//         }
//         isAttacking = false;
//     }

//     public void Takedamage(int damage, bool isBlocked)
//     {
//         if (isDead) return;

//         if (isBlocked)
//         {
//             StartCoroutine(Stun());
//         }
//         else
//         {
//             currentHealth -= damage;

//             if (currentHealth <= 0)
//             {
//                 Die();
//             }
//             else
//             {
//                 animator.SetTrigger("Hurt");
//             }
//         }
//     }

//     IEnumerator Stun()
//     {
//         isStunned = true;
//         animator.SetTrigger("Stunned");
//         yield return new WaitForSeconds(stunDuration);
//         isStunned = false;
//     }

//     void Die()
//     {
//         isDead = true;
//         animator.SetTrigger("Die");
//         StartCoroutine(DestroyAfterAnimation());
//     }

//     IEnumerator DestroyAfterAnimation()
//     {
//         yield return new WaitForEndOfFrame();

//         if (animator != null)
//         {
//             AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
//             if (stateInfo.IsName("goblin_Die"))
//             {
//                 float dieAnimationLength = stateInfo.length;
//                 Destroy(gameObject, dieAnimationLength);
//             }
//             else
//             {
//                 Destroy(gameObject, 1f);
//             }
//         }
//     }

//     public void OnAttackFinished()
//     {
//         isAttacking = false;
//     }
// }
using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public int maxHealth = 100;
    public int currentHealth;
    public int damage = 20;
    public float moveSpeed = 3f;

    [Header("Attack Settings")]
    public float attackRange = 2f;
    public float attackCooldown = 2f;
    public float attackWindupTime = 0.5f;

    [Header("Stun Settings")]
    public float stunDuration = 3f;

    [Header("References")]
    public Transform attackPoint;
    public LayerMask playerLayer;

    private bool canAttack = true;
    private bool isStunned = false;
    private Transform player;
    private PlayerCombat playerCombat;
    private Animator animator;
    private Rigidbody2D rb;

    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerCombat = player.GetComponent<PlayerCombat>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        if (attackPoint == null)
        {
            attackPoint = transform;
        }
    }

    void Update()
    {
        if (!isStunned)
        {
            MoveTowardsPlayer();
            
            if (canAttack && Vector2.Distance(transform.position, player.position) <= attackRange)
            {
                StartCoroutine(AttackPlayer());
            }
        }
    }

    void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;

        // Update animation
        animator.SetBool("isWalking", rb.velocity.magnitude > 0.1f);

        // Flip sprite if needed
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    IEnumerator AttackPlayer()
    {
        canAttack = false;
        rb.velocity = Vector2.zero;
        animator.SetTrigger("attack");
        animator.SetBool("isWalking", false);

        // Attack windup
        yield return new WaitForSeconds(attackWindupTime);

        // Perform attack
        Collider2D hitPlayer = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);
        if (hitPlayer != null)
        {
            playerCombat.OnEnemyAttack(damage);
        }

        // Attack cooldown
        yield return new WaitForSeconds(attackCooldown - attackWindupTime);
        canAttack = true;
    }

    public void Takedamage(int damage, bool isBlocking)
    {
        if(isBlocking)
        {
            return;
        }

        currentHealth -= damage;
        animator.SetTrigger("Hit");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        animator.SetTrigger("Die");
        rb.velocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        
        // You might want to add a delay before destroying the object
        Destroy(gameObject, 2f);
    }

    public void GetStunned()
    {
        if (!isStunned)
        {
            StartCoroutine(ApplyStun());
        }
    }

    IEnumerator ApplyStun()
    {
        isStunned = true;
        rb.velocity = Vector2.zero;
        animator.SetBool("Stunned", true);
        animator.SetBool("isWalking", false);

        yield return new WaitForSeconds(stunDuration);

        isStunned = false;
        animator.SetBool("Stunned", false);
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}

