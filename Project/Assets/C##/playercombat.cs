using System;
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
    public float attackAnimationDuration = 1f;  

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
    private bool isBlocking; 
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

    private AudioSource audioSource; 
    public AudioClip lightAttackSound;
    public AudioClip heavyAttackSound;
    private PlayerController playerMovement;

    void Start()  
    {  
        animator = GetComponent<Animator>();  
        staminaSystem = GetComponent<Staminasystem>();  
        healthSystem = GetComponent<Health>();  
        keyRebinding = FindObjectOfType<KeyRebinding>();
        audioSource = GetComponent<AudioSource>();  
        playerMovement = GetComponent<PlayerController>();

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

    private void NewLightAttack()  
    {  
        // Play sound for the light attack  
        if (audioSource != null && lightAttackSound != null)  
        {  
            audioSource.PlayOneShot(lightAttackSound);  
        }  

        animator.SetBool("isHeavyAttack", false);  
        animator.SetTrigger("attack");  

        // Check for all objects in the attack range  
        Collider2D[] hitTargets = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);  

        foreach (Collider2D target in hitTargets)  
        {  
            // Check for the Player  
            PlayerCombat playerCombat = target.GetComponent<PlayerCombat>();  
            if (playerCombat != null)  
            {  
                // Apply damage to the player  
                playerCombat.ReceiveAttack(this, lightAttackDamage, false);  
                Debug.Log("Attacked the player with light damage!");  
            }  
            // Check for Mushroom enemies  
            else   
            {  
                Mushroom mushroom = target.GetComponent<Mushroom>();  
                if (mushroom != null)  
                {  
                    mushroom.Takedamage(lightAttackDamage, false);  
                    Debug.Log("Attacked a Mushroom with light damage!");  
                }  
                else 
                {  
                    Enemy enemyScript = target.GetComponent<Enemy>();  
                    if (enemyScript != null)  
                    {  
                    enemyScript.Takedamage(lightAttackDamage, false);  
                    Debug.Log("Attacked an Enemy with light damage!");  
                    }  
                    else  
                    {  
                        FlyingEye flyingeye = target.GetComponent<FlyingEye>();  
                        if (flyingeye != null)  
                        {  
                        flyingeye.Takedamage(lightAttackDamage, false);  
                        Debug.Log("Attacked an Eye with light damage!");  
                        }  
                    }  
}

                
            } 
        }   

        RaycastHit2D hit = Physics2D.Raycast(attackPoint.position, transform.right, attackRange, enemyLayers);  
        if (hit.collider != null)  
        {  
            EvilWizardBoss bossScript = hit.collider.GetComponent<EvilWizardBoss>();  
            if (bossScript != null)  
            {  
                // Apply damage reduction factor  
                float reducedDamage = lightAttackDamage * bossScript.bossDamageReductionFactor;  
                bossScript.Takedamage((int)reducedDamage);  
                hit.rigidbody.AddForce((hit.transform.position - transform.position).normalized * lightAttackPushForce, ForceMode2D.Impulse);  
            }  
        } 
    } 

private void NewHeavyAttack()  
{  
    // Play sound for the heavy attack  
    if (audioSource != null && heavyAttackSound != null)  
    {  
        audioSource.PlayOneShot(heavyAttackSound);  
    }  

    // Set the animator to trigger heavy attack animation  
    animator.SetBool("isHeavyAttack", true);  
    animator.SetTrigger("attack");  

    // Check for all targets in the attack range  
    Collider2D[] hitTargets = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);  

    foreach (Collider2D target in hitTargets)  
    {  
        // Check for the Player  
        PlayerCombat playerCombat = target.GetComponent<PlayerCombat>();  
        if (playerCombat != null)  
        {  
            // Apply heavy damage to the player  
            playerCombat.ReceiveAttack(this, heavyAttackDamage, true);  
            Debug.Log("Attacked the player with heavy damage!");  
        }  
        // Check for Mushroom enemies  
        else if (target.CompareTag("Mushroom"))  // Prefer using tags for a more generic check; ensure mushrooms have this tag in Unity  
        {  
            Mushroom mushroom = target.GetComponent<Mushroom>();  
            if (mushroom != null)  
            {  
                mushroom.Takedamage(heavyAttackDamage, false);  
                Debug.Log("Attacked a Mushroom with heavy damage!");  
            } 

                else if (target.GetComponent("FlyingEye")) // Ensure that this tag is assigned to your FlyingEye GameObjects  
        {  
            FlyingEye flyingEye = target.GetComponent<FlyingEye>();  
            if (flyingEye != null)  
            {  
                //flyingEye.TakeDamage(heavyAttackDamage); // Only pass damage as an argument  
                Debug.Log("Attacked a FlyingEye with heavy damage!");  
            }  
        }       
        }  
        // Check for FlyingEye enemies  

        // Check for other Enemy types  
        else if (target.GetComponent<Enemy>() is Enemy enemyScript)  
        {  
            enemyScript.Takedamage(heavyAttackDamage, false);  
            Debug.Log("Attacked an Enemy with heavy damage!");  
        }  
    }

     RaycastHit2D hit = Physics2D.Raycast(attackPoint.position, transform.right, attackRange, enemyLayers);  
        if (hit.collider != null)  
        {  
            EvilWizardBoss bossScript = hit.collider.GetComponent<EvilWizardBoss>();  
            if (bossScript != null)  
            {  
                // Apply damage reduction factor  
                float reducedDamage = heavyAttackDamage * bossScript.bossDamageReductionFactor;  
                bossScript.Takedamage((int)reducedDamage);  
                hit.rigidbody.AddForce((hit.transform.position - transform.position).normalized * heavyAttackPushForce, ForceMode2D.Impulse);  
            }  
        }   
}
    public void Takedamage(int damage, bool someFlag)  
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
        //GetComponent<Collider2D>().enabled = false;  
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

        playerMovement.enabled = false; 

        float blockTimer = 0f;  
        while (blockTimer < blockDuration && Input.GetKey(keyRebinding.GetKeyForAction("Block")))  
        {  
            blockTimer += Time.deltaTime;  
            yield return null;  
        }  

        isBlocking = false;  
        animator.SetBool("isBlocking", false); 

        playerMovement.enabled = true; 

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
    if (healthSystem != null)  
    {  
        // Apply damage reduction if the player is blocking  
        if (isBlocking)  
        {  
            damage = (int)(damage * 0.25f); // Reduce damage by 75% when blocking  
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

    IEnumerator SuccessfulBlock()  
    {  
        canBlockImpact = false;  
        animator.SetTrigger("BlockImpact");  
        PlayBlockEffect();  
        StunNearbyEnemies();  

        if (evilWizardBoss != null)  
        {  
            evilWizardBoss.Takedamage(0); // Deal 0 damage to trigger the boss's hurt state  
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

    internal void ReceiveAttack(Enemy enemy, int damageToApply)
    {
        throw new NotImplementedException();
    }


}



