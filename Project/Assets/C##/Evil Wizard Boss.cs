using UnityEngine;
using System.Collections;

public enum BossState
{
    Idle,
    Walking,
    Jumping,
    Falling,
    LightAttack,
    HeavyAttack,
    Hurt,
    Dying
}

public class EvilWizardBoss : MonoBehaviour
{
    [Header("Boss Stats")]
    public int health = 100;
    public float attackRange = 5f;
    public float heavyAttackThreshold = 30f;
    public float fallThreshold = -5f;
    public float walkSpeed = 3f;

    [Header("References")]
    public Transform player;

    [Header("Combat Settings")]
    public float lightAttackCooldown = 2f;
    public float heavyAttackCooldown = 4f;
    public float jumpForce = 10f;
    public float jumpCooldown = 3f;
    public float lightAttackDamage = 10f;
    public float heavyAttackDamage = 20f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask playerLayer;

    private BossState currentState;
    private Animator animator;
    private Rigidbody2D rb2D;
    private bool canAttack = true;
    private bool canJump = true;
    private bool facingRight = true;
    private bool isGrounded;
    private PlayerCombat playerCombat;

    private void Start()
    {
        currentState = BossState.Idle;
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();

        if (animator == null || rb2D == null)
        {
            Debug.LogError("EvilWizardBoss is missing required components!");
            enabled = false;
            return;
        }

        playerCombat = player.GetComponent<PlayerCombat>();
        if (playerCombat == null)
        {
            Debug.LogError("PlayerCombat component not found on the player!");
        }
    }

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, playerLayer);

        UpdateAnimatorParameters();
        CheckForStateTransitions();
        HandleCurrentState();
    }

    private void UpdateAnimatorParameters()
    {
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetFloat("Speed", rb2D.velocity.magnitude);
        animator.SetFloat("VerticalSpeed", rb2D.velocity.y);
        animator.SetBool("IsJumping", !isGrounded && rb2D.velocity.y > 0);
        animator.SetBool("IsFalling", !isGrounded && rb2D.velocity.y < 0);
    }

    private void CheckForStateTransitions()
    {
        if (health <= 0)
        {
            TransitionToState(BossState.Dying);
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (!isGrounded && rb2D.velocity.y < fallThreshold && currentState != BossState.Falling)
        {
            TransitionToState(BossState.Falling);
        }
        else if (isGrounded && canAttack && distanceToPlayer < attackRange)
        {
            if (health < heavyAttackThreshold)
            {
                TransitionToState(BossState.HeavyAttack);
            }
            else
            {
                TransitionToState(BossState.LightAttack);
            }
        }
        else if (isGrounded && canJump && ShouldJump())
        {
            TransitionToState(BossState.Jumping);
        }
        else if (distanceToPlayer > attackRange && currentState != BossState.Walking)
        {
            TransitionToState(BossState.Walking);
        }
        else if (isGrounded && rb2D.velocity.magnitude < 0.1f && currentState != BossState.Idle)
        {
            TransitionToState(BossState.Idle);
        }
    }

    private bool ShouldJump()
    {
        float rayDistance = 4.0f;
        Vector2 rayOrigin = new Vector2(transform.position.x, transform.position.y - 0.5f);
        Vector2 rayDirection = facingRight ? Vector2.right : Vector2.left;

        RaycastHit2D groundAhead = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance, playerLayer);

        Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.red);

        return groundAhead.collider == null && isGrounded;
    }

    private void HandleCurrentState()
    {
        switch (currentState)
        {
            case BossState.Idle:
                HandleIdleState();
                break;
            case BossState.Walking:
                HandleWalkingState();
                break;
            case BossState.Jumping:
                HandleJumpingState();
                break;
            case BossState.Falling:
                HandleFallingState();
                break;
            case BossState.LightAttack:
                HandleLightAttackState();
                break;
            case BossState.HeavyAttack:
                HandleHeavyAttackState();
                break;
            case BossState.Hurt:
                HandleHurtState();
                break;
            case BossState.Dying:
                HandleDyingState();
                break;
        }
    }

    private void HandleJumpingState()  
    {  
        if (canJump && isGrounded)  
        {  
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);  
            animator.SetTrigger("Jump");  
            StartCoroutine(JumpCooldown());  
        }  
        else if (!isGrounded && rb2D.velocity.y > 0)  
        {  
            animator.SetBool("IsJumping", true);  
        }  
        else  
        {  
            animator.SetBool("IsJumping", false);  
            TransitionToState(BossState.Falling);  
        }  
    }

    private void HandleFallingState()
    {
        if (isGrounded)
        {
            animator.ResetTrigger("Fall");
            TransitionToState(BossState.Idle);
        }
        else
        {
            animator.SetBool("IsFalling", true);
        }
    }

    private void TransitionToState(BossState newState)
    {
        if (currentState == newState) return;

        currentState = newState;
        animator.ResetTrigger("Walk");
        animator.ResetTrigger("Jump");
        animator.ResetTrigger("Fall");
        animator.ResetTrigger("LightAttack");
        animator.ResetTrigger("HeavyAttack");
        animator.ResetTrigger("Hurt");
        animator.ResetTrigger("Die");

        switch (newState)
        {
            case BossState.Walking:
                animator.SetTrigger("Walk");
                break;
            case BossState.Jumping:
                animator.SetTrigger("Jump");
                break;
            case BossState.Falling:
                animator.SetTrigger("Fall");
                break;
            case BossState.LightAttack:
                animator.SetTrigger("LightAttack");
                break;
            case BossState.HeavyAttack:
                animator.SetTrigger("HeavyAttack");
                break;
            case BossState.Hurt:
                animator.SetTrigger("Hurt");
                break;
            case BossState.Dying:
                animator.SetTrigger("Die");
                break;
        }
    }

    private void HandleIdleState()
    {
        rb2D.velocity = Vector2.zero;
    }

    private void HandleWalkingState()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb2D.velocity = new Vector2(direction.x * walkSpeed, rb2D.velocity.y);

        if (direction.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (direction.x < 0 && facingRight)
        {
            Flip();
        }
    }

    private void HandleLightAttackState()
    {
        if (canAttack)
        {
            Debug.Log("Performing Light Attack");
            AttackPlayer(lightAttackDamage);
            StartCoroutine(AttackCooldown(lightAttackCooldown));
        }
    }

    private void HandleHeavyAttackState()
    {
        if (canAttack)
        {
            Debug.Log("Performing Heavy Attack");
            AttackPlayer(heavyAttackDamage);
            StartCoroutine(AttackCooldown(heavyAttackCooldown));
        }
    }

    private void HandleHurtState()
    {
        // Handle hurt behavior
    }

    private void HandleDyingState()
    {
        Destroy(gameObject, 2f);
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health > 0)
        {
            TransitionToState(BossState.Hurt);
        }
        else
        {
            TransitionToState(BossState.Dying);
        }
    }

    private void AttackPlayer(float damage)
    {
        if (playerCombat != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            if (distanceToPlayer <= attackRange)
            {
                playerCombat.OnEnemyAttack(damage);
            }
        }
    }

    private IEnumerator AttackCooldown(float cooldownTime)
    {
        canAttack = false;
        yield return new WaitForSeconds(cooldownTime);
        canAttack = true;
    }

    private IEnumerator JumpCooldown()
    {
        canJump = false;
        yield return new WaitForSeconds(jumpCooldown);
        canJump = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
        Gizmos.color = Color.blue;
        Vector2 rayOrigin = new Vector2(transform.position.x, transform.position.y - 0.5f);
        Vector2 rayDirection = facingRight ? Vector2.right : Vector2.left;
        Gizmos.DrawLine(rayOrigin, rayOrigin + rayDirection * 5f);
    }

    
}