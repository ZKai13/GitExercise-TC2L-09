using UnityEngine;

public class QuitButton : MonoBehaviour
{
    
    private AudioManager audioManager; // Declare audioManager as a private field within the class

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void Quit()
    {
        
        Application.Quit();
        audioManager.PlaySFX(audioManager.buttonClick);


#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}


