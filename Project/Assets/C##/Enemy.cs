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
//     public PlayerCombat playerCombat; 

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
//         playerCombat = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>();  

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
//         Invoke("CheckPlayerBlocking", 0.5f);  
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
//                 playerHealth.Takedamage(1);  
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

//     public void GetStunned()  
//     {  
//         if (!isStunned)  
//         {  
//             StartCoroutine(ApplyStun());  
//         }  
//     }  

//     void Die()  
//     {  
//         isDead = true;  
//         animator.SetTrigger("Die");  
//         StartCoroutine(DestroyAfterAnimation());  
//     }  

//     IEnumerator ApplyStun()  
//     {  
//         isStunned = true;  
//         animator.SetBool("Stunned", true);  
        
//         yield return new WaitForSeconds(stunDuration);  

//         isStunned = false;  
//         animator.SetBool("Stunned", false);  
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
//     public PlayerCombat playerCombat;

//     private bool isDead = false;
//     private bool isAttacking = false;
//     private bool isStunned = false;

//     public float cooldownTime = 0.75f;
//     private float nextAttackTime = 0f;

//     public int attackDamage = 1; // NEW: Added attack damage variable

//     void Start()
//     {
//         currentHealth = maxHealth;
//         target = GameObject.FindGameObjectWithTag("Player").transform;

//         animator = GetComponent<Animator>();
//         spriteRenderer = GetComponent<SpriteRenderer>();
//         playerCombat = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>();

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
//         Invoke("OnAttackHit", 0.5f); // Changed from CheckPlayerBlocking to OnAttackHit
//     }

//     // NEW: Added OnAttackHit method
//     void OnAttackHit()
//     {
//         if (playerCombat != null)
//         {
//             playerCombat.OnEnemyAttack(this, attackDamage);
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

//     public void GetStunned()
//     {
//         if (!isStunned)
//         {
//             StartCoroutine(ApplyStun());
//         }
//     }

//     void Die()
//     {
//         isDead = true;
//         animator.SetTrigger("Die");
//         StartCoroutine(DestroyAfterAnimation());
//     }

//     IEnumerator ApplyStun()
//     {
//         isStunned = true;
//         animator.SetBool("Stunned", true);
        
//         yield return new WaitForSeconds(stunDuration);

//         isStunned = false;
//         animator.SetBool("Stunned", false);
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

// using UnityEngine;
// using System.Collections;

// public class Enemy : MonoBehaviour
// {
//     public float moveSpeed = 3f;
//     public float attackRange = 1.5f;
//     public float attackCooldown = 2f;
//     public int attackDamage = 10;
//     public float stunDuration = 1.5f;

//     private Transform player;
//     private PlayerCombat playerCombat;
//     private Rigidbody2D rb;
//     private Animator animator;
//     private bool canAttack = true;
//     private bool isStunned = false;

//     void Start()
//     {
//         player = GameObject.FindGameObjectWithTag("Player").transform;
//         playerCombat = player.GetComponent<PlayerCombat>();
//         rb = GetComponent<Rigidbody2D>();
//         animator = GetComponent<Animator>();

//         if (player == null || playerCombat == null)
//         {
//             Debug.LogError("Player or PlayerCombat component not found!");
//         }
//     }

//     void Update()
//     {
//         if (isStunned)
//             return;

//         float distanceToPlayer = Vector2.Distance(transform.position, player.position);

//         if (distanceToPlayer <= attackRange && canAttack)
//         {
//             Attack();
//         }
//         else
//         {
//             Move();
//         }

//         // Update animation parameters
//         animator.SetFloat("Speed", rb.velocity.magnitude);
//     }

//     void Move()
//     {
//         Vector2 direction = (player.position - transform.position).normalized;
//         rb.velocity = direction * moveSpeed;

//         // Flip the sprite if moving left
//         if (direction.x < 0)
//         {
//             transform.localScale = new Vector3(-1, 1, 1);
//         }
//         else if (direction.x > 0)
//         {
//             transform.localScale = new Vector3(1, 1, 1);
//         }
//     }

//     void Attack()
//     {
//         // Stop moving
//         rb.velocity = Vector2.zero;

//         // Trigger attack animation
//         animator.SetTrigger("Attack");

//         // Attempt to damage the player
//         playerCombat.OnEnemyAttack(this, attackDamage);

//         // Start attack cooldown
//         StartCoroutine(AttackCooldown());
//     }

