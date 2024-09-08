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

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private BossState currentState;
    private Animator animator;
    private Rigidbody2D rb2D;
    private bool canAttack = true;
    private bool canJump = true;
    private bool facingRight = true;
    private bool isGrounded;

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
    }

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        UpdateAnimatorParameters();
        CheckForStateTransitions();
        HandleCurrentState();
    }

    private void UpdateAnimatorParameters()
    {
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetFloat("Speed", rb2D.velocity.magnitude);
        animator.SetFloat("VerticalSpeed", rb2D.velocity.y);
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
        // Implement jump decision logic here
        // For example, jump if there's an obstacle between the boss and the player
        return false;
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

    private void HandleJumpingState()
    {
        if (canJump && isGrounded)
        {
            rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            StartCoroutine(JumpCooldown());
        }
    }

    private void HandleFallingState()
    {
        if (isGrounded)
        {
            if (Vector2.Distance(transform.position, player.position) > attackRange)
            {
                TransitionToState(BossState.Walking);
            }
            else
            {
                TransitionToState(BossState.Idle);
            }
        }
    }

    private void HandleLightAttackState()
    {
        if (canAttack)
        {
            // Perform light attack
            Debug.Log("Performing Light Attack");
            StartCoroutine(AttackCooldown(lightAttackCooldown));
        }
    }

    private void HandleHeavyAttackState()
    {
        if (canAttack)
        {
            // Perform heavy attack
            Debug.Log("Performing Heavy Attack");
            StartCoroutine(AttackCooldown(heavyAttackCooldown));
        }
    }

    private void HandleHurtState()
    {
        // Handle hurt behavior
    }

    private void HandleDyingState()
    {
        // Handle dying behavior
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
    }
}