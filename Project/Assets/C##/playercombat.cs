using UnityEngine;
using System.Collections;

public class PlayerCombat : MonoBehaviour
{
    // Public variables
    public Animator animator; // Animation controller
    public Transform attackPoint; // Attack point
    public float attackRange = 2.5f; // Attack range
    public LayerMask enemyLayers; // Enemy layers
    public int lightAttackDamage = 20; // Light attack damage
    public int heavyAttackDamage = 40; // Heavy attack damage
    public float heavyAttackThreshold = 1f; // Heavy attack hold time threshold
    public float attackAnimationDuration = 0.5f; // Attack animation duration

    public float lightAttackStaminaCost = 10f; // Light attack stamina cost
    public float heavyAttackStaminaCost = 25f; // Heavy attack stamina cost
    public float blockStaminaCost = 15f; // Block stamina cost

    public Staminasystem staminaSystem; // Stamina system
    public Health healthSystem; // Health system

    public float blockDuration = 2f; // Block duration
    public float blockCooldown = 1f; // Block cooldown
    public float successfulBlockDuration = 0.5f; // Successful block duration

    // Private variables
    private float holdTime = 0f; // Key hold time
    public bool isAttacking = false; // Is attacking
    private bool attackButtonHeld = false; // Is attack button held
    private bool isBlocking = false; // Is blocking
    private bool canBlockImpact = true; // Can block impact
    public float lightAttackPushForce = 3f; // Light attack push force
    public float heavyAttackPushForce = 5f; // Heavy attack push force
    private bool isMoving = false; // Is moving
    public int maxHealth = 100;  
    private int currentHealth;  
    public float hurtForce = 10f;  
    public float invincibilityDuration = 0.5f;
    private bool isStunned = false;  
    private float stunDuration = 0f;  

    private Rigidbody2D rb; 

    public EvilWizardBoss evilWizardBoss;   

 
    void Start()
    {
        currentHealth = maxHealth;  
        animator = GetComponent<Animator>(); 
        animator = GetComponent<Animator>();
        staminaSystem = GetComponent<Staminasystem>();
        healthSystem = GetComponent<Health>();
        rb = GetComponent<Rigidbody2D>(); 

        // Check for necessary components
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
            // Add movement logic here
        }
        if (isStunned)  
        {  
            stunDuration -= Time.deltaTime;  
            if (stunDuration <= 0)  
            {  
                isStunned = false;  
                // Optional: Add any "unstun" logic here  
            }  
            return; // Skip other updates while stunned  
        }  
        
        HandleAttackInput(); // Handle attack input
        HandleBlockInput(); // Handle block input

