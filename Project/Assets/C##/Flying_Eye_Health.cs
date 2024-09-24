using UnityEngine;  

public class Flying_Eye_Health : MonoBehaviour  
{  
    public float maxHealth = 100f;  
    private float currentHealth;  

    private void Start()  
    {  
        currentHealth = maxHealth;  
    }  

    public void TakeDamage(float amount)  
    {  
        currentHealth -= amount;  
        if (currentHealth <= 0)  
        {  
            Die();  
        }  
    }  

    private void Die()  
    {  
        // Implement the death logic for the Flying Eye  
        Destroy(gameObject);  
    }  

    public float GetHealthPercentage()  
    {  
        return currentHealth / maxHealth;  
    }  
}