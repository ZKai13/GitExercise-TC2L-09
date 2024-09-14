using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camp : MonoBehaviour
{  
    private bool isNearInteractable = false;
    private bool hasInteracted = false;  
    public Health playerHealth;
    public PotionScript potionScript;

    void Update()  
    {  
        if (isNearInteractable && Input.GetKeyDown(KeyCode.F) && !hasInteracted)  
        {  
            Interact();  
        }  
    }

     private void Interact()
    {  
        if (playerHealth != null)  
        {  
            playerHealth.Heal(20);
            potionScript.ResetPotionCount();
            hasInteracted = true;
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
