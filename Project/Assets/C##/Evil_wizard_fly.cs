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
        // 获取所需的引用  
        bossScript = GetComponent<EvilWizardBoss>();
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = bossScript.player;

        // 确保刚体不是运动学的,并启用重力  
        if (rb2D != null)
        {
            rb2D.isKinematic = false;
            rb2D.gravityScale = 1f;
        }
    }

    public void TryStartFlying()
    {
        // 在Boss血量低于一半时尝试开始飞行  
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
         // 停止飞行  
        if (isFlying)
        {
            StopCoroutine(FlyAroundPlayer());
            isFlying = false;
            // 重新启用重力
            rb2D.gravityScale = 1f;
        }
    }

    private void StartFlying()
    {
        // 开始执行飞行协程 
        StartCoroutine(FlyAroundPlayer());
    }

    private IEnumerator FlyAroundPlayer()
    {
        // 禁用跳跃和下落动画  
        isFlying = true;

        // Disable jumping and falling animations
        animator.SetBool("IsJumping", false);
        animator.SetBool("IsFalling", false);

        // 飞行时禁用重力  
        rb2D.gravityScale = 0f;

        // 飞升到指定高度
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

        // 围绕玩家进行环形飞行 
        float angle = 0f;
        while (isFlying)
        {
            float x = player.position.x + Mathf.Cos(angle * Mathf.Deg2Rad) * circleRadius;
            float y = player.position.y + Mathf.Sin(angle * Mathf.Deg2Rad) * circleRadius + flyingHeight;
            Vector2 targetPosition = new Vector2(x, y);

            rb2D.MovePosition(Vector2.Lerp(rb2D.position, targetPosition, flyingSpeed * Time.deltaTime));

            angle += 45f * Time.deltaTime;
            yield return null;

            // 检查Boss的血量是否恢复到一半及以上,如果是则停止飞行 
            if (bossScript.currentHealth >= bossScript.maxHealth / 2)
            {
                StopFlying();
                break;
            }
        }

        // 降落回地面 
        while (transform.position.y > startY)
        {
            Vector2 newPosition = new Vector2(transform.position.x, transform.position.y - flyingSpeed * Time.deltaTime);
            rb2D.MovePosition(newPosition);
            yield return null;
        }

        // 重新启用重力
        rb2D.gravityScale = 1f;

        transform.position = new Vector3(transform.position.x, startY, transform.position.z);
        isFlying = false;
    }
}