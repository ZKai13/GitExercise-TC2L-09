// <<<<<<< Updated upstream
// using System.Collections;  
// using System.Collections.Generic;  
// using UnityEngine;  

// public class PlayerCombat : MonoBehaviour  
// {  
//     // 公共变量  
// =======
// using UnityEngine;  
// using System.Collections;  

// public class PlayerCombat : MonoBehaviour  
// {  
//     // Public variables  
// >>>>>>> Stashed changes
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

// <<<<<<< Updated upstream
//     // 私有变量  
//     private float holdTime = 0f;  
//     public bool isAttacking = false;  
//     private bool attackButtonHeld = false;  
//     private bool isBlocking = false;  
//     private bool canBlockImpact = true;  
//     public float lightAttackPushForce = 3f;  
//     public float heavyAttackPushForce = 5f;  
//     private bool isMoving = false;  
// =======
//     public float lightAttackPushForce = 3f;  
//     public float heavyAttackPushForce = 5f;  
// >>>>>>> Stashed changes
//     public int maxHealth = 100;  
//     public float hurtForce = 10f;  
//     public float invincibilityDuration = 0.5f;  

//     public EvilWizardBoss evilWizardBoss;  

//     // Private variables  
//     private float holdTime = 0f;  
//     private bool isAttacking = false;  
//     private bool attackButtonHeld = false;  
//     private bool isBlocking = false;  
//     private bool canBlockImpact = true;  
//     private bool isMoving = false;  
//     private int currentHealth;  
//     private bool isStunned = false;  
// <<<<<<< Updated upstream
//     private float stunDuration = 0f;

//     private Rigidbody2D rb; 
//     public EvilWizardBoss evilWizardBoss;
//     private KeyRebinding keyRebinding;  

//     void Start()  
//     {  
//         animator = GetComponent<Animator>();  
//         staminaSystem = GetComponent<Staminasystem>();  
//         healthSystem = GetComponent<Health>();  
//         keyRebinding = FindObjectOfType<KeyRebinding>();  

//         if (animator == null) Debug.LogError("Animator component not found on this GameObject!");  
//         if (staminaSystem == null) Debug.LogError("Staminasystem not found on this GameObject!");  
//         if (healthSystem == null) Debug.LogError("Health component not found on this GameObject!");  
//         if (keyRebinding == null) Debug.LogError("KeyRebinding component not found!");  

//         LoadPlayerPrefs();  
//     }  

//     private void OnApplicationQuit()  
//     {  
//         SavePlayerPrefs();  
//     }  

//     private void OnDisable()   
//     {  
//         SavePlayerPrefs();  
//     }  

//     void Update()  
//     {  
//         if (!isAttacking)  
//         {  
//             float moveHorizontal = Input.GetAxisRaw("Horizontal");  
//             float moveVertical = Input.GetAxisRaw("Vertical");  
//         }  
// =======
//     private float stunDuration = 0f;  
//     private bool isInvincible = false; 
    
//     private Rigidbody2D rb;  
 

//     void Start()  
//     {  
//         currentHealth = maxHealth;  
//         animator = GetComponent<Animator>();  
//         staminaSystem = GetComponent<Staminasystem>();  
//         healthSystem = GetComponent<Health>();  
//         rb = GetComponent<Rigidbody2D>();  

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
// >>>>>>> Stashed changes
//         if (isStunned)  
//         {  
//             stunDuration -= Time.deltaTime;  
//             if (stunDuration <= 0)  
//             {  
//                 isStunned = false;  
//             }  
//             return;  
//         }  
// <<<<<<< Updated upstream
//         HandleAttackInput();  
//         HandleBlockInput();  

//         isMoving = (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && !isAttacking;  
//     }  

//     public void OnEnemyAttack(float damage)  
//     {  
//         healthSystem.Takedamage((int)damage);  
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
//         if (keyRebinding == null)  
//         {  
//             Debug.LogError("KeyRebinding is not assigned!");  
//             return;  
//         }  

//         if (Input.GetKeyDown(keyRebinding.GetKeyForAction("Attack")) && !isMoving)  
// =======

