// using UnityEngine;
// using System.Collections;

// public class Enemy : MonoBehaviour
// {
//     public int maxHealth = 100;
//     private int currentHealth;
//     private Transform target;
//     public float enemyMoveSpeed = 2f;
//     public float followDistance = 10f;
    
//     private Animator animator;
//     private SpriteRenderer spriteRenderer;

//     private bool isDead = false;

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
//             animator.Play("Idle");
//         }
//     }

//     void Update()
//     {
//         if (!isDead)
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
//                 spriteRenderer.flipX = direction.x > 0;
//             }
//         }
//     }

//     public void Takedamage(int damage)
//     {
//         if (isDead) return;

//         currentHealth -= damage;

//         if (currentHealth <= 0)
//         {
//             Die();
//         }
//         else
//         {
//             if (animator != null)
//             {
//                 animator.SetTrigger("Hurt");
//             }
//         }
//     }

//     void Die()
//     {
//         isDead = true;
//         if (animator != null)
//         {
//             Debug.Log("Setting 'Die' trigger.");
//             animator.SetTrigger("Die");
//             StartCoroutine(DestroyAfterAnimation());
//         }
//     }

//     IEnumerator DestroyAfterAnimation()
//     {
//         yield return new WaitForEndOfFrame();

//         if (animator != null)
//         {
//             AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
//             Debug.Log("Current State: " + stateInfo.fullPathHash);

//             if (stateInfo.IsName("Die"))
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
// }
// using UnityEngine;
// using System.Collections;

// public class Enemy : MonoBehaviour
// {
//     public int maxHealth = 100;
//     private int currentHealth;
//     private Transform target;
//     public float enemyMoveSpeed = 2f;
//     public float followDistance = 10f;
    
//     private Animator animator;
//     private SpriteRenderer spriteRenderer;

//     private bool isDead = false;

//     // Reference to the player's Health script
//     private Health playerHealth;

//     void Start()
//     {
//         currentHealth = maxHealth;
//         target = GameObject.FindGameObjectWithTag("Player").transform;
//         animator = GetComponent<Animator>();
//         spriteRenderer = GetComponent<SpriteRenderer>();
//         playerHealth = target.GetComponent<Health>();

//         if (spriteRenderer == null)
//         {
//             Debug.LogError("SpriteRenderer component is missing from this game object.");
//         }

//         if (animator != null)
//         {
//             animator.Play("Idle");
//         }
//     }

//     void Update()
//     {
//         if (!isDead)
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
//                 spriteRenderer.flipX = direction.x > 0;
//             }

//             // Attack the player if within range
//             if (distanceToPlayer < 1.5f) // Adjust this range as needed
//             {
//                 AttackPlayer();
//             }
//         }
//     }

//     void AttackPlayer()
//     {
//         if (playerHealth != null)
//         {
//             playerHealth.Takedamage(10); // Adjust damage value as needed
//         }
//     }

//     public void Takedamage(int damage)
//     {
//         if (isDead) return;

//         currentHealth -= damage;

//         if (currentHealth <= 0)
//         {
//             Die();
//         }
//         else
//         {
//             if (animator != null)
//             {
//                 animator.SetTrigger("Hurt");
//             }
//         }
//     }

//     void Die()
//     {
//         isDead = true;
//         if (animator != null)
//         {
//             Debug.Log("Setting 'Die' trigger.");
//             animator.SetTrigger("Die");
//             StartCoroutine(DestroyAfterAnimation());
//         }
//     }

//     IEnumerator DestroyAfterAnimation()
//     {
//         yield return new WaitForEndOfFrame();

//         if (animator != null)
//         {
//             AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
//             Debug.Log("Current State: " + stateInfo.fullPathHash);

//             if (stateInfo.IsName("Die"))
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
// }
// using UnityEngine;
// using System.Collections;

// public class Enemy : MonoBehaviour
// {
//     public int maxHealth = 100;
//     private int currentHealth;
//     private Transform target;
//     public float enemyMoveSpeed = 2f;
//     public float followDistance = 10f;
    
//     private Animator animator;
//     private SpriteRenderer spriteRenderer;

//     private bool isDead = false;
//     private bool isAttacking = false;

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
//         if (!isDead && !isAttacking)
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
//                 spriteRenderer.flipX = direction.x > 0;
//             }

//             if (distanceToPlayer <= 1f) // Attack range
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
//         animator.SetBool("isWalking", false); // Stop walking
//         animator.SetTrigger("attack");
//     }

//     public void Takedamage(int damage)
//     {
//         if (isDead) return;

//         currentHealth -= damage;

//         if (currentHealth <= 0)
//         {
//             Die();
//         }
//         else
//         {
//             animator.SetTrigger("hurt");
//         }
//     }

//     void Die()
//     {
//         isDead = true;
//         animator.SetTrigger("die");
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
//                 Destroy(gameObject, 1f); // Fallback in case of error
//             }
//         }
//     }

//     // This method would be called as an event in the animation, after the attack animation ends.
//     public void OnAttackFinished()
//     {
//         isAttacking = false; // Reset the attack state
//     }
// }
using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    private Transform target;
    public float enemyMoveSpeed = 2f;
    public float followDistance = 10f;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private bool isDead = false;
    private bool isAttacking = false;

    public Health playerHealth; // Reference to the Player's Health script
    void Start()
    {
        currentHealth = maxHealth;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>(); // Corrected line
        spriteRenderer = GetComponent<SpriteRenderer>(); // Corrected line

        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component is missing from this game object.");
        }

        if (animator != null)
        {
            animator.Play("goblin_Idle");
        }
    }

    void Update()
    {
        if (!isDead && !isAttacking)
        {
            FollowPlayer();
        }
    }

    void FollowPlayer()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, target.position);

        if (distanceToPlayer < followDistance)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, target.position, enemyMoveSpeed * Time.deltaTime);

            if (spriteRenderer != null)
            {
                spriteRenderer.flipX = direction.x > 0;
            }

            if (distanceToPlayer <= 1f) // Attack range
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
            animator.SetBool("isWalking", false);
        }
    }

    void Attack()
    {
        if (isDead) return;

        isAttacking = true;
        animator.SetBool("isWalking", false); // Stop walking
        animator.SetTrigger("attack");

        // Call the player's TakeDamage method when the attack animation is finished
        Invoke("ApplyDamageToPlayer", 0.5f); // Adjust the delay based on attack animation timing
    }

    void ApplyDamageToPlayer()
    {
        if (playerHealth != null)
        {
            playerHealth.Takedamage(10); // Adjust damage as needed
        }
        isAttacking = false;
    }

    public void Takedamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            animator.SetTrigger("hurt");
        }
    }

    void Die()
    {
        isDead = true;
        animator.SetTrigger("die");
        StartCoroutine(DestroyAfterAnimation());
    }

    IEnumerator DestroyAfterAnimation()
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

