using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingTrigger : MonoBehaviour
{
    private Ending gameManager; 

    void Start()
    {
        gameManager = FindObjectOfType<Ending>();
    }

    private void OnTriggerEnter2D(Collider2D other)  
    {  
        if (other.CompareTag("Player")) 
        {  
            gameManager.ShowEnding1UI();
        }  
    }  
}