//         if (!isAttacking)  
//         {  
//             float moveHorizontal = Input.GetAxisRaw("Horizontal");  
//             float moveVertical = Input.GetAxisRaw("Vertical");  
//             // Add movement logic here  
//         }  

//         HandleAttackInput();  
//         HandleBlockInput();  

//         isMoving = (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && !isAttacking;  
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
// >>>>>>> Stashed changes
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

// <<<<<<< Updated upstream
//         if (Input.GetKeyUp(keyRebinding.GetKeyForAction("Attack")))  
// =======
//         if (Input.GetKeyUp(KeyCode.Q))  
// >>>>>>> Stashed changes
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
// <<<<<<< Updated upstream
//         isAttacking = true;
//         Debug.Log("Performing Attack: " + (isHeavyAttack ? "Heavy" : "Light"));
//         if (isHeavyAttack && staminaSystem.ConsumeStamina(heavyAttackStaminaCost))
//         {
//             animator.SetBool("isHeavyAttack", true);
//             animator.SetTrigger("heavyAttack");
//             NewHeavyAttack();
//         }
//         else if (staminaSystem.ConsumeStamina(lightAttackStaminaCost))
//         {
//             animator.SetBool("isLightAttack", true);
//             animator.SetTrigger("lightAttack");
//             NewLightAttack();
//         }
//         animator.SetBool("isAttacking", true);
//         StartCoroutine(ResetAttackState());  
// =======
//         isAttacking = true;  
//         Debug.Log("Performing Attack: " + (isHeavyAttack ? "Heavy" : "Light"));  
//         if (isHeavyAttack && staminaSystem.ConsumeStamina(heavyAttackStaminaCost))  
//         {  
//             animator.SetBool("isHeavyAttack", true);  
//             animator.SetTrigger("heavyAttack");  
//             NewHeavyAttack();  
//         }  
//         else if (staminaSystem.ConsumeStamina(lightAttackStaminaCost))  
//         {  
//             animator.SetBool("isLightAttack", true);  
//             animator.SetTrigger("lightAttack");  
//             NewLightAttack();  
//         }  
//         animator.SetBool("isAttacking", true);  
//         StartCoroutine(ResetAttackState());  
//     }  

//     private void GetHurt(Vector3 attackerPosition, float damage)  
//     {  
//         healthSystem.Takedamage(damage);  
//         Debug.Log($"Player took {damage} damage. Current health: {healthSystem.currentHealth}");  

//         Vector2 hurtDirection = (transform.position - attackerPosition).normalized;  
//         rb.AddForce(hurtDirection * hurtForce, ForceMode2D.Impulse);  

//         animator.SetTrigger("Hurt");  
//         StartCoroutine(InvincibilityFrames());  
//     }  

//     private IEnumerator InvincibilityFrames()  
//     {  
//         GetComponent<Collider2D>().enabled = false;  
//         yield return new WaitForSeconds(invincibilityDuration);  
//         GetComponent<Collider2D>().enabled = true;  
//     }  

//     public void GetStunned(float duration)  
//     {  
//         if (!isStunned)  
//         {  
//             isStunned = true;  
//             stunDuration = duration;  
//             animator.SetTrigger("Stunned");  
//             StartCoroutine(StunCoroutine());  
//         }  
//     }  

//     private IEnumerator StunCoroutine()  
//     {  
//         yield return new WaitForSeconds(stunDuration);  
//         isStunned = false;  
// >>>>>>> Stashed changes
//     }  

//     void NewLightAttack()  
//     {  
//         RaycastHit2D hit = Physics2D.Raycast(attackPoint.position, transform.right, attackRange, enemyLayers);  
//         if (hit.collider != null)  
//         {  
//             EvilWizardBoss bossScript = hit.collider.GetComponent<EvilWizardBoss>();  
//             if (bossScript != null)  
//             {  
//                 float reducedDamage = lightAttackDamage * bossScript.bossDamageReductionFactor;  
//                 bossScript.TakeDamage((int)reducedDamage);  
//                 hit.rigidbody.AddForce((hit.transform.position - transform.position).normalized * lightAttackPushForce, ForceMode2D.Impulse);  
//             }  
//         }  
//     }    

