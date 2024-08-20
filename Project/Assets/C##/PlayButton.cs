using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    // This function will be called by the button to change scenes
    public void GoToScene(string sceneName)
    {
        // Load the scene with the specified name
        SceneManager.LoadScene(sceneName);
    }
}