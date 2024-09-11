using UnityEngine;  
using System.Collections;  

// 定义Boss的状态  
public enum BossState  
{  
    Idle, // 待机  
    Walking, // 行走  
    Jumping, // 跳跃  
    Falling, // 下落  
    LightAttack, // 轻攻击  
    HeavyAttack, // 重攻击  
    Hurt, // 受伤  
    Dying // 死亡  
}  

public class EvilWizardBoss : MonoBehaviour  
{  
    [Header("Boss Stats")]  
    public int health = 100; // Boss的生命值  
    public float attackRange = 5f; // 攻击范围  
    public float heavyAttackThreshold = 30f; // 重攻击阈值  
    public float fallThreshold = -5f; // 下落阈值  
    public float walkSpeed = 3f; // 行走速度  

    [Header("References")]  
    public Transform player; // 玩家的Transform  

    [Header("Combat Settings")]  
    public float lightAttackCooldown = 2f; // 轻攻击冷却时间  
    public float heavyAttackCooldown = 4f; // 重攻击冷却时间  
    public float jumpForce = 10f; // 跳跃力度  
    public float jumpCooldown = 3f; // 跳跃冷却时间  
    public float lightAttackDamage = 10f; // 轻攻击伤害  
    public float heavyAttackDamage = 20f; // 重攻击伤害  

    [Header("Ground Check")]  
    public Transform groundCheck; // 地面检测点  
    public float groundCheckRadius = 0.2f; // 地面检测半径  
    public LayerMask playerLayer; // 玩家所在的层  

    private BossState currentState; // 当前状态  
    private Animator animator; // 动画控制器  
    private Rigidbody2D rb2D; // 刚体组件  
    private bool canAttack = true; // 是否可以攻击  
    private bool canJump = true; // 是否可以跳跃  
    private bool facingRight = true; // 朝向  
    private bool isGrounded; // 是否在地面上  
    private PlayerCombat playerCombat; // 玩家的战斗组件  
    [Header("Jump Trigger Check")]  
    public LayerMask jumpTriggerLayer; 
    public GameObject jumpCheck;     

    private void Start()  
    {  
        currentState = BossState.Idle; // 初始状态为待机  
        animator = GetComponent<Animator>();  
        rb2D = GetComponent<Rigidbody2D>();  

        // 检查必要组件是否存在  
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
        // 检测是否在地面上  
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, LayerMask.GetMask("groundLayer","Enemy"));  

        // 更新动画参数  
        UpdateAnimatorParameters();  
        // 检查状态转换  
        CheckForStateTransitions();  
        // 处理当前状态  
        HandleCurrentState();  
    }  

    private void UpdateAnimatorParameters()  
    {  
        // 更新动画参数  
        animator.SetBool("IsGrounded", isGrounded);  
        animator.SetFloat("Speed", rb2D.velocity.magnitude);  
        animator.SetFloat("VerticalSpeed", rb2D.velocity.y);  
    }  

    private void CheckForStateTransitions()  
    {  
        // 检查状态转换条件  
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
        if (jumpCheck == null || jumpCheck.GetComponent<BoxCollider2D>() == null)  
        {  
            Debug.LogError("jumpCheck game object or BoxCollider2D component is missing!");  
            return false;  
        }  

        Collider2D jumpTrigger = Physics2D.OverlapBox(jumpCheck.transform.position, jumpCheck.GetComponent<BoxCollider2D>().size, 0f, LayerMask.GetMask("Isonjump"));  
        RaycastHit2D ground = Physics2D.Raycast(transform.position, Vector2.down, groundCheckRadius, LayerMask.GetMask("groundLayer", "Enemy"));  

        Debug.DrawLine(transform.position, transform.position + Vector3.down * groundCheckRadius, Color.green);  

        return jumpTrigger != null && ground.collider != null;  
    }
            private void HandleCurrentState()  
    {  
        // 根据当前状态处理相应的逻辑  
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
        // 处理跳跃状态  
        if (canJump && isGrounded)  
        {  
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);  
            animator.SetTrigger("Jump");  
            animator.SetBool("IsJumping", true);  
            animator.SetBool("IsGrounded", false);  
            StartCoroutine(JumpCooldown());  
        }  
        else if (!isGrounded && rb2D.velocity.y <= 0)  
        {  
            TransitionToState(BossState.Falling);  
        }  
    }  

    private void HandleFallingState()  
    {  
        // 处理下落状态  
        animator.SetBool("IsFalling", true);  
        animator.SetBool("IsJumping", false);  

        if (isGrounded)  
        {  
            animator.SetBool("IsJumping", false);  
            animator.SetBool("IsFalling", false);  
            StartCoroutine(FallToIdleTransition());  
        }  
    }  

    private IEnumerator FallToIdleTransition()  
    {  
        // 从下落状态过渡到待机状态  
        yield return new WaitForSeconds(0.2f); // 调整延迟时间  
        TransitionToState(BossState.Idle);  
    }  

    private void TransitionToState(BossState newState)  
    {  
        // 转换到新的状态  
        if (currentState == newState) return;  

        currentState = newState;  

        // 重置动画触发器  
        animator.ResetTrigger("Walk");  
        animator.ResetTrigger("Jump");  
        animator.ResetTrigger("Fall");  
        animator.ResetTrigger("LightAttack");  
        animator.ResetTrigger("HeavyAttack");  
        animator.ResetTrigger("Hurt");  
        animator.ResetTrigger("Die");  
        animator.SetBool("IsJumping", false);  
        animator.SetBool("IsFalling", false);  

        // 根据新状态设置动画触发器  
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
        // 处理待机状态  
        rb2D.velocity = Vector2.zero;  
    }  

    private void HandleWalkingState()  
    {  
        // 处理行走状态  
        Vector2 direction = (player.position - transform.position).normalized;  
        rb2D.velocity = new Vector2(direction.x * walkSpeed, rb2D.velocity.y);  

        // 根据移动方向翻转Boss的朝向  
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
        // 处理轻攻击状态  
        if (canAttack)  
        {  
            Debug.Log("Performing Light Attack");  
            AttackPlayer(lightAttackDamage);  
            StartCoroutine(AttackCooldown(lightAttackCooldown));  
        }  
    }  

    private void HandleHeavyAttackState()  
    {  
        // 处理重攻击状态  
        if (canAttack)  
        {  
            Debug.Log("Performing Heavy Attack");  
            AttackPlayer(heavyAttackDamage);  
            StartCoroutine(AttackCooldown(heavyAttackCooldown));  
        }  
    }  

    private void HandleHurtState()  
    {  
        // 处理受伤状态  
        // 在这里添加受伤状态的逻辑  
    }  

    private void HandleDyingState()  
    {   
        // 处理死亡状态  
        Destroy(gameObject, 2f);  
    }  

    private void Flip()  
    {  
        // 翻转Boss的朝向  
        facingRight = !facingRight;  
        Vector3 scale = transform.localScale;  
        scale.x *= -1;  
        transform.localScale = scale;  
    }  

    public void TakeDamage(int damage)  
    {  
        // 受到伤害  
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
        // 攻击玩家  
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
        // 攻击冷却  
        canAttack = false;  
        yield return new WaitForSeconds(cooldownTime);  
        canAttack = true;  
    }  

    private IEnumerator JumpCooldown()  
    {  
        // 跳跃冷却  
        canJump = false;  
        yield return new WaitForSeconds(jumpCooldown);  
        canJump = true;  
    }  

    private void OnDrawGizmosSelected()  
    {  
        // 在编辑器中绘制调试图形  
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