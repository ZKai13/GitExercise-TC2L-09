using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Sliders : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer; 
    [SerializeField] private Slider musicSlider; 

    private void Start()
    {
        
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        }
        else
        {
            
            musicSlider.value = 0.5f; 
        }

        SetMusicVolume();

        
        musicSlider.onValueChanged.AddListener(delegate { SetMusicVolume(); });
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        myMixer.SetFloat("music", Mathf.Log10(volume) * 20);

        PlayerPrefs.SetFloat("musicVolume", volume);
        PlayerPrefs.Save(); 
    }
}