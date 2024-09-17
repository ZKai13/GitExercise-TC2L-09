using UnityEngine;  
using System.Collections;  

public class FlyingEye : MonoBehaviour  
{  
    public float health = 100f;  
    public float lightAttackCooldown = 2f;  
    public float heavyAttackCooldown = 7f;  
    public float lightAttackChance = 0.7f;  
    public float moveSpeed = 3f;  

    public int lightAttackDamage = 10;  
    public int heavyAttackDamage = 25;  

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

        Debug.Log($"Distance: {distanceToPlayer}, Light Range: {playerInLightAttackRange}, Heavy Range: {playerInHeavyAttackRange}, Can Light: {canLightAttack}, Can Heavy: {canHeavyAttack}");  

        if (playerInHeavyAttackRange && canHeavyAttack && !isPerformingHeavyAttack)  
        {  
            Debug.Log("Initiating Heavy Attack");  
            StartCoroutine(PerformHeavyAttackSequence());  
        }  
        else if (playerInLightAttackRange && canLightAttack)  
        {  
            Debug.Log("Performing Light Attack");  
            PerformLightAttack();  
        }  
        else if (!isPerformingHeavyAttack)  
        {  
            MoveTowardsPlayer();  
            PlayIdle();  
        }  

        FacePlayer();  
        RotateBody();  
    }  

    private void MoveTowardsPlayer()  
    {  
        if (isPerformingHeavyAttack) return;  

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

    private IEnumerator ActivateAttackCollider(Collider2D attackCollider, float duration)  
    {  
        attackCollider.enabled = true;  
        yield return new WaitForSeconds(duration);  
        attackCollider.enabled = false;  

        // Apply damage if player is still in range  
        if (attackCollider.IsTouching(player.GetComponent<Collider2D>()))  
        {  
            ApplyDamage(attackCollider == lightAttackCollider ? lightAttackDamage : heavyAttackDamage);  
        }  
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

    public void TakeDamage(int amount)  
    {  
        health -= amount;  
        if (health <= 0)  
        {  
            Die();  
        }  
        else  
        {  
            animator.SetTrigger(HurtHash);  
        }  
    }  

    private void Die()  
    {  
        animator.SetTrigger(DieHash);  
        GetComponent<Collider2D>().enabled = false;  
        this.enabled = false;  
    }  

    private void ApplyDamage(int damage)  
    {  
        if (player != null)  
        {  
            player.SendMessage("ReceiveAttack", new AttackData(this, damage, damage == heavyAttackDamage), SendMessageOptions.DontRequireReceiver);  
            Debug.Log($"Applied {damage} damage to player");  
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