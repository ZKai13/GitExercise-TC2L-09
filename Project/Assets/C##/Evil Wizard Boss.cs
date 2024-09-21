using UnityEngine;  
using System.Collections;  

public class EvilWizardBoss : MonoBehaviour  
{  
    [Header("Boss Stats")]  
    public int health = 100;
    public float currentHealth;  
    public int maxHealth = 100;  
    public float attackRange = 5f;  
    public float heavyAttackThreshold = 30f;  
    public float fallThreshold = -5f;  
    public float walkSpeed = 3f;
    public GameObject Border1;
    public GameObject Border2;
    public GameObject HealthUI;
    public GameObject audioManager;
    private AudioSource audioSource;
    public AudioClip deathClip;

    [Header("References")]  
    public Transform player;  
    public BossHealthBar bossHealthBar;  

    [Header("Combat Settings")]  
    public float lightAttackCooldown = 2f;  
    public float heavyAttackCooldown = 4f;  
    public float jumpForce = 10f;  
    public float jumpCooldown = 3f;  
    public float lightAttackDamage = 10f;  
    public float heavyAttackDamage = 20f;  
    public float bossDamageReductionFactor = 0.75f;  

    [Header("Ground Check")]  
    public Transform groundCheck;  
    public float groundCheckRadius = 0.2f;  
    public LayerMask groundLayer;  

    private Animator animator;  
    private Rigidbody2D rb2D;  
    private bool canAttack = true;  
    private bool canJump = true;  
    private bool isGrounded;  
    private bool facingRight = true;  
    private PlayerCombat playerCombat; 
    
    [Header("Jump Check")]  
    public LayerMask jumpTriggerLayer;  
    public float jumpCheckDistance = 1f;   
    private bool isHurt = false;  
    private bool isInvulnerable = false;  
    private float invulnerabilityDuration = 1f;
    private bool isStunned = false;
    private BossDeathUI uiManager;  
    private Evil_wizard_fly flyScript;  
    private bool canFly = true;
    public GameObject magicBallPrefab; // Reference to the MagicBall prefab  
    public float magicBallSpeed = 5f; 
    public int magicBallDamage = 2;  
    [SerializeField] private GameObject laserBeam1;  
    [SerializeField] private GameObject laserBeam2;  
    [SerializeField] private GameObject laserBeam3;
    [SerializeField] private GameObject laserBeam4;    
    [SerializeField] private LaserBehavior laserBehavior1;  
    [SerializeField] private LaserBehavior laserBehavior2;  
    [SerializeField] private LaserBehavior laserBehavior3;
    [SerializeField] private LaserBehavior laserBehavior4;
    [SerializeField] private GameObject laserBeam;  
    [SerializeField] private LaserBehavior laserBehavior;     
 

    private void Start()  
    {  
        flyScript = GetComponent<Evil_wizard_fly>();   
        animator = GetComponent<Animator>();  
        rb2D = GetComponent<Rigidbody2D>(); 
        uiManager = FindObjectOfType<BossDeathUI>(); 
        currentHealth = maxHealth;  
        laserBehavior1 = laserBeam1.GetComponent<LaserBehavior>();  
        laserBehavior2 = laserBeam2.GetComponent<LaserBehavior>();  
        laserBehavior3 = laserBeam3.GetComponent<LaserBehavior>(); 
        laserBehavior4 = laserBeam4.GetComponent<LaserBehavior>(); 
        HideLaserBeams();  // Set it to maximum at the start (this should be checked in the Unity Inspector)  
        if (bossHealthBar != null)  
        {  
            float healthPercentage = currentHealth / maxHealth; // Use this for health bar update  
            bossHealthBar.SetHealth(healthPercentage); // Initialize the health bar  
        }  
        HideLaserBeams();  // Ensure lasers are hidden initially  

        if (animator == null || rb2D == null)  
        {  
            Debug.LogError("EvilWizardBoss is missing required components!");  
            enabled = false;  
            return;  
        }  

        if (player == null)  
        {  
            player = GameObject.FindGameObjectWithTag("Player")?.transform;  
            if (player == null)  
            {  
                Debug.LogError("Player not found!");  
                enabled = false;  
                return;  
            }     
        }  

        playerCombat = player.GetComponent<PlayerCombat>();  
        if (playerCombat == null)  
        {  
            Debug.LogError("PlayerCombat component not found on the player!");  
        }  

        if (bossHealthBar == null)  
        {  
            Debug.LogWarning("BossHealthBar is not assigned!");  
        } 

        if (audioManager != null)
        {
            audioSource = audioManager.GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("在 AudioManager GameObject 上未找到 AudioSource 组件。");
            }
        }
        flyScript = GetComponent<Evil_wizard_fly>();
        if (flyScript == null)
        {
            Debug.LogError("Evil_wizard_fly component not found on the boss!");
        }
        
