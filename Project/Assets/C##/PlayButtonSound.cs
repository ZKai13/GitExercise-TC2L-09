using UnityEngine;
using UnityEngine.UI;

public class PlayButtonSound : MonoBehaviour
{
    public Button playButton; // Assign this in the Unity Editor
    public AudioClip clip;    // Assign the AudioClip you want to play

    private AudioSource audioSource;

    void Start()
    {
        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();
        
        if (playButton != null && audioSource != null && clip != null)
        {
            playButton.onClick.AddListener(PlaySound);
        }
        else
        {
            Debug.LogError("Ensure Button, AudioSource, and AudioClip are assigned.");
        }
    }
    

    void PlaySound()
    {
        // Play the AudioClip when the button is clicked
        audioSource.PlayOneShot(clip);
    }
}