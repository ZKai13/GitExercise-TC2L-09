using UnityEngine;  
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour  
{  
    
    public void RestartGame()  
    {   
        string currentSceneName = SceneManager.GetActiveScene().name;  
        SceneManager.LoadScene(currentSceneName);  
    }  

    public void LoadMainMenu()  
    {  
        SceneManager.LoadScene("MainMenu");  
    }  
}