//     void NewHeavyAttack()  
//     {  
//         RaycastHit2D hit = Physics2D.Raycast(attackPoint.position, transform.right, attackRange, enemyLayers);  
//         if (hit.collider != null)  
//         {  
//             EvilWizardBoss bossScript = hit.collider.GetComponent<EvilWizardBoss>();  
//             if (bossScript != null)  
//             {  
//                 float reducedDamage = heavyAttackDamage * bossScript.bossDamageReductionFactor;  
//                 bossScript.TakeDamage((int)reducedDamage);  
//                 hit.rigidbody.AddForce((hit.transform.position - transform.position).normalized * heavyAttackPushForce, ForceMode2D.Impulse);  
//             }  
//         }  
//     }  

//     public void Takedamage(int damage)  
//     {  
//         if (healthSystem != null)  
//         {  
//             healthSystem.Takedamage(damage);  
//             Debug.Log($"Player took {damage} damage. Current health: {healthSystem.currentHealth}");  
//             animator.SetTrigger("Hurt");  
//         }  
//         else  
//         {  
//             Debug.LogError("Health component is not assigned to the player!");  
//         }  

//         if (healthSystem.currentHealth <= 0)  
//         {  
//             Die();  
//         }  
//     }

//     public void ApplyKnockback(Vector2 force)  
//     {  
// <<<<<<< Updated upstream
//         Debug.Log("Player died!");  
//         // Trigger death animation  
//         animator.SetTrigger("Die");  
        
//         // Disable the player  
//         GetComponent<Collider2D>().enabled = false;  
//         this.enabled = false;  
//     }
//     IEnumerator ResetAttackState()  
//     {  
//         yield return new WaitForSeconds(attackAnimationDuration);  
//         animator.SetBool("isAttacking", false);  
//         animator.SetBool("isHeavyAttack", false);  
//         animator.SetBool("isLightAttack", false);
//         isAttacking = false;   
//     }  

//     void HandleBlockInput()  
//     {  
//         if (Input.GetKeyDown(keyRebinding.GetKeyForAction("Block")) && staminaSystem.ConsumeStamina(blockStaminaCost))  
//         {  
//             StartCoroutine(Block());  
//         }  
//     }  

//     IEnumerator Block()  
//     {  
//         isBlocking = true;  
//         animator.SetBool("isBlocking", true);  

//         float blockTimer = 0f;  
//         while (blockTimer < blockDuration && Input.GetKey(keyRebinding.GetKeyForAction("Block")))  
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

//     public void ReceiveAttack(MonoBehaviour attacker, float damage)  
//     {  
//         // Handle enemy attack  
//         if (isBlocking && canBlockImpact)  
// =======
//         if (rb != null)  
// >>>>>>> Stashed changes
//         {  
//             rb.AddForce(force, ForceMode2D.Impulse);  
//         }  
//     }  

//     private void Die()  
//     {  
//         Debug.Log("Player died!");  
//         // animator.SetTrigger("Die");  
//         // GetComponent<Collider2D>().enabled = false;  
//         // this.enabled = false;  
//     }  

//     IEnumerator ResetAttackState()  
//     {  
//         yield return new WaitForSeconds(attackAnimationDuration);  
//         Debug.Log("Resetting attack state");  
//         animator.SetBool("isAttacking", false);  
//         animator.SetBool("isHeavyAttack", false);  
//         animator.SetBool("isLightAttack", false);  
//         isAttacking = false;  
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

//     // public void ReceiveAttack(MonoBehaviour attacker, float damage, bool isHeavyAttack)  
//     // {  
//     //     if (isBlocking && canBlockImpact)  
//     //     {  
//     //         Debug.Log("Successful block!");  
//     //         if (isHeavyAttack)  
//     //         {  
//     //             Vector2 knockbackDirection = (transform.position - attacker.transform.position).normalized;  
//     //             ApplyKnockback(knockbackDirection * 5f);  
//     //         }  
//     //         else  
//     //         {  
//     //             // Instead of calling a specific stun method, we'll try to find a method to call  
//     //             TryStunAttacker(attacker);  
//     //         }  
//     //         StartCoroutine(SuccessfulBlock());  
//     //     }  
//     //     else  
//     //     {  
//     //         if (healthSystem != null)  
//     //         {  
//     //             if (isBlocking)  
//     //             {  
//     //                 damage *= 0.5f;  
//     //             }  

