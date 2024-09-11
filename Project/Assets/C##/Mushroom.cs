
// using System.Collections;  
// using System.Collections.Generic;  
// using UnityEngine;  

// public class Mushroom : MonoBehaviour  
// {  
//     public int maxHealth = 100;  
//     private int currentHealth;  
//     private Transform target;  

//     public float enemyMoveSpeed = 2f;  
//     public float followDistance = 10f;  
//     public float stunDuration = 2f;  

//     private Animator animator;  
//     private SpriteRenderer spriteRenderer;  
//     public PlayerCombat playerCombat;  

//     private bool isDead = false;  
//     private bool isAttacking = false;  
//     private bool isStunned = false;  

//     public Health playerHealth;  
//     public float cooldownTime = 0.75f;  
//     private float nextAttackTime = 0f;  
//     private MushroomHealth mushroomHealth;  

//     void Start()  
//     {  
//         currentHealth = maxHealth;  
//         target = GameObject.FindGameObjectWithTag("Player").transform;  
//         mushroomHealth = GetComponent<MushroomHealth>();  
//         if (mushroomHealth == null)  
//         {  
//             Debug.LogError("MushroomHealth component is missing from this game object.");  
//         }  

//         animator = GetComponent<Animator>();  
//         spriteRenderer = GetComponent<SpriteRenderer>();  
//         playerCombat = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>();  

//         if (spriteRenderer == null)  
//         {  
//             Debug.LogError("SpriteRenderer component is missing from this game object.");  
//         }  

//         if (animator != null)  
//         {  
//             animator.Play("Mushroom idle");  
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
//         if (isDead || Time.time < nextAttackTime) return;  

//         isAttacking = true;  
//         animator.SetBool("isWalking", false);  
//         animator.SetTrigger("attack");  

//         nextAttackTime = Time.time + cooldownTime;  
//         CheckPlayerBlocking();  
//     }  

//     void CheckPlayerBlocking()  
//     {  
//         bool isBlocking = playerCombat != null && playerCombat.IsBlocking();  
//         ApplyDamageToPlayer(isBlocking);  
//     }  

//     void ApplyDamageToPlayer(bool isBlocking)  
//     {  
//         if (isBlocking)  
//         {  
//             if (playerCombat != null)  
//             {  
//                 playerCombat.TriggerBlockImpact();  
//                 StartCoroutine(Stun());  
//             }  
//             Debug.Log("Player blocked the attack!");  
//         }  
//         else  
//         {  
//             if (playerHealth != null)  
//             {  
//                 playerHealth.Takedamage(1); // Damage amount can be adjusted  
//                 Debug.Log("Damage applied to player.");  
//             }  
//         }  
//         isAttacking = false;  
//     }  

//     public void Takedamage(int damage, bool isBlocking)  
//     {  
//         Debug.Log($"Takedamage called with damage: {damage}, isBlocking: {isBlocking}");  

//         if (isDead) return;  

//         if (isBlocking)  
//         {  
//             Debug.Log("Enemy is stunned due to block.");  
//             StartCoroutine(Stun());  
//         }  
//         else  
//         {  
//             currentHealth -= damage; // Update current health  
//             mushroomHealth.TakeDamage(damage); // Call TakeDamage on MushroomHealth  

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
//             if (stateInfo.IsName("Mushroom_die"))  
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
using System.Collections;  
using System.Collections.Generic;  
using UnityEngine;  

public class Mushroom : MonoBehaviour  
{  
    public int maxHealth = 100;  
    private int currentHealth;  
    private Transform target;  

    public float enemyMoveSpeed = 2f;  
    public float followDistance = 10f;  
    public float stunDuration = 2f;  

    private Animator animator;  
    private SpriteRenderer spriteRenderer;  
    public PlayerCombat playerCombat;  

    private bool isDead = false;  
    private bool isAttacking = false;  
    private bool isStunned = false;  

    public Health playerHealth;  
    public float cooldownTime = 0.75f;  
    private float nextAttackTime = 0f;  
    private float heavyAttackCooldown = 3f; // Cooldown for heavy attack  
    private float nextHeavyAttackTime = 0f; // Time for the next heavy attack  
    private MushroomHealth mushroomHealth;  

    public int heavyAttackDamage = 20; // Damage for heavy attack  
    public int normalAttackDamage = 10; // Damage for normal attack  
    public float normalAttackRange = 1.5f; // Range for normal attack  
    public float heavyAttackRange = 3.5f; // Range for heavy attack  

