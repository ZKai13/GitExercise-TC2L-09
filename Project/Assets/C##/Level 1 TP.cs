using UnityEngine;  

public class Teleporter : MonoBehaviour  
{  
    public Vector2 teleportPosition;

    private void OnTriggerEnter2D(Collider2D other)  
    {  
        if (other.CompareTag("Player")) 
        {  
            // 获取 Health 组件  
            Health playerHealth = other.GetComponent<Health>();  
            if (playerHealth != null)  
            {  
                playerHealth.Takedamage(1);
            }  
            
            other.transform.position = teleportPosition;   
        }  
    }  
}