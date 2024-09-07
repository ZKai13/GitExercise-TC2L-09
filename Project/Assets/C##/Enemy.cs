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

    private bool canAttack = true;
    private bool isStunned = false;
    private Transform player;
    private PlayerCombat playerCombat;
    private Animator animator;
    private Rigidbody2D rb;
    private Vector3 originalScale;

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
            
                if (canAttack && Vector2.Distance(transform.position, player.position) <= attackRange)
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
        rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);

        // Update animation
        animator.SetBool("isWalking", rb.velocity.magnitude > 0.1f);

        // Flip sprite if needed
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
        animator.SetTrigger("Hurt");

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

