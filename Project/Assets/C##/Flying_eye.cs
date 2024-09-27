using UnityEngine;  
using System.Collections;  

public class FlyingEye : MonoBehaviour  
{  
    public float health = 100f;  
    public float lightAttackCooldown = 2f;  
    public float heavyAttackCooldown = 7f;  
    public float lightAttackChance = 100f;  
    public float moveSpeed = 3f;  
    private float attackCooldown = 1f; // Cooldown period in seconds  
    private float nextAttackTime = 0f;  
    public int lightAttackDamage = 10;  
    public int heavyAttackDamage = 25;  

// 敌人的旋转速度和角度范围
    public float rotationSpeed = 100f;  
    public float rotationChangeInterval = 2f;  
    public float maxRotationAngle = 30f;  

    public Collider2D lightAttackCollider;  
    public Collider2D heavyAttackCollider;  

    private Animator animator;  
    private Transform player;  
    private Rigidbody2D rb;  
    private SpriteRenderer spriteRenderer;  
    private bool canLightAttack = true;  
    private bool canHeavyAttack = true;  

    private float targetRotation;  
    private float currentRotation;  
    public float lightAttackRange = 2f;  
    public float heavyAttackRange = 8f;  
    public float heavyAttackDashSpeed = 10f;  
    private bool isPerformingHeavyAttack = false;  
    private Flying_Eye_Health healthComponent; 
     

    private static readonly int IdleHash = Animator.StringToHash("Idle");  
    private static readonly int LightAttackHash = Animator.StringToHash("LightAttack");  
    private static readonly int HeavyAttackHash = Animator.StringToHash("HeavyAttack");  
    private static readonly int HurtHash = Animator.StringToHash("Hurt");  
    private static readonly int DieHash = Animator.StringToHash("Die");  

    private void Start()  
    {  
        animator = GetComponent<Animator>();  
        rb = GetComponent<Rigidbody2D>();  
        spriteRenderer = GetComponent<SpriteRenderer>();  
        player = GameObject.FindGameObjectWithTag("Player").transform;  
        PlayIdle();  
        StartCoroutine(RandomRotation()); 
        healthComponent = GetComponent<Flying_Eye_Health>();   

        // Ensure attack colliders are disabled at start  
        lightAttackCollider.enabled = false;  
        heavyAttackCollider.enabled = false;  
    }  

