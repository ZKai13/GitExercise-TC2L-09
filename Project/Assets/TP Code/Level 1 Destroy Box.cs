using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1DestroyBox : MonoBehaviour

{  
    public GameObject box;

    private void OnTriggerEnter2D(Collider2D other)  
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(box);
            Destroy(gameObject);
        }  
    }  
}

