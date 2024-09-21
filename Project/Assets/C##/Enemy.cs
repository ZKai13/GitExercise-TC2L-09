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

    

    void Start()  
    {  
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

        if (distanceToPlayer <= attackRange && canAttack)  
        {  
            Attack();  
        }  
        else  
        {  
            Move();  
        }  

        // Update animation parameter  
        animator.SetBool("isWalking", rb.velocity.magnitude > 0.1f);  
    }  

    void Move()  
    {  
        Vector2 direction = (player.position - transform.position).normalized;  
        rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y); // Only move horizontally

        // Flip the sprite if moving left  
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
        // Stop moving  
        rb.velocity = Vector2.zero;  

        // Trigger attack animation  
        animator.SetTrigger("attack");  

        // Attempt to damage the player    
        if (playerCombat != null)  
        {  
            bool isHeavyAttack = canPerformHeavyAttack && Random.value > 0.7f; // 30% chance for heavy attack if capable  
            int damageToApply = isHeavyAttack ? heavyAttackDamage : attackDamage;  
            playerCombat.Takedamage(damageToApply, isHeavyAttack); // Update to include the heavy attack parameter
        }  
        else  
        {  
            Debug.LogError("PlayerCombat component not found!");  
        }  

        // Start attack cooldown  
        StartCoroutine(AttackCooldown());  
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

    public void Takedamage(int damage, bool isHeavyAttack)  // Only takes damage as an integer
    {  
        if (isHurt) return; // Prevent taking damage while already hurt  

        currentHealth -= damage;  
        Debug.Log($"Enemy {gameObject.name} took {damage} damage. Current health: {currentHealth}");  

        isHurt = true;  
        rb.velocity = Vector2.zero; // Stop moving when hurt  
        animator.SetTrigger("Hurt");  
        StartCoroutine(ResetHurtState());  

        if (currentHealth <= 0)  
        {  
            Die();  
        }  
    }  

    IEnumerator ResetHurtState()  
    {  
        yield return new WaitForSeconds(hurtAnimationDuration);  
        isHurt = false;  
    }  

    void Die()  
    {  
        Debug.Log($"Enemy {gameObject.name} died!");  
        animator.SetTrigger("Die");  

        // Disable components  
        GetComponent<Collider2D>().enabled = false;  
        this.enabled = false;  
        rb.velocity = Vector2.zero;  
        rb.isKinematic = true;  

        // Destroy the enemy object after animation  
        StartCoroutine(DestroyAfterAnimation());  
    }  

    IEnumerator DestroyAfterAnimation()  
    {  
        // Wait for the death animation to finish  
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);  
        Destroy(gameObject);  
    }  

    // Optional: Visual representation of the enemy's health in the scene view  
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
