using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapL3 : MonoBehaviour
{
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
             
        }  
    }  
}