//     IEnumerator AttackCooldown()
//     {
//         canAttack = false;
//         yield return new WaitForSeconds(attackCooldown);
//         canAttack = true;
//     }

//     public void GetStunned()
//     {
//         if (isStunned)
//             return;

//         isStunned = true;
//         rb.velocity = Vector2.zero;
//         animator.SetTrigger("Stunned");

//         StartCoroutine(StunCooldown());
//     }

//     IEnumerator StunCooldown()
//     {
//         yield return new WaitForSeconds(stunDuration);
//         isStunned = false;
//     }

//     // You can add a method to handle the enemy taking damage here
//     public void TakeDamage(int damage)
//     {
//         // Implement damage logic, health reduction, etc.
//         animator.SetTrigger("Hurt");
//         // Check if the enemy should die
//         // If health <= 0, call Die() method
//     }

//     void Die()
//     {
//         // Implement death logic
//         animator.SetTrigger("Die");
//         // Disable components, play death animation, etc.
//         // Destroy the enemy object after animation or immediately
//         Destroy(gameObject);
//     }
// }

// using UnityEngine;
// using System.Collections;

// public class Enemy : MonoBehaviour
// {
//     public float moveSpeed = 3f;
//     public float attackRange = 1.5f;
//     public float attackCooldown = 2f;
//     public int attackDamage = 10;
//     public float stunDuration = 1.5f;

//     private Transform player;
//     private PlayerCombat playerCombat;
//     private Rigidbody2D rb;
//     private Animator animator;
//     private bool canAttack = true;
//     private bool isStunned = false;
//     private Vector3 originalScale;
//     public int maxHealth = 100;  
//     private int currentHealth; 
    

//     void Start()
//     {
//         player = GameObject.FindGameObjectWithTag("Player").transform;
//         playerCombat = player.GetComponent<PlayerCombat>();
//         rb = GetComponent<Rigidbody2D>();
//         animator = GetComponent<Animator>();
//         originalScale = transform.localScale;  
//         currentHealth = maxHealth; 

//         if (player == null || playerCombat == null)
//         {
//             Debug.LogError("Player or PlayerCombat component not found!");
//         }
//     }

//     void Update()
//     {
//         if (isStunned)
//             return;

//         float distanceToPlayer = Vector2.Distance(transform.position, player.position);

//         if (distanceToPlayer <= attackRange && canAttack)
//         {
//             Attack();
//         }
//         else
//         {
//             Move();
//         }

//         // Update animation parameter
//         animator.SetBool("isWalking", rb.velocity.magnitude > 0.1f);
//     }

//     void Move()
//     {
//         Vector2 direction = (player.position - transform.position).normalized;
//         rb.velocity = direction * moveSpeed;

//         // Flip the sprite if moving left
//         if (direction.x < 0)  
//         {  
//             transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);  
//         }  
//         else if (direction.x > 0)  
//         {  
//             transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);  
//         }  
//     }

//     void Attack()
//     {
//         // Stop moving
//         rb.velocity = Vector2.zero;

//         // Trigger attack animation
//         animator.SetTrigger("attack");

//         // Attempt to damage the player
//         playerCombat.OnEnemyAttack(this, attackDamage);

//         // Start attack cooldown
//         StartCoroutine(AttackCooldown());
//     }

//     IEnumerator AttackCooldown()
//     {
//         canAttack = false;
//         yield return new WaitForSeconds(attackCooldown);
//         canAttack = true;
//     }

//     public void GetStunned()
//     {
//         if (isStunned)
//             return;

//         isStunned = true;
//         rb.velocity = Vector2.zero;
//         animator.SetTrigger("Stunned");

//         StartCoroutine(StunCooldown());
//     }

//     IEnumerator StunCooldown()
//     {
//         yield return new WaitForSeconds(stunDuration);
//         isStunned = false;
//     }

//     public void Takedamage(int damage)
//     {
//         // Implement damage logic, health reduction, etc.
//         animator.SetTrigger("Hurt");
//         // Check if the enemy should die
//         // If health <= 0, call Die() method
//     }

//     void Die()
//     {
//         // Implement death logic
//         animator.SetTrigger("Die");
//         // Disable components, play death animation, etc.
//         // Destroy the enemy object after animation or immediately
//         Destroy(gameObject);
//     }
// }
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
        rb.velocity = direction * moveSpeed;  

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
            playerCombat.ReceiveAttack(this, attackDamage);  
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

    public void Takedamage(int damage)  
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

