using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level12TP : MonoBehaviour
{  
    public Vector2 teleportPosition;

    private void OnTriggerEnter2D(Collider2D other)  
    {
        {   
            other.transform.position = teleportPosition;   
        }  
    }  
}