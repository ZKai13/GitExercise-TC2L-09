// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Timeline;

// public class playercombat : MonoBehaviour
// {
//     public Animator animator;
//     public Transform attackpoint;
//     public float attackRange = 2.5f;
//     public LayerMask enemyLayers;
//     public int attackdamage = 40;

//     //cooldown variable
//     public float attackCooldown = 0.5f;  // Cooldown time in seconds
//     private float nextAttackTime = 0f;
//     //attack timing variable
//     public int lightAttackDamage = 20;
//     public int heavyAttackDamage = 60;
//     private float attackHoldTime = 0f;
//     public float heavyAttackThreshold = 1f; // Time to hold the key for a heavy attack
//     void Update()
//     {
//         // Check if the Q key is being held down
//         if (Input.GetKey(KeyCode.Q))
//         {
//             attackHoldTime += Time.deltaTime; // Increment the hold time
//         }

//         // Check if the Q key was released
//         if (Input.GetKeyUp(KeyCode.Q))
//         {
//             if (attackHoldTime >= heavyAttackThreshold)
//             {
//                 HeavyAttack();
//             }
//             else
//             {
//                 LightAttack();
//             }

//             attackHoldTime = 0f; // Reset the hold time
//         }
//     }
//         void LightAttack()
//     {
//         // Play a light attack animation
//         animator.SetTrigger("lightAttack");

//         // Detect enemies in range of the attack
//         Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackpoint.position, attackRange, enemyLayers);

//         // Damage them
//         foreach (Collider2D enemy in hitEnemies)
//         {
//             Enemy enemyScript = enemy.GetComponent<Enemy>();
//             if (enemyScript != null)
//             {
//                 enemyScript.Takedamage(lightAttackDamage);
//             }
//         }

//         Debug.Log("Performed a light attack");
//     }
//     void HeavyAttack()
//     {
//         // Play a heavy attack animation
//         animator.SetTrigger("heavyAttack");

//         // Detect enemies in range of the attack
//         Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackpoint.position, attackRange, enemyLayers);

//         // Damage them
//         foreach (Collider2D enemy in hitEnemies)
//         {
//             Enemy enemyScript = enemy.GetComponent<Enemy>();
//             if (enemyScript != null)
//             {
//                 enemyScript.Takedamage(heavyAttackDamage);
//             }
//         }

//         Debug.Log("Performed a heavy attack");
//     }




//         void Attack()
//     {
//         //play an attack animation
//         animator.SetTrigger("attack");

//         //Detect enemies in range of attack
//         Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackpoint.position, attackRange, enemyLayers);
//         // Check if any enemies were hit
//         if (hitEnemies.Length > 0)
//         {
//             Debug.Log("Enemies detected: " + hitEnemies.Length);
//         }
//         else
//         {
//             Debug.Log("No enemies hit");
//         }

//         //Damage them
//         foreach(Collider2D enemy in hitEnemies)
//         {       
//             Debug.Log("Enemy position: " + enemy.transform.position);
//             Enemy enemyScript = enemy.GetComponent<Enemy>();
//             if (enemyScript != null)
//             {
//                 Debug.Log("Hit enemy: " + enemy.name);
//                 enemyScript.Takedamage(attackdamage);            
//             }
//             else
//             {
//             Debug.Log("Enemy does not have the Enemy script: " + enemy.name);            
//             }
//         }
   
//     }
//     void OnDrawGizmosSelected()
//     {
//         if (attackpoint == null)
//         return;
//         Gizmos.color = Color.red;
//         Gizmos.DrawWireSphere(attackpoint.position, attackRange);
//     }
// }
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class PlayerCombat : MonoBehaviour
// {
//     public Animator animator;
//     public Transform attackPoint;
//     public float attackRange = 2.5f;
//     public LayerMask enemyLayers;
//     public int lightAttackDamage = 20;
//     public int heavyAttackDamage = 60;

//     private float attackHoldTime = 0f;
//     public float heavyAttackThreshold = 1f;

//     void Update()
//     {
//         // Check if the Q key is being held down
//         if (Input.GetKey(KeyCode.Q))
//         {
//             attackHoldTime += Time.deltaTime; // Increment the hold time
//         }

//         // Check if the Q key was released
//         if (Input.GetKeyUp(KeyCode.Q))
//         {
//             // Determine whether to perform a light or heavy attack
//             bool isHeavyAttack = attackHoldTime >= heavyAttackThreshold;

//             Attack(isHeavyAttack);

//             attackHoldTime = 0f; // Reset the hold time
//         }
//     }

//     void Attack(bool isHeavyAttack)
//     {
//         // Play the attack animation, passing whether it's a heavy attack
//         animator.SetBool("isHeavyAttack", isHeavyAttack);
//         animator.SetTrigger("attack");

//         // Detect enemies in range of the attack
//         Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

//         // Damage them
//         foreach (Collider2D enemy in hitEnemies)
//         {
//             Enemy enemyScript = enemy.GetComponent<Enemy>();
//             if (enemyScript != null)
//             {
//                 int damage = isHeavyAttack ? heavyAttackDamage : lightAttackDamage;
//                 enemyScript.Takedamage(damage);
//             }
//         }
//     }

