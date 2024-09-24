using UnityEngine;  

public class MagicBall : MonoBehaviour  
{  
     // 魔法球相关属性 
    public float speed = 5f; // Speed of the magic ball  
    public float lifetime = 20f; // Lifetime of the magic ball before it is returned to the pool  
    public int damage = 2; // Damage dealt by the magic ball  

    private Vector2 direction; // Direction towards the player  

    // 初始化魔法球,设置发射方向和速度 
    public void Initialize(Vector2 shootDirection)  
    {  
        direction = shootDirection.normalized; // 规范化方向向量,确保速度恒定 
        GetComponent<Rigidbody2D>().velocity = direction * speed; // 设置速度   
    }  

    private void OnEnable()  
    {  
        // 当魔法球被激活时,开始生命周期倒计时   
        Invoke("ReturnToPool", lifetime);  
    }  

    private void OnDisable()  
    {  
        // 当魔法球被禁用时,取消生命周期倒计时   
        CancelInvoke();  
    }  

    private void ReturnToPool()  
    {  
        // 获取魔法球对象池引用,并将该魔法球返回到对象池中   
        MagicBallPool magicBallPool = FindObjectOfType<MagicBallPool>();  
        if (magicBallPool != null)  
        {  
            magicBallPool.ReturnMagicBall(gameObject); // Return this magic ball to the pool  
        }  
    }  

    private void OnCollisionEnter2D(Collision2D collision)  
    {  
        Debug.Log("MagicBall collided with " + collision.gameObject.name);  // Log collision details  
        
        // 检查是否与玩家发生碰撞 
        if (collision.gameObject.CompareTag("Player"))  
        {  
            PlayerCombat playerCombat = collision.gameObject.GetComponent<PlayerCombat>();  
            if (playerCombat != null)  
            {  
                playerCombat.ReceiveAttack(null, damage, false);  // Inflict damage to the player  
            }  
            ReturnToPool();  // 将魔法球返回对象池 
        }  
        // Check for collision with the Ground or Obstacle  
        else if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Obstacle"))  
        {  
            ReturnToPool();  // 将魔法球返回对象池  
        }  
    }
}
