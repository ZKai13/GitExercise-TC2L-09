using UnityEngine;  

public class Teleporter : MonoBehaviour  
{  
    public Vector2 teleportPosition;
    public Health playerHealth;

    private void OnTriggerEnter2D(Collider2D other)  
    {  
        if (other.CompareTag("Player")) 
        {  
            
            Health playerHealth = other.GetComponent<Health>();  
            if (playerHealth != null)  
            {  
                playerHealth.Takedamage(20);
            }  
            
            other.transform.position = teleportPosition;   
        }  
    }  
}