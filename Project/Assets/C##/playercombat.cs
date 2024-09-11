// using System.Collections;  
// using System.Collections.Generic;  
// using UnityEngine;  

// public class PlayerCombat : MonoBehaviour  
// {  
//     // Public variables  
//     public Animator animator; // Reference to the Animator component to control animations  
//     public Transform attackPoint; // The point from which the player's attacks are calculated  
//     public float attackRange = 2.5f; // The range of the player's attack  
//     public LayerMask enemyLayers; // The layers that represent enemies  
//     public int lightAttackDamage = 20; // Damage dealt by a light attack  
//     public int heavyAttackDamage = 40; // Damage dealt by a heavy attack  
//     public float heavyAttackThreshold = 1f; // The threshold of hold time to differentiate between a light and heavy attack  

//     public float lightAttackStaminaCost = 10f; // Stamina cost for a light attack  
//     public float heavyAttackStaminaCost = 25f; // Stamina cost for a heavy attack  
//     public float blockStaminaCost = 15f; // Stamina cost for blocking  

//     public Staminasystem staminaSystem; // Reference to the player's stamina system  
//     public Health healthSystem; // Reference to the player's health system  

//     // Private variables  
//     private float holdTime = 0f; // The duration the attack button has been held  
//     private bool isAttacking = false; // Tracks if the player is currently attacking  
//     private bool attackButtonHeld = false; // Tracks if the attack button is being held down  
//     public bool isBlocking = false; // Tracks if the player is blocking  
//     public float blockDuration = 2f; // Duration of the block  
//     public float blockCooldown = 1f; // Cooldown for blocking  
//     public float successfulBlockDuration = 0.5f; // Duration of a successful block  
//     private bool canBlockImpact = true; // Tracks if the player can block impacts  

//     void Start()  
//     {  
//         animator = GetComponent<Animator>();  
//         staminaSystem = GetComponent<Staminasystem>();  
//         if (staminaSystem == null)  
//         {  
//             Debug.LogError("Staminasystem not found on this GameObject!");  
//         }  
//     }  

//     void Update()  
//     {  
//         if (Input.GetKeyDown(KeyCode.Q))  
//         {  
//             if (!isAttacking)  
//             {  
//                 holdTime = 0f;  
//                 isAttacking = true;  
//                 attackButtonHeld = true;  
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
//                 if (holdTime >= heavyAttackThreshold && staminaSystem.ConsumeStamina(heavyAttackStaminaCost))  
//                 {  
//                     HeavyAttack();  
//                 }  
//                 else if (staminaSystem.ConsumeStamina(lightAttackStaminaCost))  
//                 {  
//                     LightAttack();  
//                 }  

//                 isAttacking = false;  
//                 attackButtonHeld = false;  
//                 holdTime = 0f;  
//             }  
//         }  

//         if (Input.GetKeyDown(KeyCode.E))  
//         {  
//             if (!isBlocking && staminaSystem.ConsumeStamina(blockStaminaCost))  
//             {  
//                 StartCoroutine(Block());  
//             }  
//         }  
//     }  

//     void LightAttack()  
//     {  
//         animator.SetBool("isHeavyAttack", false);  
//         animator.SetTrigger("attack");  

//         Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);  

//         foreach (Collider2D enemy in hitEnemies)  
//         {  
//             Enemy enemyScript = enemy.GetComponent<Enemy>();  
//             if (enemyScript != null)  
//             {  
//                 enemyScript.Takedamage(lightAttackDamage, false);  
//             }  
//         }  
//     }  

//     void HeavyAttack()  
//     {  
//         animator.SetBool("isHeavyAttack", true);  
//         animator.SetTrigger("attack");  

//         Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);  

//         foreach (Collider2D enemy in hitEnemies)  
//         {  
//             Enemy enemyScript = enemy.GetComponent<Enemy>();  
//             if (enemyScript != null)  
//             {  
//                 enemyScript.Takedamage(heavyAttackDamage, false);  
//             }  
//         }  
//     }  

//     IEnumerator Block()  
//     {  
//         isBlocking = true;  
//         animator.SetBool("isBlocking", true);  
//         while (isBlocking)  
//         {  
//             if (Input.GetKeyUp(KeyCode.E))  
//             {  
//                 break;  
//             }  
//             yield return null;  
//         }  
//         isBlocking = false;  
//         animator.SetBool("isBlocking", false);  
//         yield return new WaitForSeconds(blockCooldown);  
//     }  

//     public void TriggerBlockImpact()  
//     {  
//         animator.SetTrigger("BlockImpact");  
//     }  

//     public bool IsBlocking()  
//     {  
//         return isBlocking;  
//     }  

//     public void OnEnemyAttack(int damage)  
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
//                 healthSystem.Takedamage(damage);  
//             }  
//         }  
//     }  

//     IEnumerator SuccessfulBlock()  
//     {  
//         canBlockImpact = false;  
//         animator.SetTrigger("BlockImpact");  

//         // Play block effect  
//         PlayBlockEffect();  

//         // Stun the enemy  
//         StunNearbyEnemies();  

//         yield return new WaitForSeconds(successfulBlockDuration);  
//         canBlockImpact = true;  
//     }  

//     void PlayBlockEffect()  
//     {  
//         // Implement your block effect here  
//         // For example:  
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
// }
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
    private bool isAttacking = false;
    private bool attackButtonHeld = false;
    private bool isBlocking = false;
    private bool canBlockImpact = true;

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
        HandleAttackInput();
        HandleBlockInput();
    }

    void HandleAttackInput()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!isAttacking)
            {
                holdTime = 0f;
                isAttacking = true;
                attackButtonHeld = true;
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
                if (holdTime >= heavyAttackThreshold && staminaSystem.ConsumeStamina(heavyAttackStaminaCost))
                {
                    HeavyAttack();
                }
                else if (staminaSystem.ConsumeStamina(lightAttackStaminaCost))
                {
                    LightAttack();
                }

                isAttacking = false;
                attackButtonHeld = false;
                holdTime = 0f;
            }
        }
    }

    void HandleBlockInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isBlocking && staminaSystem.ConsumeStamina(blockStaminaCost))
            {
                StartCoroutine(Block());
            }
        }
    }

    void LightAttack()
    {
        animator.SetBool("isHeavyAttack", false);
        animator.SetTrigger("attack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.Takedamage(lightAttackDamage, false);
            }
        }
    }

    void HeavyAttack()
    {
        animator.SetBool("isHeavyAttack", true);
        animator.SetTrigger("attack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.Takedamage(heavyAttackDamage, false);
            }
        }
    }

    IEnumerator Block()
    {
        isBlocking = true;
        animator.SetBool("isBlocking", true);
        while (isBlocking)
        {
            if (Input.GetKeyUp(KeyCode.E))
            {
                break;
            }
            yield return null;
        }
        isBlocking = false;
        animator.SetBool("isBlocking", false);
        yield return new WaitForSeconds(blockCooldown);
    }

    public void TriggerBlockImpact()
    {
        animator.SetTrigger("BlockImpact");
    }

    public bool IsBlocking()
    {
        return isBlocking;
    }

    public void OnEnemyAttack(float damage)
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
}