    private IEnumerator PerformHeavyAttackSequence()  
    {  
        Debug.Log("Starting Heavy Attack Sequence");  
        isPerformingHeavyAttack = true;  
        canHeavyAttack = false;  

        // Dash towards the player  
        float dashDuration = 0.5f; // Adjust as needed  
        float elapsedTime = 0f;  
        Vector2 startPosition = transform.position;  
        Vector2 targetPosition = player.position;  

        while (elapsedTime < dashDuration)  
        {  
            transform.position = Vector2.Lerp(startPosition, targetPosition, elapsedTime / dashDuration);  
            elapsedTime += Time.deltaTime;  
            yield return null;  
        }  

        // Perform the heavy attack  
        Debug.Log("Executing Heavy Attack Animation");  
        animator.Play("HeavyAttack"); // Replace "HeavyAttack" with the exact name of your animation state  

        // Log the current state of the animator  
        Debug.Log($"Current Animator State: {animator.GetCurrentAnimatorStateInfo(0).fullPathHash}");  

        StartCoroutine(ActivateAttackCollider(heavyAttackCollider, 1f));  

        // Wait for attack animation to finish  
        yield return new WaitForSeconds(1f); // Adjust based on your animation length  

        Debug.Log("Heavy Attack Sequence Completed");  
        isPerformingHeavyAttack = false;  
        StartCoroutine(HeavyAttackCooldown());  
    }  
    

private void Update()  
{  
    if (health <= 0) return;  

    if (player == null)  
    {  
        Debug.LogWarning("Player reference is null!");  
        return;  
    }  

    Vector2 directionToPlayer = player.position - transform.position;  
    float distanceToPlayer = directionToPlayer.magnitude;  

    bool playerInLightAttackRange = distanceToPlayer <= lightAttackRange;  
    bool playerInHeavyAttackRange = distanceToPlayer <= heavyAttackRange;  

    // Always move towards the player  
    MoveTowardsPlayer();  

    if (Time.time >= nextAttackTime)  
    {  
        if (playerInHeavyAttackRange && canHeavyAttack)  
        {  
            Debug.Log("Initiating Heavy Attack");  
            nextAttackTime = Time.time + heavyAttackCooldown; // Set the next available attack time  
            StartCoroutine(PerformHeavyAttackSequence());  
        }  
        else if (playerInLightAttackRange && canLightAttack)  
        {  
            Debug.Log("Performing Light Attack");  
            nextAttackTime = Time.time + lightAttackCooldown; // Set the next available attack time  
            PerformLightAttack();  
        }  
    }  

    FacePlayer();  
    RotateBody();  
}  

private void MoveTowardsPlayer()  
{  
    if (health <= 0) return;  // Ensure no movement when dead.  
    
    // Calculate the direction to the player and stop if already close enough  
    Vector2 direction = (player.position - transform.position).normalized;  
    rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);  
}

    private void FacePlayer()  
    {  
        Vector2 direction = player.position - transform.position;  
        spriteRenderer.flipX = direction.x < 0;  
    }  

    private void RotateBody()  
    {  
        currentRotation = Mathf.MoveTowards(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);  
        transform.rotation = Quaternion.Euler(0, 0, currentRotation * (spriteRenderer.flipX ? -1 : 1));  
    }  

    private IEnumerator RandomRotation()  
    {  
        while (true)  
        {  
            targetRotation = Random.Range(-maxRotationAngle, maxRotationAngle);  
            yield return new WaitForSeconds(rotationChangeInterval);  
        }  
    }  

    private void PerformAttack(bool playerInLightAttackRange)  
    {  
        if (canHeavyAttack && Random.value > lightAttackChance)  
        {  
            Debug.Log("Performing Heavy Attack");  
            PerformHeavyAttack();  
        }  
        else if (canLightAttack)  
        {  
            Debug.Log("Performing Light Attack");  
            PerformLightAttack();  
        }  
    }  

    private void PerformLightAttack()  
    {  
        canLightAttack = false;  
        animator.SetTrigger(LightAttackHash);  
        StartCoroutine(LightAttackCooldown());  
        StartCoroutine(ActivateAttackCollider(lightAttackCollider, 0.5f)); // Activate for 0.5 seconds  
    }  

    private void PerformHeavyAttack()  
    {  
        canHeavyAttack = false;  
        animator.SetTrigger(HeavyAttackHash);  
        StartCoroutine(HeavyAttackCooldown());  
        StartCoroutine(ActivateAttackCollider(heavyAttackCollider, 1f)); // Activate for 1 second  
    }  

    private bool hasDamagedPlayer = false;  
    private IEnumerator ActivateAttackCollider(Collider2D attackCollider, float duration)  
    {  
        attackCollider.enabled = true;  
        hasDamagedPlayer = false; // Reset damage flag  

        // Only activate collider for a short amount of time  
        yield return new WaitForSeconds(duration);  

        // Apply damage if player is in range when collider is enabled  
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(attackCollider.bounds.center, attackCollider.bounds.size, 0f);  
        foreach (var hitCollider in hitColliders)  
        {  
            if (hitCollider.CompareTag("Player") && !hasDamagedPlayer)  
            {  
                ApplyDamage(attackCollider == lightAttackCollider ? lightAttackDamage : heavyAttackDamage);  
                hasDamagedPlayer = true; // Mark as damaged to prevent repeat damage  
                break;  
            }  
        }  

        attackCollider.enabled = false; // Deactivate collider after checking  
    }    
    private IEnumerator LightAttackCooldown()  
    {  
        yield return new WaitForSeconds(lightAttackCooldown);  
        canLightAttack = true;  
    }  

    private IEnumerator HeavyAttackCooldown()  
    {  
        yield return new WaitForSeconds(heavyAttackCooldown);  
        canHeavyAttack = true;  
    }  

    public void PlayIdle()  
    {  
        animator.SetTrigger(IdleHash);  
    }  

    public void Takedamage(int damage, bool isBlocking)  
    {  
        Debug.Log($"Takedamage called with damage: {damage}, isBlocking: {isBlocking}");  

        if (health <= 0) return;  

        if (isBlocking)  
        {  
            Debug.Log("Enemy is stunned due to block.");   
        }  
        else  
        {  
            if (healthComponent == null)  
            {  
                Debug.LogError("healthComponent is null at the time of taking damage.");  
                return;  
            }  

            health -= damage;  
            healthComponent.TakeDamage(damage);  

            if (health <= 0)  
            {  
                Die();  
            }  
            else  
            {  
                animator.SetTrigger(HurtHash);  
            }  
        }  
    }


    private void Die()  
    {  
        GetComponent<Collider2D>().enabled = false;  
        StartCoroutine(TriggerDeathAnimation());  
    }

    IEnumerator TriggerDeathAnimation()
    {
        yield return new WaitForEndOfFrame();
        animator.SetTrigger(DieHash);
        

        yield return new WaitForSeconds(2f);
        this.enabled = false;
        Destroy(gameObject);
    }  

    private void ApplyDamage(int damage)  
    {  
        if (player != null)  
        {  
            player.GetComponent<PlayerCombat>().ReceiveAttack(this, damage, damage == heavyAttackDamage);  
        }  
    }  

    public void GetStunned(float duration)  
    {  
        StartCoroutine(StunCoroutine(duration));  
    }  

    private IEnumerator StunCoroutine(float duration)  
    {  
        canLightAttack = false;  
        canHeavyAttack = false;  

        yield return new WaitForSeconds(duration);  

        canLightAttack = true;  
        canHeavyAttack = true;  
    } 
    private void EnableAttackCollider()  
    {  
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("LightAttack"))  
        {  
            StartCoroutine(ActivateAttackCollider(lightAttackCollider, 0.5f)); // Adjust duration as needed  
        }  
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("HeavyAttack"))  
        {  
            StartCoroutine(ActivateAttackCollider(heavyAttackCollider, 0.5f)); // Adjust duration as needed  
        }  
    }  

    private void DisableAttackCollider()  
    {  
        lightAttackCollider.enabled = false;  
        heavyAttackCollider.enabled = false;  
    }  

}  


public struct AttackData  
{  
    public MonoBehaviour Attacker;  
    public int Damage;  
    public bool IsHeavyAttack;  

    public AttackData(MonoBehaviour attacker, int damage, bool isHeavyAttack)  
    {  
        Attacker = attacker;  
        Damage = damage;  
        IsHeavyAttack = isHeavyAttack;  
    }  
}