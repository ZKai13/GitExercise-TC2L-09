using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level13TP : MonoBehaviour
{  
    public Vector2 teleportPosition;

    private void OnTriggerEnter2D(Collider2D other)  
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.position = teleportPosition;   
            Destroy(gameObject);
        }
    }  
}
