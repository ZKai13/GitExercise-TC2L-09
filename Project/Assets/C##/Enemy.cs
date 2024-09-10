using System.Collections;  
using System.Collections.Generic;  
using UnityEngine;  

public class Enemy : MonoBehaviour  
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
    private bool canBePushed = true;  
    private float pushForce = 3f;  
    private float pushCooldown = 1f;  

    void Start()  
    {  
        playerCombat = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>();  
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();  
        currentHealth = maxHealth;  
        target = GameObject.FindGameObjectWithTag("Player").transform; 
         

        animator = GetComponent<Animator>();  
        spriteRenderer = GetComponent<SpriteRenderer>();  
        playerCombat = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>();  

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

            if (distanceToPlayer <= 1.5f)  
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
        animator.SetBool("isWalking", false);  
        animator.SetTrigger("attack");  

        nextAttackTime = Time.time + cooldownTime;  
        Invoke("CheckPlayerBlocking", 0.5f);  
    }  

    void CheckPlayerBlocking()  
    {  
        bool isBlocking = playerCombat != null && playerCombat.IsBlocking();  
        ApplyDamageToPlayer(isBlocking);  
    }  

    void ApplyDamageToPlayer(bool isBlocking)  
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
                playerHealth.Takedamage(1);
                Debug.Log("Applying damage to player");  
                  
            }  
            else  
            {  
                Debug.LogError("playerHealth is null!");  
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
            currentHealth -= damage;  
            PushEnemy(true);  

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

    private void PushEnemy(bool shouldPush)  
    {  
        if (shouldPush && canBePushed)  
        {  
            Vector2 pushDirection = (transform.position - target.position).normalized;  
            GetComponent<Rigidbody2D>().AddForce(pushDirection * pushForce, ForceMode2D.Impulse);  
            StartCoroutine(PushCooldown());  
        }  
    }  

    private IEnumerator PushCooldown()  
    {  
        canBePushed = false;  
        yield return new WaitForSeconds(pushCooldown);  
        canBePushed = true;  
    }  

    IEnumerator Stun()  
    {  
        isStunned = true;  
        animator.SetTrigger("Stunned");  
        yield return new WaitForSeconds(stunDuration);  
        isStunned = false;  
    }  

    public void GetStunned()  
    {  
        if (!isStunned)  
        {  
            StartCoroutine(ApplyStun());  
        }  
    }  

    void Die()  
    {  
        isDead = true;  
        animator.SetTrigger("Die");  
        StartCoroutine(DestroyAfterAnimation());  
    }  

    IEnumerator ApplyStun()  
    {  
        isStunned = true;  
        animator.SetBool("Stunned", true);  

        yield return new WaitForSeconds(stunDuration);  

        isStunned = false;  
        animator.SetBool("Stunned", false);  
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
                Destroy(gameObject, 1f);  
            }  
        }  
    }  

    public void OnAttackFinished()  
    {  
        isAttacking = false;  
    }  

    private void OnCollisionEnter2D(Collision2D collision)  
    {  
        // 检查碰撞对象是否为你想要的碰撞体  
        if (collision.gameObject.CompareTag("DestroyCollider"))  
        {  
            Destroy(gameObject); // 销毁敌人  
        }  
    }  
}