// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class PlayerCombat : MonoBehaviour
// {
//     // Public variables
//     public Animator animator;
//     public Transform attackPoint;
//     public float attackRange = 2.5f;
//     public LayerMask enemyLayers;
//     public int lightAttackDamage = 20;
//     public int heavyAttackDamage = 40;
//     public float heavyAttackThreshold = 1f;
//     public float attackAnimationDuration = 0.5f;

//     public float lightAttackStaminaCost = 10f;
//     public float heavyAttackStaminaCost = 25f;
//     public float blockStaminaCost = 15f;

//     public Staminasystem staminaSystem;
//     public Health healthSystem;

//     public float blockDuration = 2f;
//     public float blockCooldown = 1f;
//     public float successfulBlockDuration = 0.5f;

//     public float lightAttackPushForce = 3f;
//     public float heavyAttackPushForce = 5f;

//     // NEW: Added hurt force variable
//     public float hurtForce = 10f;

//     // Private variables
//     private float holdTime = 0f;
//     public bool isAttacking = false;
//     private bool attackButtonHeld = false;
//     private bool isBlocking = false;
//     private bool canBlockImpact = true;
//     private bool isMoving = false;

//     // NEW: Added Rigidbody2D reference
//     private Rigidbody2D rb;

//     void Start()
//     {
//         animator = GetComponent<Animator>();
//         staminaSystem = GetComponent<Staminasystem>();
//         healthSystem = GetComponent<Health>();
//         // NEW: Get the Rigidbody2D component
//         rb = GetComponent<Rigidbody2D>();

//         // Check for necessary components
//         if (staminaSystem == null)
//         {
//             Debug.LogError("Staminasystem not found on this GameObject!");
//         }
//         if (healthSystem == null)
//         {
//             Debug.LogError("Health component not found on this GameObject!");
//         }
//     }

//     void Update()
//     {
//         if (!isAttacking)
//         {
//             // Handle movement
//             float moveHorizontal = Input.GetAxisRaw("Horizontal");
//             float moveVertical = Input.GetAxisRaw("Vertical");
//             // Add movement logic here
//         }
//         HandleAttackInput();
//         HandleBlockInput();

//         // Set isMoving based on player input
//         isMoving = (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && !isAttacking;
//     }

//     // NEW: Modified OnEnemyAttack method
//     public void OnEnemyAttack(Enemy enemy, float damage)
//     {
//         if (isBlocking && canBlockImpact)
//         {
//             Debug.Log("Successful block!");
//             StartCoroutine(SuccessfulBlock());
//         }
//         else
//         {
//             if (healthSystem != null)
//             {
//                 GetHurt(enemy.transform.position, damage);
//             }
//             else
//             {
//                 Debug.LogError("Health component is not assigned to the player!");
//             }
//         }
//     }

//     // NEW: Added GetHurt method
//     private void GetHurt(Vector3 attackerPosition, float damage)
//     {
//         healthSystem.Takedamage(damage);
//         Debug.Log($"Player took {damage} damage. Current health: {healthSystem.currentHealth}");

//         // Calculate direction from attacker to player
//         Vector2 hurtDirection = (transform.position - attackerPosition).normalized;

//         // Apply backward force
//         rb.AddForce(hurtDirection * hurtForce, ForceMode2D.Impulse);

//         // Trigger hurt animation
//         animator.SetTrigger("Hurt");

//         // Optional: Add invincibility frames
//         StartCoroutine(InvincibilityFrames());
//     }

//     // NEW: Added InvincibilityFrames coroutine
//     private IEnumerator InvincibilityFrames()
//     {
//         // Disable player's collider or set a flag to prevent further damage
//         GetComponent<Collider2D>().enabled = false;

//         // Wait for a short duration (e.g., 0.5 seconds)
//         yield return new WaitForSeconds(0.5f);

//         // Re-enable player's collider or reset the flag
//         GetComponent<Collider2D>().enabled = true;
//     }

//     public void OnAttackStart()
//     {
//         isAttacking = true;
//         animator.SetBool("isAttacking", true);
//         animator.SetTrigger("attack");
//     }

//     public void OnAttackEnd()
//     {
//         isAttacking = false;
//         animator.SetBool("isAttacking", false);
//     }

//     void HandleAttackInput()
//     {
//         if (Input.GetKeyDown(KeyCode.Q) && !isMoving)
//         {
//             if (!isAttacking)
//             {
//                 holdTime = 0f;
//                 isAttacking = true;
//                 attackButtonHeld = true;
//                 isMoving = false;
//                 animator.SetBool("isAttacking", true);
//                 animator.SetTrigger("attack");
//                 OnAttackStart();
//             }
//         }

//         if (attackButtonHeld)
//         {
//             holdTime += Time.deltaTime;
//         }

//         if (Input.GetKeyUp(KeyCode.Q))
//         {
//             if (isAttacking)
//             {
//                 PerformAttack(holdTime >= heavyAttackThreshold);
//                 attackButtonHeld = false;
//                 holdTime = 0f;
//                 isMoving = true;
//                 OnAttackEnd();
//             }
//         }
//     }

//     void PerformAttack(bool isHeavyAttack)
//     {
//         isAttacking = true;
//         Debug.Log("Performing Attack: " + (isHeavyAttack ? "Heavy" : "Light"));
//         if (isHeavyAttack && staminaSystem.ConsumeStamina(heavyAttackStaminaCost))
//         {
//             animator.SetBool("isHeavyAttack", true);
//             animator.SetTrigger("heavyAttack");
//             HeavyAttack();
//         }
//         else if (staminaSystem.ConsumeStamina(lightAttackStaminaCost))
//         {
//             animator.SetBool("isLightAttack", true);
//             animator.SetTrigger("lightAttack");
//             LightAttack();
//         }
//         animator.SetBool("isAttacking", true);
//         StartCoroutine(ResetAttackState());
//     }

