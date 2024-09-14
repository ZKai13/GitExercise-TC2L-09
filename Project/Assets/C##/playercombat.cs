using System.Collections;  
using System.Collections.Generic;  
using UnityEngine;  

public class PlayerCombat : MonoBehaviour  
{  
    // 公共变量  
    public Animator animator; // 动画控制器  
    public Transform attackPoint; // 攻击点  
    public float attackRange = 2.5f; // 攻击范围  
    public LayerMask enemyLayers; // 敌人所在的层  
    public int lightAttackDamage = 20; // 轻攻击伤害  
    public int heavyAttackDamage = 40; // 重攻击伤害  
    public float heavyAttackThreshold = 1f; // 重攻击按键按下时间阈值  
    public float attackAnimationDuration = 0.5f; // 攻击动画持续时间  

    public float lightAttackStaminaCost = 10f; // 轻攻击消耗的体力  
    public float heavyAttackStaminaCost = 25f; // 重攻击消耗的体力  
    public float blockStaminaCost = 15f; // 格挡消耗的体力  

    public Staminasystem staminaSystem; // 体力系统  
    public Health healthSystem; // 生命值系统  

    public float blockDuration = 2f; // 格挡持续时间  
    public float blockCooldown = 1f; // 格挡冷却时间  
    public float successfulBlockDuration = 0.5f; // 成功格挡后的持续时间  

    // 私有变量  
    private float holdTime = 0f; // 按键按下时间  
    public bool isAttacking = false; // 是否正在攻击  
    private bool attackButtonHeld = false; // 攻击按键是否被按住  
    private bool isBlocking = false; // 是否正在格挡  
    private bool canBlockImpact = true; // 是否可以格挡攻击  
    public float lightAttackPushForce = 3f; // 轻攻击击退力度  
    public float heavyAttackPushForce = 5f; // 重攻击击退力度  
    private bool isMoving = false; // 是否正在移动  

    void Start()  
    {  
        animator = GetComponent<Animator>();  
        staminaSystem = GetComponent<Staminasystem>();  
        healthSystem = GetComponent<Health>();  

        // 检查必要组件是否存在  
        if (staminaSystem == null)  
        {  
            Debug.LogError("Staminasystem not found on this GameObject!");  
        }  
        if (healthSystem == null)  
        {  
            Debug.LogError("Health component not found on this GameObject!");  
        }  
    }  

    void Update()  
    {  
        if (!isAttacking)  
        {  
            // 处理移动  
            float moveHorizontal = Input.GetAxisRaw("Horizontal");  
            float moveVertical = Input.GetAxisRaw("Vertical");  
            // 在这里添加移动逻辑  
        }  
        HandleAttackInput(); // 处理攻击输入  
        HandleBlockInput(); // 处理格挡输入  

        // 根据玩家输入设置isMoving  
        isMoving = (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && !isAttacking;  
    }  

    public void OnEnemyAttack(float damage)  
    {  
        // 处理敌人的攻击  
        // 例如，可以减少玩家的生命值或触发格挡/闪避动画  
        healthSystem.Takedamage((int)damage);  
    }  

    public void OnAttackStart()  
    {  
        isAttacking = true;  
        animator.SetBool("isAttacking", true);  
        animator.SetTrigger("attack");  
    }  

    public void OnAttackEnd()  
    {  
        isAttacking = false;  
        animator.SetBool("isAttacking", false);
    }  

    void HandleAttackInput()  
    {  
        // 处理攻击输入  
        if (Input.GetKeyDown(KeyCode.Q) && !isMoving)  
        {  
            if (!isAttacking)  
            {  
                holdTime = 0f;  
                isAttacking = true;  
                attackButtonHeld = true;  
                isMoving = false;  
                animator.SetBool("isAttacking", true);  
                animator.SetTrigger("attack");  
                OnAttackStart();  
            }  
        }  

        if (attackButtonHeld)  
        {  
            holdTime += Time.deltaTime;  
        }  

        if (Input.GetKeyUp(KeyCode.Q))  
        {  
            if (isAttacking)  
            {  
                PerformAttack(holdTime >= heavyAttackThreshold);  
                attackButtonHeld = false;  
                holdTime = 0f;  
                isMoving = true;  
                OnAttackEnd();  
            }  
        }  
    }  

    void PerformAttack(bool isHeavyAttack)  
    { 
        isAttacking = true;
        Debug.Log("Performing Attack: " + (isHeavyAttack ? "Heavy" : "Light"));  
        if (isHeavyAttack && staminaSystem.ConsumeStamina(heavyAttackStaminaCost))  
        { 
            animator.SetBool("isHeavyAttack", true);
            animator.SetTrigger("heavyAttack");  
            HeavyAttack();  
        }  
        else if (staminaSystem.ConsumeStamina(lightAttackStaminaCost))  
        {  
            animator.SetBool("isLightAttack", true);
            animator.SetTrigger("lightAttack");  
            LightAttack();  
        }  
        animator.SetBool("isAttacking", true);  
        StartCoroutine(ResetAttackState());  
    }  

    void LightAttack()  
    {  
        // 执行轻攻击逻辑  
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);  
        foreach (Collider2D enemy in hitEnemies)  
        {  
            enemy.GetComponent<Enemy>().Takedamage(lightAttackDamage, isBlocking);  
            enemy.GetComponent<Rigidbody2D>().AddForce((enemy.transform.position - transform.position).normalized * lightAttackPushForce, ForceMode2D.Impulse);  
        }  
    }  