//     //             healthSystem.Takedamage(damage);  
//     //             Debug.Log($"Player took {damage} damage. Current health: {healthSystem.currentHealth}");  

//     //             if (attacker != null)  
//     //             {  
//     //                 Vector2 knockbackDirection = (transform.position - attacker.transform.position).normalized;  
//     //                 ApplyKnockback(knockbackDirection * (isHeavyAttack ? 7f : 3f));  
//     //             }  

//     //             animator.SetTrigger("Hurt");  
//     //         }  
//     //         else  
//     //         {  
//     //             Debug.LogError("Health component is not assigned to the player!");  
//     //         }  
//     //     }  
//     // }  
// public void ReceiveAttack(MonoBehaviour attacker, int damage, bool isHeavyAttack)  
// {  
//     if (isInvincible) return;  

//     currentHealth = Mathf.Max(0, currentHealth - damage);  
//     Debug.Log($"Player took {damage} damage. Current health: {currentHealth}");  

//     // Trigger hurt animation, invincibility frames, etc.  

//     if (currentHealth <= 0)  
//     {  
//         Die();  
//     }  
// }

//     private void TryStunAttacker(MonoBehaviour attacker)  
//     {  
//         // Try to call a Stun or GetStunned method on the attacker  
//         var stunMethod = attacker.GetType().GetMethod("Stun") ?? attacker.GetType().GetMethod("GetStunned");  
//         if (stunMethod != null)  
//         {  
//             if (stunMethod.GetParameters().Length == 0)  
//             {  
//                 stunMethod.Invoke(attacker, null);  
//             }  
//             else if (stunMethod.GetParameters().Length == 1 && stunMethod.GetParameters()[0].ParameterType == typeof(float))  
//             {  
//                 stunMethod.Invoke(attacker, new object[] { 2f });  
//             }  
//             Debug.Log($"Stunned {attacker.name}");  
//         }  
//         else  
//         {  
//             Debug.Log($"Unable to stun {attacker.name}. No stun method found.");  
//         }  
//     }


//     IEnumerator SuccessfulBlock()  
//     {  
//         canBlockImpact = false;  
//         animator.SetTrigger("BlockImpact");  
//         PlayBlockEffect();  
//         StunNearbyEnemies();  

//         if (evilWizardBoss != null)  
//         {  
// <<<<<<< Updated upstream
//             evilWizardBoss.TakeDamage(0); // Deal 0 damage to trigger the boss's hurt state  
//             Debug.Log("Boss stunned (simulated)"); 
//         }
// =======
//             evilWizardBoss.TakeDamage(0);  
//             Debug.Log("Boss stunned (simulated)");  
//         }  
// >>>>>>> Stashed changes

//         yield return new WaitForSeconds(successfulBlockDuration);  
//         canBlockImpact = true;  
//     }  

//     void PlayBlockEffect()  
//     {  
// <<<<<<< Updated upstream
//         // 实现格挡特效  
//         //blockEffectParticleSystem.Play();  
//         //audioSource.PlayOneShot(blockSoundEffect);  
// =======
//         // Implement block effect  
// >>>>>>> Stashed changes
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
//         SavePlayerPrefs();  
//         Debug.Log("Light Attack Damage increased to: " + lightAttackDamage);  
//     }  

//     public void IncreaseHeavyAttackDamage(int amount)  
//     {  
//         heavyAttackDamage += amount;  
//         SavePlayerPrefs();  
//         Debug.Log("Heavy Attack Damage increased to: " + heavyAttackDamage);  
//     }  

//     private void SavePlayerPrefs()  
//     {  
//         PlayerPrefs.SetInt("LightAttackDamage", lightAttackDamage);  
//         PlayerPrefs.SetInt("HeavyAttackDamage", heavyAttackDamage);  
//         PlayerPrefs.Save();  
//     }  

//     private void LoadPlayerPrefs()  
//     {  
// <<<<<<< Updated upstream
//         lightAttackDamage = PlayerPrefs.GetInt("LightAttackDamage", lightAttackDamage);  
//         heavyAttackDamage = PlayerPrefs.GetInt("HeavyAttackDamage", heavyAttackDamage);  
//     } 

   


