using UnityEngine;  

public class LaserBehavior : MonoBehaviour  
{  
    public float damageAmount = 10f;  

    private void OnTriggerEnter2D(Collider2D collision)  
    {  
        // Check if the colliding object is the player  
        if (collision.CompareTag("Player"))  
        {  
            // Apply damage to the player  
            collision.GetComponent<Health>().Takedamage(damageAmount);  
        }  
    }  
}