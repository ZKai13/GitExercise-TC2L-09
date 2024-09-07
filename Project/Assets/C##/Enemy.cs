// using UnityEngine;
// using System.Collections;

// public class Enemy : MonoBehaviour
// {
//     public int maxHealth = 100;
//     private int currentHealth;
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

//     // Stun properties
//     public float stunDuration = 2f; // How long the enemy stays stunned
//     private bool isStunned = false; // Flag to check if the enemy is stunned

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
//         if (!isDead && !isAttacking && !isStunned) // Add !isStunned to prevent movement when stunned
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

//             if (distanceToPlayer <= 1.5f && Time.time >= nextAttackTime) // Attack range
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
//         if (isDead || isStunned) return; // Prevent attack if dead or stunned

//         isAttacking = true;
//         animator.SetBool("isWalking", false); // Stop walking
//         animator.SetTrigger("attack");
        
//         nextAttackTime = Time.time + cooldownTime;
//         Invoke("ApplyDamageToPlayer", 0.5f); // Adjust the delay based on attack animation timing
//     }

//     void ApplyDamageToPlayer()
//     {
//         if (playerHealth != null)
//         {
//             playerHealth.Takedamage(1); // Adjust damage as needed
//         }

//         isAttacking = false;
//     }

//     public void Takedamage(int damage, bool isBlocking)
//     {
//         if (isDead) return;

//         currentHealth -= damage;

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

//     public void GetStunned()
//     {
//         if (!isStunned && !isDead) // Prevent stun if already stunned or dead
//         {
//             StartCoroutine(ApplyStun());
//         }
//     }

//     IEnumerator ApplyStun()
//     {
//         isStunned = true;
//         isAttacking = false; // Cancel any attack in progress
//         animator.SetBool("isWalking", false);
//         animator.SetTrigger("Stunned"); // Optional: If you have a stunned animation

//         yield return new WaitForSeconds(stunDuration);

//         isStunned = false;
//     }

//     public void OnAttackFinished()
//     {
//         isAttacking = false;
//     }
// }
// using UnityEngine;
// using System.Collections;

// public class Enemy : MonoBehaviour
// {
//     [Header("Stats")]
//     public int maxHealth = 100;
//     public int currentHealth;
//     public int damage = 20;
//     public float moveSpeed = 3f;

//     [Header("Attack Settings")]
//     public float attackRange = 2f;
//     public float attackCooldown = 2f;
//     public float attackWindupTime = 0.5f;
//     [Header("Chase Settings")]
//     public float maxChaseDistance = 5f;

//     [Header("Stun Settings")]
//     public float stunDuration = 3f;

//     [Header("References")]
//     public Transform attackPoint;
//     public LayerMask playerLayer;
//     private Vector3 originalScale;

//     private bool canAttack = true;
//     private bool isStunned = false;
//     private Transform player;
//     private PlayerCombat playerCombat;
//     private Animator animator;
//     private Rigidbody2D rb;

//     void Start()
//     {
//         currentHealth = maxHealth;
//         player = GameObject.FindGameObjectWithTag("Player").transform;
//         playerCombat = player.GetComponent<PlayerCombat>();
//         animator = GetComponent<Animator>();
//         rb = GetComponent<Rigidbody2D>();
//         originalScale = transform.localScale;

//         if (attackPoint == null)
//         {
//             attackPoint = transform;
//         }
//     }

//     void Update()
//     {
//         if (!isStunned)
//         {
//             float distanceToPlayer = Vector2.Distance(transform.position, player.position);
//             if (distanceToPlayer <= maxChaseDistance)
//             {

//             MoveTowardsPlayer();
            
//             if (canAttack && Vector2.Distance(transform.position, player.position) <= attackRange)
//             {
//                 StartCoroutine(AttackPlayer());
//             }
//         }
//         else
//                 rb.velocity = Vector2.zero;
//                 animator.SetBool("isWalking", false);        
//     }

//     void MoveTowardsPlayer()
//     {
//         Vector2 direction = (player.position - transform.position).normalized;
//         rb.velocity = direction * moveSpeed;

//         // Update animation
//         animator.SetBool("isWalking", rb.velocity.magnitude > 0.1f);