    void HeavyAttack()  
    {  
        // 执行重攻击逻辑  
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);  
        foreach (Collider2D enemy in hitEnemies)  
        {  
            enemy.GetComponent<Enemy>().Takedamage(heavyAttackDamage, isBlocking);  
            enemy.GetComponent<Rigidbody2D>().AddForce((enemy.transform.position - transform.position).normalized * heavyAttackPushForce, ForceMode2D.Impulse);  
        }  
    }  

    IEnumerator ResetAttackState()  
    {  
        // 等待攻击动画结束  
        yield return new WaitForSeconds(attackAnimationDuration);  
        animator.SetBool("isAttacking", false);  
        animator.SetBool("isHeavyAttack", false);
        animator.SetBool("isLightAttack", false);  
    }  

    void HandleBlockInput()  
    {  
        // 处理格挡输入  
        if (Input.GetKeyDown(KeyCode.E) && staminaSystem.ConsumeStamina(blockStaminaCost))  
        {  
            StartCoroutine(Block());  
        }  
    }  

    IEnumerator Block()  
    {  
        isBlocking = true;  
        animator.SetBool("isBlocking", true);  

        float blockTimer = 0f;  
        while (blockTimer < blockDuration && Input.GetKey(KeyCode.E))  
        {  
            blockTimer += Time.deltaTime;  
            yield return null;  
        }  

        isBlocking = false;  
        animator.SetBool("isBlocking", false);  

        yield return new WaitForSeconds(blockCooldown);  
        canBlockImpact = true;  
    }  

    public void TriggerBlockImpact()  
    {  
        // 触发格挡攻击  
        if (canBlockImpact)  
        {  
            canBlockImpact = false;  
            staminaSystem.ConsumeStamina(blockStaminaCost);  
            StartCoroutine(BlockImpactCooldown());  
        }  
    }  

    IEnumerator BlockImpactCooldown()  
    {  
        yield return new WaitForSeconds(successfulBlockDuration);  
        canBlockImpact = true;  
    }  

    public bool IsBlocking()  
    {  
        return isBlocking;  
    }  

    public void OnEnemyAttack(Enemy enemy, float damage)  
    {  
        // 处理敌人的攻击  
        if (isBlocking && canBlockImpact)  
        {  
            Debug.Log("Successful block!");  
            StartCoroutine(SuccessfulBlock());  
        }  
        else  
        {  
            if (healthSystem != null)  
            {  
                healthSystem.Takedamage(damage);  
                Debug.Log($"Player took {damage} damage. Current health: {healthSystem.currentHealth}");  
            }  
            else  
            {  
                Debug.LogError("Health component is not assigned to the player!");  
            }  
        }  
    }  

    IEnumerator SuccessfulBlock()  
    {  
        // 处理成功格挡  
        canBlockImpact = false;  
        animator.SetTrigger("BlockImpact");  

        // 播放格挡特效  
        PlayBlockEffect();  

        // 击晕附近的敌人  
        StunNearbyEnemies();  

        yield return new WaitForSeconds(successfulBlockDuration);  
        canBlockImpact = true;  
    }  

    void PlayBlockEffect()  
    {  
        // 实现格挡特效  
        // 例如:  
        // blockEffectParticleSystem.Play();  
        // audioSource.PlayOneShot(blockSoundEffect);  
    }  

    void StunNearbyEnemies()  
    {  
        // 击晕附近的敌人  
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayers);  
        foreach (Collider2D enemy in hitEnemies)  
        {  
            Enemy enemyScript = enemy.GetComponent<Enemy>();  
            if (enemyScript != null)  
            {  
                enemyScript.GetStunned();  
            }  
        }  
    }  

    void OnDrawGizmosSelected()  
    {  
        // 在编辑器中绘制攻击范围  
        if (attackPoint == null) return;  
        Gizmos.color = Color.red;  
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);  
    }  

    public void IncreaseLightAttackDamage(int amount)  
    {  
        // 增加轻攻击伤害  
        lightAttackDamage += amount;  
        Debug.Log("Light Attack Damage increased to: " + lightAttackDamage);  
    }  

    public void IncreaseHeavyAttackDamage(int amount)  
    {  
        // 增加重攻击伤害  
        heavyAttackDamage += amount;  
        Debug.Log("Heavy Attack Damage increased to: " + heavyAttackDamage);  
    }  
}