//     void OnDrawGizmosSelected()
//     {
//         if (attackPoint == null)
//             return;

//         Gizmos.color = Color.red;
//         Gizmos.DrawWireSphere(attackPoint.position, attackRange);
//     }
// }

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class PlayerCombat : MonoBehaviour
// {
//     public Animator animator;
//     public Transform attackPoint;
//     public float attackRange = 2.5f;
//     public LayerMask enemyLayers;
//     public int lightAttackDamage = 20;
//     public int heavyAttackDamage = 40;
//     public float heavyAttackThreshold = 1f; // Time in seconds to determine heavy attack

//     private float holdTime = 1f;
//     private bool isAttacking = false;

//     void Update()
//     {
//         // Check if the Fire1 button is pressed
//         if (Input.GetKeyDown(KeyCode.Q))
//         {
//             if (!isAttacking)
//             {
//                 // Start the attack timer
//                 holdTime = 0f;
//                 isAttacking = true;
//             }

//             // Increment holdTime based on time passed
//             holdTime += Time.deltaTime;
//             Debug.Log("Hold Time: " + holdTime);
//         }

//         // Check if the Fire1 button was released
//         if (Input.GetKeyDown(KeyCode.Q))
//         {
//             if (isAttacking)
//             {
//                 // Determine attack type based on holdTime
//                 if (holdTime >= heavyAttackThreshold)
//                 {
//                     Debug.Log("Heavy Attack Triggered"); // Debug log
//                     HeavyAttack(); // Heavy attack
//                 }
//                 else
//                 {
//                     Debug.Log("Light Attack Triggered");
//                     LightAttack(); // Quick attack
//                 }

//                 // Reset attack state
//                 isAttacking = false;
//             }
//         }
//     }

//     void LightAttack()
//     {
//         // Trigger light attack animation
//         animator.SetBool("isHeavyAttack", false);
//         animator.SetTrigger("attack");

//         // Detect enemies in range of attack
//         Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

//         // Damage enemies
//         foreach (Collider2D enemy in hitEnemies)
//         {
//             Enemy enemyScript = enemy.GetComponent<Enemy>();
//             if (enemyScript != null)
//             {
//                 enemyScript.Takedamage(lightAttackDamage);
//             }
//         }
//     }

//     void HeavyAttack()
//     {
//         Debug.Log("Performing Heavy Attack"); // Debug log
//         // Trigger heavy attack animation
//         animator.SetBool("isHeavyAttack", true);
//         animator.SetTrigger("attack");

//         // Detect enemies in range of attack
//         Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

//         // Damage enemies
//         foreach (Collider2D enemy in hitEnemies)
//         {
//             Enemy enemyScript = enemy.GetComponent<Enemy>();
//             if (enemyScript != null)
//             {
//                 enemyScript.Takedamage(heavyAttackDamage);
//             }
//         }
//     }

//     void OnDrawGizmosSelected()
//     {
//         if (attackPoint == null)
//             return;

//         Gizmos.color = Color.red;
//         Gizmos.DrawWireSphere(attackPoint.position, attackRange);
//     }
// }
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    public Transform attackPoint;
    public float attackRange = 2.5f;
    public LayerMask enemyLayers;
    public int lightAttackDamage = 20;
    public int heavyAttackDamage = 40;
    public float heavyAttackThreshold = 1f; // Time in seconds to determine heavy attack

    private float holdTime = 0f;
    private bool isAttacking = false;
    private bool attackButtonHeld = false;

    void Update()
    {
        // Check if the Fire1 button is pressed
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!isAttacking)
            {
                // Start the attack timer
                holdTime = 0f;
                isAttacking = true;
                attackButtonHeld = true;
            }
        }

        // If the button is being held, increment the hold time
        if (attackButtonHeld)
        {
            holdTime += Time.deltaTime;
            Debug.Log("Hold Time: " + holdTime);
        }

        // Check if the Fire1 button was released
        if (Input.GetKeyUp(KeyCode.Q))
        {
            if (isAttacking)
            {
                // Determine attack type based on holdTime
                if (holdTime >= heavyAttackThreshold)
                {
                    Debug.Log("Heavy Attack Triggered"); // Debug log
                    HeavyAttack(); // Heavy attack
                }
                else
                {
                    Debug.Log("Light Attack Triggered");
                    LightAttack(); // Quick attack
                }

                // Reset attack state
                isAttacking = false;
                attackButtonHeld = false;
            }
        }
    }

    void LightAttack()
    {
        // Trigger light attack animation
        animator.SetBool("isHeavyAttack", false);
        animator.SetTrigger("attack");

        // Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // Damage enemies
        foreach (Collider2D enemy in hitEnemies)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.Takedamage(lightAttackDamage);
            }
        }
    }

    void HeavyAttack()
    {
        Debug.Log("Performing Heavy Attack"); // Debug log
        // Trigger heavy attack animation
        animator.SetBool("isHeavyAttack", true);
        animator.SetTrigger("attack");

        // Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // Damage enemies
        foreach (Collider2D enemy in hitEnemies)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.Takedamage(heavyAttackDamage);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
