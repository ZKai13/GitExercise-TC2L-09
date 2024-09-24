using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float attackRange = 1.5f;
    public float attackCooldown = 2f;
    public int attackDamage = 10;
    public float stunDuration = 1.5f;
    public int maxHealth = 100;
    public float hurtAnimationDuration = 0.5f;
    public float chaseRange = 5f;  // 新增的追击范围

    private Transform player;
    private PlayerCombat playerCombat;
    private Rigidbody2D rb;
    private Animator animator;
    private bool canAttack = true;
    private bool isStunned = false;
    private Vector3 originalScale;
    private int currentHealth;
    private bool isHurt = false;
    public bool canPerformHeavyAttack = false;
    public int heavyAttackDamage = 20;
    private PopUp popUp;

    void Start()
    {
        popUp = FindObjectOfType<PopUp>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerCombat = player.GetComponent<PlayerCombat>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        originalScale = transform.localScale;
        currentHealth = maxHealth;

        if (player == null || playerCombat == null)
        {
            Debug.LogError("Player or PlayerCombat component not found!");
        }
    }

    void Update()
    {
        if (isStunned || isHurt)
            return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // 只有玩家在追击范围内，敌人才会开始移动
        if (distanceToPlayer <= chaseRange)
        {
            if (distanceToPlayer <= attackRange && canAttack)
            {
                Attack();
            }
            else
            {
                Move();
            }

            // 更新动画参数
            animator.SetBool("isWalking", rb.velocity.magnitude > 0.1f);
        }
        else
        {
            rb.velocity = Vector2.zero;  // 玩家在追击范围外，停止移动
            animator.SetBool("isWalking", false);  // 停止行走动画
        }
    }

    void Move()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y); // 只在水平方向移动

        // 翻转角色精灵
        if (direction.x < 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
        else if (direction.x > 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
    }

void Attack()  
{  
    rb.velocity = Vector2.zero; // Stop moving when attacking  
    animator.SetTrigger("attack"); // Trigger attack animation  

    // Check if player is blocking before applying damage  
    if (playerCombat != null)  
    {  
        bool isHeavyAttack = canPerformHeavyAttack && Random.value > 0.7f; // 30% chance for heavy attack  
        int damageToApply = isHeavyAttack ? heavyAttackDamage : attackDamage;  

        // Check if player is blocking  
        if (!playerCombat.IsBlocking())  
        {  
            playerCombat.Takedamage(damageToApply, isHeavyAttack); // Apply damage if not blocking  
        }  
        else  
        {  
            Debug.Log("Player is blocking, no damage taken."); 
            StartCoroutine(Stun());
        }  
    }  
    else  
    {  
        Debug.LogError("PlayerCombat component not found!");  
    }  

    StartCoroutine(AttackCooldown()); // Start attack cooldown  
}

    IEnumerator Stun()
    {
        yield return new WaitForSeconds(1f);
        GetStunned();
    }

    IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    public void GetStunned()
    {
        if (isStunned)
            return;

        isStunned = true;
        rb.velocity = Vector2.zero;
        animator.SetTrigger("Stunned");

        StartCoroutine(StunCooldown());
    }

    IEnumerator StunCooldown()
    {
        yield return new WaitForSeconds(stunDuration);
        isStunned = false;
    }

    public void Takedamage(int damage, bool isHeavyAttack)
    {
        if (isHurt) return; // 防止在受伤时再次受伤

        currentHealth -= damage;
        Debug.Log($"Enemy {gameObject.name} took {damage} damage. Current health: {currentHealth}");

        isHurt = true;
        rb.velocity = Vector2.zero; // 受伤时停止移动
        animator.SetTrigger("Hurt");
        ApplyKnockback(isHeavyAttack);  // 传递是否是重击的参数
        StartCoroutine(ResetHurtState());

        if (currentHealth <= 0)
        {
            Die();
            AchievementsGoblin();
            PlayerPrefs.SetInt("HeroKnight", PlayerPrefs.GetInt("HeroKnight", 0) + 1);
            StartCoroutine(TriggerDeathAnimation());
        }
    }

    void ApplyKnockback(bool isHeavyAttack)
    {
        Vector2 knockbackDirection = (transform.position - player.position).normalized; // 获取远离玩家的方向
        float knockbackForce = isHeavyAttack ? 500f : 300f; // 根据是否是重击调整击退力
        rb.AddForce(knockbackDirection * knockbackForce); // 应用击退力
    }

    IEnumerator TriggerDeathAnimation()
    {
        yield return new WaitForEndOfFrame();
        animator.SetTrigger("Die");
    }

    IEnumerator ResetHurtState()
    {
        yield return new WaitForSeconds(hurtAnimationDuration);
        isHurt = false;
    }

    void Die()
    {
        Debug.Log($"Enemy {gameObject.name} died!");

        // 禁用组件
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;

        // 在动画结束后销毁敌人对象
        StartCoroutine(DestroyAfterAnimation());
    }

    void AchievementsGoblin()
    {
        if (!PlayerPrefs.HasKey("TheGoblin"))
        {
            if (PlayerPrefs.GetInt("TheGoblin", 0) == 1 && popUp != null) // 解锁成就
                PlayerPrefs.Save(); // 确保更改已保存
            Debug.Log("Achievement Unlocked: The Goblin");
            popUp.DisplayAchievement(popUp.goblinSprite);
        }
    }

    IEnumerator DestroyAfterAnimation()
    {
        // 等待死亡动画结束
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    // 可选：在场景视图中显示敌人的血量
    void OnDrawGizmosSelected()
    {
        Vector3 position = transform.position + Vector3.up * 2f;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(position, new Vector3(1f, 0.1f, 0f));
        if (Application.isPlaying)
        {
            float healthPercentage = (float)currentHealth / maxHealth;
            Gizmos.color = Color.green;
            Gizmos.DrawCube(position - new Vector3((1f - healthPercentage) * 0.5f, 0, 0),
                            new Vector3(healthPercentage, 0.1f, 0f));
        }
    }
}