// }
using System.Collections;  
using System.Collections.Generic;  
using UnityEngine;  

public class PlayerCombat : MonoBehaviour  
{  
    // 公共变量  
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

    // 私有变量  
    private float holdTime = 0f;  
    public bool isAttacking = false;  
    private bool attackButtonHeld = false;  
    private bool isBlocking = false;  
    private bool canBlockImpact = true;  
    public float lightAttackPushForce = 3f;  
    public float heavyAttackPushForce = 5f;  
    private bool isMoving = false;  
    public int maxHealth = 100;  
    private int currentHealth;  
    public float hurtForce = 10f;  
    public float invincibilityDuration = 0.5f;
    private bool isStunned = false;  
    private float stunDuration = 0f;

    private Rigidbody2D rb; 
    public EvilWizardBoss evilWizardBoss;
    private KeyRebinding keyRebinding;  

    void Start()  
    {  
        animator = GetComponent<Animator>();  
        staminaSystem = GetComponent<Staminasystem>();  
        healthSystem = GetComponent<Health>();  
        keyRebinding = FindObjectOfType<KeyRebinding>();  

        if (animator == null) Debug.LogError("Animator component not found on this GameObject!");  
        if (staminaSystem == null) Debug.LogError("Staminasystem not found on this GameObject!");  
        if (healthSystem == null) Debug.LogError("Health component not found on this GameObject!");  
        if (keyRebinding == null) Debug.LogError("KeyRebinding component not found!");  

        LoadPlayerPrefs();  
    }  

    private void OnApplicationQuit()  
    {  
        SavePlayerPrefs();  
    }  

    private void OnDisable()   
    {  
        SavePlayerPrefs();  
    }  

    void Update()  
    {  
        if (!isAttacking)  
        {  
            float moveHorizontal = Input.GetAxisRaw("Horizontal");  
            float moveVertical = Input.GetAxisRaw("Vertical");  
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
        HandleAttackInput();  
        HandleBlockInput();  

        isMoving = (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && !isAttacking;  
    }  

    public void OnEnemyAttack(float damage)  
    {  
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
        if (keyRebinding == null)  
        {  
            Debug.LogError("KeyRebinding is not assigned!");  
            return;  
        }  

        if (Input.GetKeyDown(keyRebinding.GetKeyForAction("Attack")) && !isMoving)  
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

        if (Input.GetKeyUp(keyRebinding.GetKeyForAction("Attack")))  
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
        animator.SetBool("isAttacking", false);  
        animator.SetBool("isHeavyAttack", false);  
        animator.SetBool("isLightAttack", false);
        isAttacking = false;   
    }  

    void HandleBlockInput()  
    {  
        if (Input.GetKeyDown(keyRebinding.GetKeyForAction("Block")) && staminaSystem.ConsumeStamina(blockStaminaCost))  
        {  
            StartCoroutine(Block());  
        }  
    }  

    IEnumerator Block()  
    {  
        isBlocking = true;  
        animator.SetBool("isBlocking", true);  

        float blockTimer = 0f;  
        while (blockTimer < blockDuration && Input.GetKey(keyRebinding.GetKeyForAction("Block")))  
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

    public void ReceiveAttack(MonoBehaviour attacker, int damage, bool isHeavyAttack)  
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
                    damage = (int)(damage * 0.25f);     // Reduce damage by 50% when blocking  
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
        canBlockImpact = false;  
        animator.SetTrigger("BlockImpact");  
        PlayBlockEffect();  
        StunNearbyEnemies();  

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
        // 实现格挡特效  
        //blockEffectParticleSystem.Play();  
        //audioSource.PlayOneShot(blockSoundEffect);  
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

    private void LoadPlayerPrefs()  
    {  
        lightAttackDamage = PlayerPrefs.GetInt("LightAttackDamage", lightAttackDamage);  
        heavyAttackDamage = PlayerPrefs.GetInt("HeavyAttackDamage", heavyAttackDamage);  
    } 

    public void ResetAttackDamage()  
    {  
         
        lightAttackDamage = 20;
        heavyAttackDamage = 60;

    
        SavePlayerPrefs();  

        Debug.Log("Attack damage has been reset to default values.");  
    }


}



