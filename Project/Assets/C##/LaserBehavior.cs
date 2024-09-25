using UnityEngine; 
using System.Collections; 

public class LaserBehavior : MonoBehaviour  
{  
    public float damageAmount   = 10f; // 激光造成的伤害  
    private Coroutine laserCoroutine; // 用于管理激光的激活时间的协程   

    private void Start()  
    {  
        // 确保激光一开始是不活跃的,需要通过命令才能激活 
        gameObject.SetActive(false); 
         
    }

    private void OnTriggerEnter2D(Collider2D collision)  
    {  
        Debug.Log("Laser collided with: " + collision.gameObject.name); // 记录激光的碰撞信息
        // 检查是否与玩家发生碰撞   
        if (collision.CompareTag("Player"))  
        {  
            Debug.Log("Laser hit the player!"); // 确认激光击中了玩家  

            Health playerHealth = collision.GetComponent<Health>(); 
            // 获取玩家的Health组件,并对其造成伤害   
            if (playerHealth != null)  
            {  
                Debug.Log("Applying damage to the player: " + damageAmount);  
                playerHealth.Takedamage((int)damageAmount); // Apply damage to the player as int  
            }  
            else  
            {  
                Debug.LogError("Health component not found on player!");  
            }  
        }  
    }

    public void ShowLaserBeam()  
    {  
        gameObject.SetActive(true);  // 激活激光         
        // 可选择启动一个协程来管理激光的持续时间   
        if (laserCoroutine != null)  
        {  
            StopCoroutine(laserCoroutine);  
        }  
        laserCoroutine = StartCoroutine(DeactivateLaserAfterTime(5f)); // 示例持续时间为5秒 
    }  

    public void HideLaserBeam()  
    {  
        // 禁用激光 
        gameObject.SetActive(false);  
    }  

    private IEnumerator DeactivateLaserAfterTime(float time)  
    {  
        // 在指定时间后自动禁用激光 
        yield return new WaitForSeconds(time);  
        HideLaserBeam();   
    }  
}