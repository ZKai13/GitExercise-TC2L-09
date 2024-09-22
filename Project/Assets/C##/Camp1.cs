using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camp1 : MonoBehaviour
{  
    private bool isNearInteractable = false;  
    public Health playerHealth;
    private bool used = false;


    private PopUp popUp;
    private void Start()
    {
        popUp = FindObjectOfType<PopUp>();
    }
    void Update()  
    {  
        if (isNearInteractable && Input.GetKeyDown(KeyCode.F) && !used)  
        {  
            Interact();
            if (!PlayerPrefs.HasKey("AhhYes"))
            {
                PlayerPrefs.SetInt("AhhYes", PlayerPrefs.GetInt("AhhYes", 0) + 1); // Achievement unlocked
                PlayerPrefs.Save(); // Ensure changes are saved
                Debug.Log("Achievement Unlocked: Ahh..Yes, The campfire");
                popUp.DisplayAchievement(popUp.campfireSprite);
            }  
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


