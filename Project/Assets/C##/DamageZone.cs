using UnityEngine;  

public class DamageZone : MonoBehaviour  
{  
    public float damageAmount = 1f;
    public float cooldownTime = 1f;
    private float lastDamageTime;

    private void OnTriggerEnter2D(Collider2D other)  
    {  
        if (other.CompareTag("Player"))
        {  
     
            if (Time.time >= lastDamageTime + cooldownTime)  
            {  
                Health playerHealth = other.GetComponent<Health>();
                if (playerHealth != null)  
                {  
                    playerHealth.Takedamage(damageAmount);  
                    lastDamageTime = Time.time; //
                }  
            }  
        }  
    }  
}