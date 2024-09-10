using System.Collections;  
using System.Collections.Generic;  
using UnityEngine;  

public class PlayerCombat : MonoBehaviour  
{  
    // Public variables  
    public Animator animator;  
    public Transform attackPoint;  
    public float attackRange = 2.5f;  
    public LayerMask enemyLayers;  
    public int lightAttackDamage = 20;  
    public int heavyAttackDamage = 40;  
    public float heavyAttackThreshold = 1f;  
    public float attackAnimationDuration = 0.5f;  

    public float lightAttackStaminaCost = 10f;  
    public float heavyAttackStaminaCost = 25f;  
    public float blockStaminaCost = 15f;  

    public Staminasystem staminaSystem;  
    public Health healthSystem;  

    public float blockDuration = 2f;  
    public float blockCooldown = 1f;  
    public float successfulBlockDuration = 0.5f;  

    // Private variables  
    private float holdTime = 0f;  
    public bool isAttacking = false;  
    private bool attackButtonHeld = false;  
    private bool isBlocking = false;  
    private bool canBlockImpact = true;  
    public float lightAttackPushForce = 3f;  
    public float heavyAttackPushForce = 5f;  
    private bool isMoving = false;  

    void Start()  
    {  
        animator = GetComponent<Animator>();  
        staminaSystem = GetComponent<Staminasystem>();  
        healthSystem = GetComponent<Health>();  

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
            // Handle movement  
            float moveHorizontal = Input.GetAxisRaw("Horizontal");  
            float moveVertical = Input.GetAxisRaw("Vertical");  
            // Add your movement logic here  
        }  
        HandleAttackInput();  
        HandleBlockInput();  

        // Set isMoving based on player input  
        isMoving = (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && !isAttacking;  
    }  
    public void OnEnemyAttack(float damage)  
    {  
        // Add the logic to handle the enemy's attack here  
        // For example, you could reduce the player's health or trigger a block/dodge animation  
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
        Debug.Log("Performing Attack: " + (isHeavyAttack ? "Heavy" : "Light"));
        if (isHeavyAttack && staminaSystem.ConsumeStamina(heavyAttackStaminaCost))  
        {  
            animator.SetTrigger("heavyAttack");
            HeavyAttack();  
        }  
        else if (staminaSystem.ConsumeStamina(lightAttackStaminaCost))  
        {  
            animator.SetTrigger("lightAttack");       
 
            LightAttack();  
        }  
        animator.SetBool("isAttacking", true);  
        StartCoroutine(ResetAttackState());  
    }  

    void LightAttack()  
    {  
        // Perform light attack logic  
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);  
        foreach (Collider2D enemy in hitEnemies)  
        {  
            enemy.GetComponent<Enemy>().Takedamage(lightAttackDamage, isBlocking);  
            enemy.GetComponent<Rigidbody2D>().AddForce((enemy.transform.position - transform.position).normalized * lightAttackPushForce, ForceMode2D.Impulse);  
        }  
    }  

    void HeavyAttack()  
    {  
        // Perform heavy attack logic  
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);  
        foreach (Collider2D enemy in hitEnemies)  
        {  
            enemy.GetComponent<Enemy>().Takedamage(heavyAttackDamage, isBlocking);  
            enemy.GetComponent<Rigidbody2D>().AddForce((enemy.transform.position - transform.position).normalized * heavyAttackPushForce, ForceMode2D.Impulse);  
        }  
    }  
    

    IEnumerator ResetAttackState()  
    {  
        // Wait for the attack animation to finish  
        yield return new WaitForSeconds(attackAnimationDuration);  
        animator.SetBool("isAttacking", false);  
        animator.SetBool("isHeavyAttack", false);  
            
    }  

    void HandleBlockInput()  
    {  
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

    public void OnEnemyAttack(Enemy enemy,float damage)
    {
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
        canBlockImpact = false;
        animator.SetTrigger("BlockImpact");

        // Play block effect
        PlayBlockEffect();

        // Stun nearby enemies
        StunNearbyEnemies();

        yield return new WaitForSeconds(successfulBlockDuration);
        canBlockImpact = true;
    }

    void PlayBlockEffect()
    {
        // Implement your block effect here
        // For example:
        // blockEffectParticleSystem.Play();
        // audioSource.PlayOneShot(blockSoundEffect);
    }

    void StunNearbyEnemies()
    {
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
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
    public void IncreaseLightAttackDamage(int amount)
    {
        lightAttackDamage += amount;
        Debug.Log("Light Attack Damage increased to: " + lightAttackDamage);
    }

    public void IncreaseHeavyAttackDamage(int amount)
    {
       heavyAttackDamage += amount;
       Debug.Log("Heavy Attack Damage increased to: " + heavyAttackDamage);
    }
}