//         // Flip sprite if needed
//         if (direction.x < 0)
//         {
//             transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
//         }
//         else if (direction.x > 0)
//         {
//             transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
//         }
//     }

//     IEnumerator AttackPlayer()
//     {
//         canAttack = false;
//         rb.velocity = Vector2.zero;
//         animator.SetTrigger("attack");

//         // Attack windup
//         yield return new WaitForSeconds(attackWindupTime);

//         // Perform attack
//         Collider2D hitPlayer = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);
//         if (hitPlayer != null)
//         {
//             playerCombat.OnEnemyAttack(damage);
//         }

//         // Attack cooldown
//         yield return new WaitForSeconds(attackCooldown - attackWindupTime);
//         canAttack = true;
//     }
//     }

//     public void Takedamage(int damage, bool isHeavyAttack)
//     {
//         currentHealth -= damage;
//         animator.SetTrigger("Hurt");

//         if (currentHealth <= 0)
//         {
//             Die();
//         }
//     }

//     void Die()
//     {
//         animator.SetTrigger("Die");
//         rb.velocity = Vector2.zero;
//         GetComponent<Collider2D>().enabled = false;
//         this.enabled = false;
        
//         // You might want to add a delay before destroying the object
//         Destroy(gameObject, 2f);
//     }

//     public void GetStunned()
//     {
//         if (!isStunned)
//         {
//             StartCoroutine(ApplyStun());
//         }
//     }

//     IEnumerator ApplyStun()
//     {
//         isStunned = true;
//         rb.velocity = Vector2.zero;
//         animator.SetBool("Stunned", true);

//         yield return new WaitForSeconds(stunDuration);

//         isStunned = false;
//         animator.SetBool("Stunned", false);
//     }

//     void OnDrawGizmosSelected()
//     {
//         if (attackPoint == null)
//             return;

//         Gizmos.color = Color.red;
//         Gizmos.DrawWireSphere(attackPoint.position, attackRange);
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

    [Header("Chase Settings")]
    public float maxChaseDistance = 5f;

    [Header("Stun Settings")]
    public float stunDuration = 3f;

    [Header("References")]
    public Transform attackPoint;
    public LayerMask playerLayer;
    private Vector3 originalScale;

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
        originalScale = transform.localScale;

        if (attackPoint == null)
        {
            attackPoint = transform;
        }
    }

    void Update()
    {
        if (!isStunned)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer <= maxChaseDistance)
            {
                MoveTowardsPlayer();

                if (canAttack && distanceToPlayer <= attackRange)
                {
                    StartCoroutine(AttackPlayer());
                }
            }
            else
            {
                rb.velocity = Vector2.zero;
                animator.SetBool("isWalking", false);
            }
        }
    }

    void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;

        // Update walking animation
        animator.SetBool("isWalking", rb.velocity.magnitude > 0.1f);

        // Flip sprite based on movement direction
        if (direction.x < 0)
        {
            transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
        }
        else if (direction.x > 0)
        {
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
        }
    }

    IEnumerator AttackPlayer()
    {
        canAttack = false;
        rb.velocity = Vector2.zero;
        animator.SetTrigger("attack");

        // Attack windup
        yield return new WaitForSeconds(attackWindupTime);

        // Perform attack
        Collider2D hitPlayer = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);
        if (hitPlayer != null)
        {
            playerCombat.OnEnemyAttack(damage); // Call the player's damage function
        }

        // Attack cooldown
        yield return new WaitForSeconds(attackCooldown - attackWindupTime);
        canAttack = true;
    }

    public void Takedamage(int damage, bool isHeavyAttack)
    {
        currentHealth -= damage;
        animator.SetTrigger("Hurt"); // Play hurt animation

        if (currentHealth <= 0)
        {
            Die(); // Play die animation and disable enemy
        }
    }

    void Die()
    {
        animator.SetTrigger("Die");
        rb.velocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false; // Disable collider
        this.enabled = false; // Disable script

        // Destroy the game object after 2 seconds to allow death animation to play
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
        rb.velocity = Vector2.zero; // Stop movement
        animator.SetBool("Stunned", true); // Play stunned animation

        yield return new WaitForSeconds(stunDuration);

        isStunned = false;
        animator.SetBool("Stunned", false); // End stunned state
    }

    // Visualize the attack range in the Unity editor
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
