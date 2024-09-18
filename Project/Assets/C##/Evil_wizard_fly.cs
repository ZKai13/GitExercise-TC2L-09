using System.Collections;
using UnityEngine;

public class Evil_wizard_fly : MonoBehaviour
{
    public float flyingHeight = 5f;
    public float flyingSpeed = 10f; // Increased flying speed
    public float circleRadius = 5f;

    private bool isFlying = false;
    private EvilWizardBoss bossScript;
    private Rigidbody2D rb2D;
    private Animator animator;
    private Transform player;

    private void Start()
    {
        bossScript = GetComponent<EvilWizardBoss>();
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = bossScript.player;

        // Ensure the Rigidbody2D is not kinematic and gravity is enabled
        if (rb2D != null)
        {
            rb2D.isKinematic = false;
            rb2D.gravityScale = 1f;
        }
    }

    public void TryStartFlying()
    {
        if (bossScript.currentHealth < bossScript.maxHealth / 2 && !isFlying)
        {
            Debug.Log("Boss health below half, trying to start flying");
            StartFlying();
        }
        else
        {
            Debug.Log("Boss health not below half, not flying");
        }
    }

    public void StopFlying()
    {
        if (isFlying)
        {
            StopCoroutine(FlyAroundPlayer());
            isFlying = false;
            // Re-enable gravity when stopping flight
            rb2D.gravityScale = 1f;
        }
    }

    private void StartFlying()
    {
        StartCoroutine(FlyAroundPlayer());
    }

    private IEnumerator FlyAroundPlayer()
    {
        isFlying = true;

        // Disable jumping and falling animations
        animator.SetBool("IsJumping", false);
        animator.SetBool("IsFalling", false);

        // Disable gravity while flying
        rb2D.gravityScale = 0f;

        // Fly up to the specified height
        float startY = transform.position.y;
        float targetY = startY + flyingHeight;
        while (transform.position.y < targetY)
        {
            Vector2 newPosition = new Vector2(transform.position.x, transform.position.y + flyingSpeed * Time.deltaTime);
            rb2D.MovePosition(newPosition);
            Debug.Log($"Flying upwards. Current height: {transform.position.y}, Target height: {targetY}");
            yield return null;
        }

        Debug.Log("Reached target height, starting circular movement");

        // Fly around the player in a circular pattern
        float angle = 0f;
        while (isFlying)
        {
            float x = player.position.x + Mathf.Cos(angle * Mathf.Deg2Rad) * circleRadius;
            float y = player.position.y + Mathf.Sin(angle * Mathf.Deg2Rad) * circleRadius + flyingHeight;
            Vector2 targetPosition = new Vector2(x, y);

            rb2D.MovePosition(Vector2.Lerp(rb2D.position, targetPosition, flyingSpeed * Time.deltaTime));

            angle += 45f * Time.deltaTime;
            yield return null;

            // Check if the boss's health is above or equal to half and stop flying
            if (bossScript.currentHealth >= bossScript.maxHealth / 2)
            {
                StopFlying();
                break;
            }
        }

        // Land back on the ground
        while (transform.position.y > startY)
        {
            Vector2 newPosition = new Vector2(transform.position.x, transform.position.y - flyingSpeed * Time.deltaTime);
            rb2D.MovePosition(newPosition);
            yield return null;
        }

        // Re-enable gravity
        rb2D.gravityScale = 1f;

        transform.position = new Vector3(transform.position.x, startY, transform.position.z);
        isFlying = false;
    }
}