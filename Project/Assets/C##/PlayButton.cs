using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{

    private AudioManager audioManager; // Declare audioManager as a private field within the class

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // This function will be called by the button to change scenes
    public void GoToScene(string sceneName)
    {
        // Load the scene with the specified name
        audioManager.PlaySFX(audioManager.buttonClick);
        SceneManager.LoadScene(sceneName);
        
    }
}