using UnityEngine;  
using UnityEngine.SceneManagement;  
using System.Collections;  

public class Teleporter : MonoBehaviour  
{  
    public GameObject PLAYER;  
    private Animator anim;  
    private PlayerCombat playercombat;
   

    void Start()  
    {  
        //anim = GetComponent<Animator>();  
        playercombat = FindObjectOfType<PlayerCombat>(); 
        if (playercombat == null)  
        {  
            Debug.LogError("PlayerCombat component not found!");  
        }  
    }  

    private void OnTriggerEnter2D(Collider2D other)  
    {  
        if (other.gameObject.CompareTag("Player"))  
        {  
            Animator playerAnimator = other.gameObject.GetComponent<Animator>();  
            //if (playerAnimator != null)  
            //{  
              
            //}  

            Health playerHealth = other.GetComponent<Health>();  
            if (playerHealth != null)  
            {  
                playerHealth.Takedamage(100);  
            }  

             
            
        }  
    }  


}