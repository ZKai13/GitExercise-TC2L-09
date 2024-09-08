using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Falloutmap3 : MonoBehaviour
{  
    public Transform player;

    void Update()  
    {  
    
        if (player.position.y < -14)
        {  
            RestartScene();  
        }  
    }  

    void RestartScene()  
    {  
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  
    }  
}
