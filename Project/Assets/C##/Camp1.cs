using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camp1 : MonoBehaviour
{  
    private bool isNearInteractable = false;  
    public Health playerHealth;
    private bool used = false;

    void Update()  
    {  
        if (isNearInteractable && Input.GetKeyDown(KeyCode.F) && !used)  
        {  
            Interact();  
        }  
    }

     private void Interact()
    {  
        if (playerHealth != null)  
        {  
            playerHealth.Heal(20);
            used = true;
        }  
    }  

    private void OnTriggerEnter2D(Collider2D other)  
    {  
        if (other.CompareTag("Player")) 
        {  
            isNearInteractable = true;  
        }  
    }  

    private void OnTriggerExit2D(Collider2D other)  
    {  
        if (other.CompareTag("Player"))  
        {  
            isNearInteractable = false;  
        }  
    }  
}


