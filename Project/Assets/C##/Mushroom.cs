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
    public float cooldownTime = 1f;  
    private float nextAttackTime = 1f;  
    private float heavyAttackCooldown = 5f;  
    private float nextHeavyAttackTime = 1f; 
     
    private MushroomHealth mushroomHealth;  

    [SerializeField] private int heavyAttackDamage = 5;  
    [SerializeField] private int normalAttackDamage = 3;  
    public float normalAttackRange = 1.5f;  
    public float heavyAttackRange = 3.5f;  
    public float heavyAttackKnockbackForce = 5f;  

    public BoxCollider2D lightAttackCollider;  

    void Start()  
    {  
        currentHealth = maxHealth;  
        target = GameObject.FindGameObjectWithTag("Player").transform;  
        mushroomHealth = GetComponent<MushroomHealth>();  
        animator = GetComponent<Animator>();  
        spriteRenderer = GetComponent<SpriteRenderer>();  
        playerCombat = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>();  
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();  
        

        if (mushroomHealth == null || animator == null || spriteRenderer == null || playerCombat == null || playerHealth == null)  
        {  
            Debug.LogError("One or more required components are missing!");  
        }  
    }  

    void Update()  
    {  
        if (!isDead && !isAttacking && !isStunned)  
        {  
            FollowPlayer();  
            CheckForAttacks();  
        }  
    }  

    void CheckForAttacks()  
    {  
        if (target == null)  
        {  
            Debug.LogError("Target (player) is null!");  
            return;  
        }  

        float distanceToPlayer = Vector2.Distance(transform.position, target.position);  
        Debug.Log($"Distance to player: {distanceToPlayer}");  
        
        if (distanceToPlayer <= heavyAttackRange && Time.time >= nextHeavyAttackTime)  
        {  
            Debug.Log("Attempting Heavy Attack");  
            HeavyAttack();  
        }  
        else if (distanceToPlayer <= normalAttackRange && Time.time >= nextAttackTime)  
        {  
            Debug.Log("Attempting Normal Attack");  
            Attack();  
        }  
        else  
        {  
            Debug.Log("Player not in range or attack on cooldown");  
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

    void Attack()  
    {  
        Debug.Log("Attack method called");  
        if (isDead)   
        {  
            Debug.Log("Attack cancelled: Mushroom is dead");  
            return;  
        }  

        isAttacking = true;  
        if (animator != null)  
        {  
            animator.SetBool("isWalking", false);  
            animator.SetTrigger("attack");  
            Debug.Log("Attack animation triggered");  
        }  
        else  
        {  
            Debug.LogError("Animator is null!");  
        }  

        nextAttackTime = Time.time + cooldownTime;  

        if (lightAttackCollider != null)  
        {  
            StartCoroutine(EnableColliderForDuration(lightAttackCollider, 0.2f));  
            Debug.Log("Light attack collider enabled");  
        }  
        else  
        {  
            Debug.LogError("Light Attack Collider is not assigned!");  
        }  

        CheckPlayerBlocking();  
    }  

    void HeavyAttack()  
    {  
        if (isDead) return;  

        isAttacking = true; 
        // 触发重攻击动画  
        animator.SetBool("isWalking", false);  
        animator.SetTrigger("heavyAttack");
        //设置下一次重攻击的时间    
        nextHeavyAttackTime = Time.time + heavyAttackCooldown;
        // 检查玩家是否格挡了重攻击  
        CheckPlayerBlockingHeavy();  
    }  

    void CheckPlayerBlocking()  
    {  
        // 检查玩家是否正在格挡 
        if (playerCombat == null)  
        {  
            Debug.LogError("playerCombat is null!");  
            return;  
        }  

        bool isBlocking = playerCombat.IsBlocking();  
        Debug.Log($"Player blocking status: {isBlocking}");  
        ApplyDamageToPlayer(isBlocking, normalAttackDamage, false);  
    }  

    void CheckPlayerBlockingHeavy()  
    {  
        // 检查玩家是否正在格挡 
        bool isBlocking = playerCombat != null && playerCombat.IsBlocking();  
        ApplyDamageToPlayer(isBlocking, heavyAttackDamage, true);  
    }  

    void ApplyDamageToPlayer(bool isBlocking, int damage, bool isHeavyAttack)  
    {  
        if (isBlocking)  
        {  
            Debug.Log("Player blocked the attack!");  
            if (playerCombat != null)  
            {  
                // 如果是重攻击,则对玩家施加击退效果  
                if (isHeavyAttack)  
                {  
                    // Heavy attack: apply knockback  
                    ApplyKnockbackToPlayer();  
                }  
                else  
                {  
                    // Light attack: stun the mushroom  
                    StartCoroutine(Stun());  
                }  
                playerCombat.TriggerBlockImpact();  
            }  
        }  
        else  
        {  
            if (playerHealth != null)  
            {  
                playerHealth.Takedamage(damage);  
                Debug.Log($"Damage applied to player: {damage}");  
            }  
            else  
            {  
                Debug.LogError("playerHealth is null!");  
            }  
        }  
        isAttacking = false;  
    }
    void ApplyKnockbackToPlayer()  
    {  
        if (playerCombat != null && playerCombat.GetComponent<Rigidbody2D>() != null)  
        {  
            Vector2 knockbackDirection = (playerCombat.transform.position - transform.position).normalized;  
            playerCombat.GetComponent<Rigidbody2D>().AddForce(knockbackDirection * heavyAttackKnockbackForce, ForceMode2D.Impulse);  
            Debug.Log("Knockback applied to player");  
        }  
        else  
        {  
            Debug.LogError("Player Rigidbody2D not found!");  
        }  
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
                return;  
            }  

            currentHealth -= damage;  
            mushroomHealth.TakeDamage(damage);  

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
        StartCoroutine(TriggerDeathAnimation());  
        //StartCoroutine(DestroyAfterAnimation());  
    } 

    IEnumerator TriggerDeathAnimation()
    {
        yield return new WaitForEndOfFrame();
        animator.SetTrigger("Die");

        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }  

    IEnumerator EnableColliderForDuration(Collider2D collider, float duration)  
    {  
        collider.enabled = true;  
        yield return new WaitForSeconds(duration);  
        collider.enabled = false;  
    }  

    public void OnAttackFinished()  
    {  
        isAttacking = false;  
    }  
}