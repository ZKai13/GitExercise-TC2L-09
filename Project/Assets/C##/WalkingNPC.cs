using System.Collections;  
using System.Collections.Generic;  
using UnityEngine;  

public class WalkingNPC : MonoBehaviour  
{  
    [SerializeField] private GameObject[] waypoints;  
    private int currentWaypointIndex = 0;  

    [SerializeField] private float speed = 2f;  

    private Vector3 originalScale;

    private void Start()  
    {  
        originalScale = transform.localScale;  
    }  

    private void Update()  
    {  
        
        Vector2 targetPosition = waypoints[currentWaypointIndex].transform.position;  

        
        if (Vector2.Distance(targetPosition, transform.position) < .1f)  
        {  
            currentWaypointIndex++;  
            if (currentWaypointIndex >= waypoints.Length)  
            {  
                currentWaypointIndex = 0;  
            }  
        }  

    
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);  

        
        if (transform.position.x < targetPosition.x)  
        {  
            
            transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
        }  
        else if (transform.position.x > targetPosition.x)  
        {  
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
        }   
    }  
}