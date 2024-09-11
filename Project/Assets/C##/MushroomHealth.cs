using System.Collections;  
using System.Collections.Generic;  
using UnityEngine;  

public class MushroomHealth : MonoBehaviour  
{  
    public int maxHealth = 100; // Maximum health of the mushroom  
    private int currentHealth; // Current health of the mushroom  

    void Start()  
    {  
        currentHealth = maxHealth; // Initialize current health  
    }  

    // Method to take damage  
    public void TakeDamage(int damage)  
    {  
        currentHealth -= damage; // Reduce current health by damage amount  
        Debug.Log($"Mushroom took {damage} damage. Current health: {currentHealth}");  

        if (currentHealth <= 0)  
        {  
            Die(); // Call die method if health is zero or below  
        }  
    }  

    // Method to handle mushroom death  
    void Die()  
    {  
        Debug.Log("Mushroom died.");  
        // Add death logic here (e.g., play death animation, destroy object)  
        Destroy(gameObject); // Destroy the mushroom object  
    }  
}