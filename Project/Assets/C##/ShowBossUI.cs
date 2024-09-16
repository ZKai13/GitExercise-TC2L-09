using System.Collections;  
using System.Collections.Generic;  
using UnityEngine;  

public class ShowBossUI : MonoBehaviour  
{  
    public CanvasGroup BossCanvasGroup;  

    private void OnTriggerEnter2D(Collider2D other)  
    {  
        if (other.CompareTag("Player"))   
        {  
            ShowHealthUI();
            Destroy(gameObject);   
        }  
    }  

    public void ShowHealthUI()  
    {  
        BossCanvasGroup.alpha = 1;   
    }  
}