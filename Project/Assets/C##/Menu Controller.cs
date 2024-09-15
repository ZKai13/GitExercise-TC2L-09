using UnityEngine;  
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour  
{  
    
    public void RestartGame()  
    {   
        string currentSceneName = SceneManager.GetActiveScene().name;  
        SceneManager.LoadScene(currentSceneName);  
        Time.timeScale = 1;
    }  

    // 加载主菜单的方法  
    public void LoadMainMenu()  
    {  
        SceneManager.LoadScene("MainMenu");  
    }  
}