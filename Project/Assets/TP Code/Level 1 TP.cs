using UnityEngine;  
using UnityEngine.SceneManagement;  
using System.Collections;  

public class Teleporter : MonoBehaviour  
{  
    public GameObject PLAYER;  
    private Animator anim;

    void Start()  
    {  
        //anim = GetComponent<Animator>();  
    }  

    private void OnTriggerEnter2D(Collider2D other)  
    {
        Animator playerAnimator = other.gameObject.GetComponent<Animator>();  
        if (playerAnimator != null)  
        {  
            
        }  
        if (other.gameObject.CompareTag("Player"))  
        {      
            Health playerHealth = other.GetComponent<Health>();
                if (playerHealth != null)  
                {  
                    playerHealth.Takedamage(20);  
                } 
        }  
    }  
}