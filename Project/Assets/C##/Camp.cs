using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camp : MonoBehaviour
{  
    private bool isNearInteractable = false;  
    public Health playerHealth;
    public GameObject Square1;

    void Update()  
    {  
        if (isNearInteractable && Input.GetKeyDown(KeyCode.P))  
        {  
            Interact();  
        }  
    }

     private void Interact()
    {  
        if (playerHealth != null)  
        {  
            playerHealth.Heal(20);
            Destroy(Square1);
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
