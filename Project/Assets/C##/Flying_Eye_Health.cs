using UnityEngine;  

public class Flying_Eye_Health : MonoBehaviour  
{  
    public float maxHealth = 100f;  
    private float currentHealth;  

    private void Start()  
    {  
        currentHealth = maxHealth; 
    }  

    public void TakeDamage(int damage)  
    {  
        currentHealth -= damage; // Reduce current health by damage amount  
        Debug.Log($"Mushroom took {damage} damage. Current health: {currentHealth}");  

        if (currentHealth <= 0)  
        {  
            Die(); // Call die method if health is zero or below  
        }  
    } 

    private void Die()  
    {  
        //Destroy(gameObject);  
    }  

    public float GetHealthPercentage()  
    {  
        return currentHealth / maxHealth;  
    }  
}