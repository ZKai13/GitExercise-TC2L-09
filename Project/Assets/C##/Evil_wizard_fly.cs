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
        float angle = 0f;//这行代码初始化一个变量 angle 为 0
        while (isFlying)
        {
            float x = player.position.x + Mathf.Cos(angle * Mathf.Deg2Rad) * circleRadius;
            //x 坐标正在计算
            //Adding the result of the cosine function, which is based on the 
            //current angle (converted from degrees to radians) multiplied by the circleRadius. 
            //This creates the circular movement around the player
            float y = player.position.y + Mathf.Sin(angle * Mathf.Deg2Rad) * circleRadius + flyingHeight;
            //An additional offset based on the sine function (also converted from degrees to radians) multiplied by the circleRadius

            Vector2 targetPosition = new Vector2(x, y);

            rb2D.MovePosition(Vector2.Lerp(rb2D.position, targetPosition, flyingSpeed * Time.deltaTime));
            //这一行将法师的刚体移动到 targetPosition。Lerp 函数创建了一个平滑的过渡：

            angle += 45f * Time.deltaTime;
            //angle 每秒增量为 45f 度。这个逐渐增加使法师在一段时间内绕着玩家完成一个圆形路径。Time.deltaTime 确保了无论帧率如何，增量都是一致的
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