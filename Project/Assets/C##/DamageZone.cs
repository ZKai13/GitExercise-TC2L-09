using UnityEngine;  

public class DamageZone : MonoBehaviour  
{  
    public float damageAmount = 1f; // 扣除的伤害量  
    public float cooldownTime = 1f; // 冷却时间  
    private float lastDamageTime; // 上次造成伤害的时间  

    private void OnTriggerEnter2D(Collider2D other)  
    {  
        if (other.CompareTag("Player")) // 确保是玩家角色  
        {  
            // 检查冷却时间  
            if (Time.time >= lastDamageTime + cooldownTime)  
            {  
                Health playerHealth = other.GetComponent<Health>(); // 获取 Health 组件  
                if (playerHealth != null)  
                {  
                    playerHealth.Takedamage(damageAmount); // 扣除生命  
                    lastDamageTime = Time.time; // 更新上次造成伤害的时间  
                }  
            }  
        }  
    }  
}