    void Start()  
    {  
        currentHealth = maxHealth;  
        target = GameObject.FindGameObjectWithTag("Player").transform;  
        mushroomHealth = GetComponent<MushroomHealth>();  
        if (mushroomHealth == null)  
        {  
            Debug.LogError("MushroomHealth component is missing from this game object.");  
        }  

        animator = GetComponent<Animator>();  
        spriteRenderer = GetComponent<SpriteRenderer>();  
        playerCombat = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>();  

        if (spriteRenderer == null)  
        {  
            Debug.LogError("SpriteRenderer component is missing from this game object.");  
        }  

        if (animator != null)  
        {  
            animator.Play("Mushroom idle");  
        }  
    }  

    void Update()  
    {  
        if (!isDead && !isAttacking && !isStunned)  
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
                spriteRenderer.flipX = direction.x < 0;  
            }  

            animator.SetBool("isWalking", true);  
        }  
        else  
        {  
            animator.SetBool("isWalking", false);  
        }  
    }  

    void OnTriggerEnter2D(Collider2D other)  
    {  
        if (other.CompareTag("Player"))  
        {  
            // Check if the player is within the heavy attack range  
            if (Vector2.Distance(transform.position, other.transform.position) <= heavyAttackRange && Time.time >= nextHeavyAttackTime)  
            {  
                HeavyAttack();  
            }  
            // Check if the player is within the normal attack range  
            else if (Vector2.Distance(transform.position, other.transform.position) <= normalAttackRange && Time.time >= nextAttackTime)  
            {  
                Attack();  
            }  
        }  
    }  

    void Attack()  
    {  
        if (isDead) return;  

        isAttacking = true;  
        animator.SetBool("isWalking", false);  
        animator.SetTrigger("attack");  

        nextAttackTime = Time.time + cooldownTime;  
        CheckPlayerBlocking();  
    }  

    void HeavyAttack()  
    {  
        if (isDead) return;  

        isAttacking = true;  
        animator.SetBool("isWalking", false);  
        animator.SetTrigger("heavyAttack"); // Trigger heavy attack animation  

        nextHeavyAttackTime = Time.time + heavyAttackCooldown;  
        CheckPlayerBlockingHeavy();  
    }  

    void CheckPlayerBlocking()  
    {  
        bool isBlocking = playerCombat != null && playerCombat.IsBlocking();  
        ApplyDamageToPlayer(isBlocking, normalAttackDamage); // Regular attack damage  
    }  

    void CheckPlayerBlockingHeavy()  
    {  
        bool isBlocking = playerCombat != null && playerCombat.IsBlocking();  
        ApplyDamageToPlayer(isBlocking, heavyAttackDamage); // Heavy attack damage  
    }  

    void ApplyDamageToPlayer(bool isBlocking, int damage)  
    {  
        if (isBlocking)  
        {  
            if (playerCombat != null)  
            {  
                playerCombat.TriggerBlockImpact();  
                StartCoroutine(Stun());  
            }  
            Debug.Log("Player blocked the attack!");  
        }  
        else  
        {  
            if (playerHealth != null)  
            {  
                playerHealth.Takedamage(damage);  
                Debug.Log("Damage applied to player.");  
            }  
        }  
        isAttacking = false;  
    }  

    public void Takedamage(int damage, bool isBlocking)  
    {  
        Debug.Log($"Takedamage called with damage: {damage}, isBlocking: {isBlocking}");  

        if (isDead) return;  

        if (isBlocking)  
        {  
            Debug.Log("Enemy is stunned due to block.");  
            StartCoroutine(Stun());  
        }  
        else  
        {  
            if (mushroomHealth == null)  
            {  
                Debug.LogError("MushroomHealth is null at the time of taking damage.");  
                return; // Exit early if mushroomHealth is null  
            }  

            currentHealth -= damage; // Update current health  
            mushroomHealth.TakeDamage(damage); // Call TakeDamage on MushroomHealth  

            if (currentHealth <= 0)  
            {  
                Die();  
            }  
            else  
            {  
                animator.SetTrigger("Hurt");  
            }  
        }  
    }  

    IEnumerator Stun()  
    {  
        isStunned = true;  
        animator.SetTrigger("Stunned");  
        yield return new WaitForSeconds(stunDuration);  
        isStunned = false;  
    }  

    void Die()  
    {  
        isDead = true;  
        animator.SetTrigger("Die");  
        StartCoroutine(DestroyAfterAnimation());  
    }  

    IEnumerator DestroyAfterAnimation()  
    {  
        yield return new WaitForEndOfFrame();  

        if (animator != null)  
        {  
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);  
            if (stateInfo.IsName("Mushroom_die"))  
            {  
                float dieAnimationLength = stateInfo.length;  
                Destroy(gameObject, dieAnimationLength);  
            }  
            else  
            {  
                Destroy(gameObject, 1f);  
            }  
        }  
    }  

    public void OnAttackFinished()  
    {  
        isAttacking = false;  
    }  
}