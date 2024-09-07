using System.Collections;  
using System.Collections.Generic;  
using UnityEngine;  

public class WalkingNPC : MonoBehaviour  
{  
    [SerializeField] private GameObject[] waypoints;  
    private int currentWaypointIndex = 0;  

    [SerializeField] private float speed = 2f;  

    private Vector3 originalScale; // 存储原始缩放值  

    private void Start()  
    {  
        // 在开始时保存原始缩放值  
        originalScale = transform.localScale;  
    }  

    private void Update()  
    {  
        // 获得目标位置  
        Vector2 targetPosition = waypoints[currentWaypointIndex].transform.position;  

        // 检查与目标点的距离  
        if (Vector2.Distance(targetPosition, transform.position) < .1f)  
        {  
            currentWaypointIndex++;  
            if (currentWaypointIndex >= waypoints.Length)  
            {  
                currentWaypointIndex = 0;  
            }  
        }  

        // 移动到目标点  
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);  

        // 根据目标位置调整角色朝向  
        if (transform.position.x < targetPosition.x)  
        {  
            // 如果目标在右侧，朝向右  
            transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z); // 面朝右  
        }  
        else if (transform.position.x > targetPosition.x)  
        {  
            // 如果目标在左侧，朝向左  
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z); // 面朝左  
        }   
    }  
}