        // Set isMoving based on player input
        isMoving = (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && !isAttacking;
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
        // Handle attack input
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
            NewHeavyAttack();
        }
        else if (staminaSystem.ConsumeStamina(lightAttackStaminaCost))
        {
            animator.SetBool("isLightAttack", true);
            animator.SetTrigger("lightAttack");
            NewLightAttack();
        }
        animator.SetBool("isAttacking", true);
        StartCoroutine(ResetAttackState());
    }
    private void GetHurt(Vector3 attackerPosition, float damage)
    {
        healthSystem.Takedamage(damage);
        Debug.Log($"Player took {damage} damage. Current health: {healthSystem.currentHealth}");

        // Calculate direction from attacker to player
        Vector2 hurtDirection = (transform.position - attackerPosition).normalized;

        // Apply backward force
        rb.AddForce(hurtDirection * hurtForce, ForceMode2D.Impulse);

        // Trigger hurt animation
        animator.SetTrigger("Hurt");

        // Optional: Add invincibility frames
        StartCoroutine(InvincibilityFrames());
    }
    // NEW: Added InvincibilityFrames coroutine
    private IEnumerator InvincibilityFrames()
    {
        // Disable player's collider or set a flag to prevent further damage
        GetComponent<Collider2D>().enabled = false;

        // Wait for a short duration (e.g., 0.5 seconds)
        yield return new WaitForSeconds(0.5f);

        // Re-enable player's collider or reset the flag
        GetComponent<Collider2D>().enabled = true;
    }
    public void GetStunned(float duration)  
    {  
        isStunned = true;  
        stunDuration = duration;  
        animator.SetTrigger("Stunned"); // Assuming you have a "Stunned" animation  
        Debug.Log($"Boss stunned for {duration} seconds");  
    }  

    void NewLightAttack()  
    {  
        // Raycast-based attack detection  
        RaycastHit2D hit = Physics2D.Raycast(attackPoint.position, transform.right, attackRange, enemyLayers);  
        if (hit.collider != null)  
        {  
            EvilWizardBoss bossScript = hit.collider.GetComponent<EvilWizardBoss>();  
            if (bossScript != null)  
            {  
                // Apply damage reduction factor  
                float reducedDamage = lightAttackDamage * bossScript.bossDamageReductionFactor;  
                bossScript.TakeDamage((int)reducedDamage);  
                hit.rigidbody.AddForce((hit.transform.position - transform.position).normalized * lightAttackPushForce, ForceMode2D.Impulse);  
            }  
        }  
    }  

    void NewHeavyAttack()  
    {  
        // Raycast-based attack detection  
        RaycastHit2D hit = Physics2D.Raycast(attackPoint.position, transform.right, attackRange, enemyLayers);  
        if (hit.collider != null)  
        {  
            EvilWizardBoss bossScript = hit.collider.GetComponent<EvilWizardBoss>();  
            if (bossScript != null)  
            {  
                // Apply damage reduction factor  
                float reducedDamage = heavyAttackDamage * bossScript.bossDamageReductionFactor;  
                bossScript.TakeDamage((int)reducedDamage);  
                hit.rigidbody.AddForce((hit.transform.position - transform.position).normalized * heavyAttackPushForce, ForceMode2D.Impulse);  
            }  
        }  
    }  
    public void Takedamage(int damage)  
    {  
        if (healthSystem != null)  
        {  
            healthSystem.Takedamage(damage);  
            Debug.Log($"Player took {damage} damage. Current health: {healthSystem.currentHealth}");  
            
            // Trigger hurt animation  
            animator.SetTrigger("Hurt");  
        }  
        else  
        {  
            Debug.LogError("Health component is not assigned to the player!");  
        }  

        // Check if the player has died  
        if (healthSystem.currentHealth <= 0)  
        {  
            Die();  
        }  
    }  

    private void Die()  
    {  
        Debug.Log("Player died!");  
        // Trigger death animation  
        animator.SetTrigger("Die");  
        
        // Disable the player  
        GetComponent<Collider2D>().enabled = false;  
        this.enabled = false;  
    }
    

    IEnumerator ResetAttackState()
    {
    yield return new WaitForSeconds(attackAnimationDuration);  
    Debug.Log("Resetting attack state");  
    animator.SetBool("isAttacking", false);  
    animator.SetBool("isHeavyAttack", false);  
    animator.SetBool("isLightAttack", false);  
    isAttacking = false;  
    }

    void HandleBlockInput()
    {
        // Handle block input
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
        // Trigger block impact
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

    public void ReceiveAttack(MonoBehaviour attacker, float damage)  
    {  
        // Handle enemy attack  
        if (isBlocking && canBlockImpact)  
        {  
            Debug.Log("Successful block!");  
            StartCoroutine(SuccessfulBlock());  
        }  
        else  
        {  
            if (healthSystem != null)  
            {  
                // Apply damage reduction if the player is blocking  
                if (isBlocking)  
                {  
                    damage *= 0.5f; // Reduce damage by 50% when blocking  
                }  

                healthSystem.Takedamage(damage);  
                Debug.Log($"Player took {damage} damage. Current health: {healthSystem.currentHealth}");  

                // Trigger the Hurt animation  
                if (animator != null)  
                {  
                    animator.SetTrigger("Hurt");  
                }  
                else  
                {  
                    Debug.LogError("Animator component not found on the player!");  
                }  
            }  
            else  
            {  
                Debug.LogError("Health component is not assigned to the player!");  
            }  
        }  
    }  

    IEnumerator SuccessfulBlock()  
    {  
        // Handle successful block  
        canBlockImpact = false;  
        animator.SetTrigger("BlockImpact");  

        // Play block effect  
        PlayBlockEffect();  

        // Stun nearby enemies  
        StunNearbyEnemies();  

        // Stun the boss  
        if (evilWizardBoss != null)  
        {  
            evilWizardBoss.TakeDamage(0); // Deal 0 damage to trigger the boss's hurt state  
            Debug.Log("Boss stunned (simulated)"); 
        }  

        yield return new WaitForSeconds(successfulBlockDuration);  
        canBlockImpact = true;  
    }

    void PlayBlockEffect()
    {
        // Implement block effect
        // Example:
        // blockEffectParticleSystem.Play();
        // audioSource.PlayOneShot(blockSoundEffect);
    }

    void StunNearbyEnemies()
    {
        // Stun nearby enemies
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
        // Draw attack range in editor
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public void IncreaseLightAttackDamage(int amount)  
    {  
        lightAttackDamage += amount;  
        SavePlayerPrefs();   
        Debug.Log("Light Attack Damage increased to: " + lightAttackDamage);  
    }  

    public void IncreaseHeavyAttackDamage(int amount)  
    {  
        heavyAttackDamage += amount;  
        SavePlayerPrefs();   
        Debug.Log("Heavy Attack Damage increased to: " + heavyAttackDamage);  
    }  

    private void SavePlayerPrefs()  
    {  
        PlayerPrefs.SetInt("LightAttackDamage", lightAttackDamage);  
        PlayerPrefs.SetInt("HeavyAttackDamage", heavyAttackDamage);  
        PlayerPrefs.Save();  
    }  

    public void ResetAttackDamage()  
    {  
        // Reset the light and heavy attack damage to their default values  
        lightAttackDamage = 20;  
        heavyAttackDamage = 40;  
        Debug.Log("Player attack damage has been reset.");  
    }  
}



