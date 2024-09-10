using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioSettings : MonoBehaviour
{
    public Slider musicSlider; // Assign this in the Unity Inspector
    public Slider sfxSlider;   // Assign this in the Unity Inspector
    public AudioMixer myMixer; // Assign this in the Unity Inspector for both music and SFX groups

    private void Start()
    {
        // Load music volume
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        }
        else
        {
            musicSlider.value = 0.5f; // Default volume
        }

        // Load SFX volume
        if (PlayerPrefs.HasKey("sfxVolume"))
        {
            sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
        }
        else
        {
            sfxSlider.value = 0.5f; // Default volume
        }

        SetMusicVolume();
        SetSFXVolume();

        // Add listeners for when sliders are changed
        musicSlider.onValueChanged.AddListener(delegate { SetMusicVolume(); });
        sfxSlider.onValueChanged.AddListener(delegate { SetSFXVolume(); });
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        myMixer.SetFloat("music", Mathf.Log10(volume) * 20); // Assuming "music" is the exposed parameter in your AudioMixer for music volume

        PlayerPrefs.SetFloat("musicVolume", volume);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume()
    {
        float volume = sfxSlider.value;
        myMixer.SetFloat("sfx", Mathf.Log10(volume) * 20); // Assuming "sfx" is the exposed parameter in your AudioMixer for SFX volume

        PlayerPrefs.SetFloat("sfxVolume", volume);
        PlayerPrefs.Save();
    }
}