//     void LightAttack()
//     {
//         Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
//         foreach (Collider2D enemy in hitEnemies)
//         {
//             enemy.GetComponent<Enemy>().Takedamage(lightAttackDamage, isBlocking);
//             enemy.GetComponent<Rigidbody2D>().AddForce((enemy.transform.position - transform.position).normalized * lightAttackPushForce, ForceMode2D.Impulse);
//         }
//     }

//     void HeavyAttack()
//     {
//         Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
//         foreach (Collider2D enemy in hitEnemies)
//         {
//             enemy.GetComponent<Enemy>().Takedamage(heavyAttackDamage, isBlocking);
//             enemy.GetComponent<Rigidbody2D>().AddForce((enemy.transform.position - transform.position).normalized * heavyAttackPushForce, ForceMode2D.Impulse);
//         }
//     }

//     IEnumerator ResetAttackState()
//     {
//         yield return new WaitForSeconds(attackAnimationDuration);
//         animator.SetBool("isAttacking", false);
//         animator.SetBool("isHeavyAttack", false);
//         animator.SetBool("isLightAttack", false);
//     }

//     void HandleBlockInput()
//     {
//         if (Input.GetKeyDown(KeyCode.E) && staminaSystem.ConsumeStamina(blockStaminaCost))
//         {
//             StartCoroutine(Block());
//         }
//     }

//     IEnumerator Block()
//     {
//         isBlocking = true;
//         animator.SetBool("isBlocking", true);

//         float blockTimer = 0f;
//         while (blockTimer < blockDuration && Input.GetKey(KeyCode.E))
//         {
//             blockTimer += Time.deltaTime;
//             yield return null;
//         }

//         isBlocking = false;
//         animator.SetBool("isBlocking", false);

//         yield return new WaitForSeconds(blockCooldown);
//         canBlockImpact = true;
//     }

//     public void TriggerBlockImpact()
//     {
//         if (canBlockImpact)
//         {
//             canBlockImpact = false;
//             staminaSystem.ConsumeStamina(blockStaminaCost);
//             StartCoroutine(BlockImpactCooldown());
//         }
//     }

//     IEnumerator BlockImpactCooldown()
//     {
//         yield return new WaitForSeconds(successfulBlockDuration);
//         canBlockImpact = true;
//     }

//     public bool IsBlocking()
//     {
//         return isBlocking;
//     }

//     IEnumerator SuccessfulBlock()
//     {
//         canBlockImpact = false;
//         animator.SetTrigger("BlockImpact");

//         PlayBlockEffect();
//         StunNearbyEnemies();

//         yield return new WaitForSeconds(successfulBlockDuration);
//         canBlockImpact = true;
//     }

//     void PlayBlockEffect()
//     {
//         // Implement block effect here
//         // Example:
//         // blockEffectParticleSystem.Play();
//         // audioSource.PlayOneShot(blockSoundEffect);
//     }

//     void StunNearbyEnemies()
//     {
//         Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayers);
//         foreach (Collider2D enemy in hitEnemies)
//         {
//             Enemy enemyScript = enemy.GetComponent<Enemy>();
//             if (enemyScript != null)
//             {
//                 enemyScript.GetStunned();
//             }
//         }
//     }

//     void OnDrawGizmosSelected()
//     {
//         if (attackPoint == null) return;
//         Gizmos.color = Color.red;
//         Gizmos.DrawWireSphere(attackPoint.position, attackRange);
//     }

//     public void IncreaseLightAttackDamage(int amount)
//     {
//         lightAttackDamage += amount;
//         Debug.Log("Light Attack Damage increased to: " + lightAttackDamage);
//     }

//     public void IncreaseHeavyAttackDamage(int amount)
//     {
//         heavyAttackDamage += amount;
//         Debug.Log("Heavy Attack Damage increased to: " + heavyAttackDamage);
//     }
// }



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

    private Rigidbody2D rb;  

 
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
        
        HandleAttackInput(); // Handle attack input
        HandleBlockInput(); // Handle block input

        // Set isMoving based on player input
        isMoving = (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && !isAttacking;
    }

    public void OnEnemyAttack(float damage)
    {
        // Handle enemy attack
        healthSystem.Takedamage((int)damage);
    }
        public void OnEnemyAttack(Enemy enemy, float damage)
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
                GetHurt(enemy.transform.position, damage);
            }
            else
            {
                Debug.LogError("Health component is not assigned to the player!");
            }
        }
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

    void NewLightAttack()
    {
        // Execute light attack logic
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.Takedamage(lightAttackDamage);
                Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();
                if (enemyRb != null)
                {
                    enemyRb.AddForce((enemy.transform.position - transform.position).normalized * lightAttackPushForce, ForceMode2D.Impulse);
                }
            }
        }
    }

    void NewHeavyAttack()
    {
        // Execute heavy attack logic
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.Takedamage(heavyAttackDamage);
                Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();
                if (enemyRb != null)
                {
                    enemyRb.AddForce((enemy.transform.position - transform.position).normalized * heavyAttackPushForce, ForceMode2D.Impulse);
                }
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

    public void ReceiveAttack(Enemy enemy, float damage)  
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
        // Increase light attack damage
        lightAttackDamage += amount;
        Debug.Log("Light Attack Damage increased to: " + lightAttackDamage);
    }

    public void IncreaseHeavyAttackDamage(int amount)
    {
        // Increase heavy attack damage
        heavyAttackDamage += amount;
        Debug.Log("Heavy Attack Damage increased to: " + heavyAttackDamage);
    }
}