        if (magicBallPrefab == null)  
        {  
            Debug.LogError("MagicBallPrefab is not assigned in the EvilWizardBoss script!");  
        }

        laserBehavior = laserBeam.GetComponent<LaserBehavior>();  
        if (laserBehavior == null)  
        {  
            Debug.LogError("LaserBehavior component not found on the laser beam!");  
        }  
        HideLaserBeams();

    }  

    private void Update()  
    {  
     if (!isStunned)  
    {  
        UpdateAnimatorParameters();  
        HandleMovement();  
        CheckForAttack();  
        CheckForJump();  
        TryStartFlyingIfNeeded();  

        // Check if the boss's health is at or below half  
        if (currentHealth <= maxHealth / 2 && currentHealth > maxHealth / 4)    
        {  
            ShowLaserBeams();  
        }  
        else  
        {  
            HideLaserBeams();  
        }  

        // Check if the boss's health is at or below a quarter  
        if (currentHealth <= maxHealth / 4)  
        {  
            ShootMagicBall();  
        }  
    }   
    } 
    private void ShowLaserBeams()  
    {  
        if (laserBehavior1 != null)  
            laserBehavior1.ShowLaserBeam();  
        if (laserBehavior2 != null)  
            laserBehavior2.ShowLaserBeam();  
        if (laserBehavior3 != null)  
            laserBehavior3.ShowLaserBeam();  
        if (laserBehavior4 != null)  
            laserBehavior4.ShowLaserBeam();  
    }
    private void HideLaserBeams()  
    {  
        if (laserBehavior1 != null)  
            laserBehavior1.HideLaserBeam();  
        if (laserBehavior2 != null)  
            laserBehavior2.HideLaserBeam();  
        if (laserBehavior3 != null)  
            laserBehavior3.HideLaserBeam();  
        if (laserBehavior4 != null)  
            laserBehavior4.HideLaserBeam();  
    }
    private void TryStartFlyingIfNeeded()
    {
        if (flyScript != null && canFly && currentHealth < maxHealth / 2)
        {
            Debug.Log($"Trying to start flying. Current health: {currentHealth}, Max health: {maxHealth}");
            flyScript.TryStartFlying();
            canFly = false;
            StartCoroutine(ResetFlyingCooldown());
        }
    }
    private IEnumerator ResetFlyingCooldown()
    {
        yield return new WaitForSeconds(5f); // Adjust this cooldown as needed
        canFly = true;
    }
      

    private void UpdateAnimatorParameters()  
    {  
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);  
        animator.SetBool("IsGrounded", isGrounded);  
        animator.SetFloat("Speed", Mathf.Abs(rb2D.velocity.x));  
        animator.SetFloat("VerticalSpeed", rb2D.velocity.y);  
        animator.SetBool("IsJumping", !isGrounded && rb2D.velocity.y > 0);  
        animator.SetBool("IsFalling", !isGrounded && rb2D.velocity.y < fallThreshold);  
    }  

    private void HandleMovement()  
    {  
        if (player != null)  
        {  
            float distanceToPlayer = player.position.x - transform.position.x;  
            float moveDirection = Mathf.Sign(distanceToPlayer);  

            if (Mathf.Abs(distanceToPlayer) > attackRange)  
            {  
                rb2D.velocity = new Vector2(moveDirection * walkSpeed, rb2D.velocity.y);  
                animator.SetTrigger("Walk");  
            }  
            else  
            {  
                rb2D.velocity = new Vector2(0, rb2D.velocity.y);  
                animator.SetFloat("Speed", 0);
            }  

            if ((moveDirection > 0 && !facingRight) || (moveDirection < 0 && facingRight))  
            {  
                Flip();  
            }  
        }  
    }  

    private void Flip()  
    {  
        facingRight = !facingRight;  
        Vector3 scale = transform.localScale;  
        scale.x *= -1;  
        transform.localScale = scale;  
    }  

    private void CheckForAttack()  
    {  
        if (canAttack && player != null)  
        {  
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);  
            if (distanceToPlayer <= attackRange)  
            {  
                if (health < heavyAttackThreshold)  
                {  
                    PerformHeavyAttack();  
                }  
                else  
                {  
                    PerformLightAttack();  
                }  
                ShootMagicBall(); 
            }  
        }  
    }  
    private void ShootMagicBall()  
    {  
        MagicBallPool magicBallPool = FindObjectOfType<MagicBallPool>();  
        if (magicBallPool != null)  
        {  
            GameObject magicBall = magicBallPool.GetMagicBall();  
            if (magicBall != null)  
            {  
                // Set the position to the boss  
                magicBall.transform.position = transform.position;  

                // Determine the direction the boss is facing  
                Vector2 shootDirection = facingRight ? Vector2.right : Vector2.left;  

                // Initialize the magic ball with the direction and damage  
                MagicBall ballScript = magicBall.GetComponent<MagicBall>();  
                ballScript.Initialize(shootDirection);  
                ballScript.damage = magicBallDamage; // Set the damage here  

                // Set it active  
                magicBall.SetActive(true);  
            }  
        }  
    }  

    private void CheckForJump()  
    {  
        if (canJump && isGrounded && ShouldJump())  
        {  
            StartCoroutine(PerformJump());  
        }  
    }  

    private bool ShouldJump()  
    {  
        // Check for obstacles  
        RaycastHit2D obstacleHit = Physics2D.Raycast(transform.position, facingRight ? Vector2.right : Vector2.left, jumpCheckDistance, groundLayer);  
        
        // Check for jump trigger  
        RaycastHit2D jumpTriggerHit = Physics2D.Raycast(transform.position, facingRight ? Vector2.right : Vector2.left, jumpCheckDistance, jumpTriggerLayer);  

        // Draw debug rays  
        Debug.DrawRay(transform.position, (facingRight ? Vector2.right : Vector2.left) * jumpCheckDistance, Color.red);  

        return obstacleHit.collider != null || jumpTriggerHit.collider != null;  
    }  

    private IEnumerator PerformJump()  
    {  
        canJump = false;  
        animator.SetTrigger("Jump");  
        rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);  
        yield return new WaitForSeconds(jumpCooldown);  
        canJump = true;  
    }  

    private void PerformLightAttack()  
    {  
        Debug.Log("Performing Light Attack");  
        animator.SetTrigger("LightAttack");  
        StartCoroutine(AttackCoroutine(lightAttackDamage, lightAttackCooldown));  
    }  

    private void PerformHeavyAttack()  
    {  
        Debug.Log("Performing Heavy Attack");  
        animator.SetTrigger("HeavyAttack");  
        StartCoroutine(AttackCoroutine(heavyAttackDamage, heavyAttackCooldown));  
    }  

    private IEnumerator AttackCoroutine(float damage, float cooldown)  
    {  
        canAttack = false;  
        yield return new WaitForSeconds(0.5f);  
        AttackPlayer(damage);  
        yield return new WaitForSeconds(cooldown - 0.5f);  
        canAttack = true;  
//        Debug.Log("Boss can attack again");  
    }  

    private void AttackPlayer(float damage)  
    {  
        if (playerCombat != null && player != null)  
        {  
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);  
            if (distanceToPlayer <= attackRange)  
            {  
                float reducedDamage = damage * bossDamageReductionFactor;  
                bool isHeavyAttack = damage == heavyAttackDamage; // Determine if it's a heavy attack  
                int roundedDamage = Mathf.RoundToInt(reducedDamage); // Convert float to int  
//                Debug.Log($"Boss attacking player. Base damage: {damage}, Reduced damage: {reducedDamage}, Rounded damage: {roundedDamage}, Is Heavy Attack: {isHeavyAttack}");  
                playerCombat.ReceiveAttack(this, roundedDamage, isHeavyAttack);  
            }  
            else  
            {  
                Debug.Log($"Player out of range. Distance: {distanceToPlayer}, Attack Range: {attackRange}");  
            }  
        }  
        else  
        {  
            Debug.LogError("PlayerCombat or player reference is null!");  
        }  
    }
    public void Takedamage(int damage)  
    {  
        if (isStunned) return;  

        currentHealth = Mathf.Max(0, currentHealth - damage);
        health = (int)currentHealth; // Update the 'health' variable to match 'currentHealth'
       // Debug.Log($"Boss took {damage} damage. Current health: {currentHealth}");
        
        if (bossHealthBar != null)  
        {  
            float healthPercentage = (float)currentHealth / maxHealth;
            bossHealthBar.SetHealth(healthPercentage);
        }  

        StartCoroutine(HitReaction());  

        if (health <= 0)  
        {  
            Die();  
        }  
    }  
    private IEnumerator HitReaction()  
    {  
        isInvulnerable = true;  
        animator.SetTrigger("Hurt");
        animator.SetBool("Stunned", true);   

        // Apply a small knockback  
        float knockbackForce = 5f;  
        Vector2 knockbackDirection = (player.position.x > transform.position.x) ? Vector2.left : Vector2.right;  
        rb2D.velocity = Vector2.zero;  
        rb2D.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);  

        yield return new WaitForSeconds(0.5f);  // Short stun duration  

        // Force the boss back to the ground  
        rb2D.velocity = Vector2.zero;  
        yield return new WaitForSeconds(0.1f);  

        animator.SetBool("Stunned", false);  
        animator.SetBool("IsJumping", false);  
        animator.SetBool("IsFalling", false);  
        animator.SetFloat("VerticalSpeed", 0);  
        animator.SetFloat("Speed", 0);  

        isStunned = false; 
    } 


    private IEnumerator HurtRoutine()  
    {  
        isHurt = true;  
        rb2D.velocity = new Vector2(0, rb2D.velocity.y); // Stop horizontal movement  
        yield return new WaitForSeconds(0.5f); // Adjust this time based on your hurt animation length  
        isHurt = false;  
        ResetState();  
    }  
    private void ResetState()  
    {  
        animator.SetBool("IsJumping", false);  
        animator.SetBool("IsFalling", false);  
        animator.SetFloat("Speed", 0);  
        animator.SetFloat("VerticalSpeed", 0);  
    }  
    private void OnCollisionEnter2D(Collision2D collision)  
    {  
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))  
        {  
            animator.SetBool("IsGrounded", true);  
            animator.SetBool("IsJumping", false);  
            animator.SetBool("IsFalling", false);  
            animator.SetFloat("VerticalSpeed", 0);  
        }  
    } 

    private void Die()  
    {  
        Debug.Log("Boss is dying");  
        animator.SetTrigger("Die");  
        enabled = false;
        Destroy(Border1);
        Destroy(Border2);
        Destroy(HealthUI);
        StartCoroutine(ShowAndHideUIWithDelay());

        
    }  

    private IEnumerator ShowAndHideUIWithDelay()
    {
        yield return new WaitForSeconds(3f);
        uiManager.ShowBossUI();
        yield return new WaitForSeconds(3f);
        uiManager.HideBossUI();
        Destroy(gameObject);

        if (audioSource != null && deathClip != null)
        {
            audioSource.clip = deathClip;
            audioSource.Play();
        }
    }

    private void OnDrawGizmosSelected()  
    {  
        Gizmos.color = Color.red;  
        Gizmos.DrawWireSphere(transform.position, attackRange);  

        if (groundCheck != null)  
        {  
            Gizmos.color = Color.green;  
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);  
        }  
        Gizmos.color = Color.blue;  
        Gizmos.DrawRay(transform.position, (facingRight ? Vector2.right : Vector2.left) * jumpCheckDistance);  
    }  
}