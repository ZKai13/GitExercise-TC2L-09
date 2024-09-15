using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ResolutionsMenu : MonoBehaviour
{
    public Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;  

    private Resolution[] resolutions;

    void Start()
    {

        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

    
        foreach (Resolution resolution in resolutions)
        {
            string option = resolution.width + " x " + resolution.height + " @ " + resolution.refreshRate + "Hz";
            options.Add(option);
        }

      
        resolutionDropdown.AddOptions(options);

       
        int savedResolutionIndex = GetSavedResolutionIndex();
        resolutionDropdown.value = savedResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        bool isFullscreen = GetSavedFullscreenState();
        fullscreenToggle.isOn = isFullscreen;

        
        ApplyResolution(savedResolutionIndex);
        Screen.fullScreen = isFullscreen;

       
        resolutionDropdown.onValueChanged.AddListener(delegate { SetResolution(resolutionDropdown.value); });
        fullscreenToggle.onValueChanged.AddListener(delegate { SetFullscreen(fullscreenToggle.isOn); });
    }

    
    private int GetSavedResolutionIndex()
    {
        
        return PlayerPrefs.GetInt("ResolutionIndex", 0);
    }

    
    private bool GetSavedFullscreenState()
    {
       
        return PlayerPrefs.GetInt("Fullscreen", 1) == 1;
    }

    public void SetResolution(int resolutionIndex)
    {
        if (resolutionIndex >= 0 && resolutionIndex < resolutions.Length)
        {
            Resolution selectedResolution = resolutions[resolutionIndex];
            Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreen);
           
            PlayerPrefs.SetInt("ResolutionIndex", resolutionIndex);
            PlayerPrefs.Save();
        }
        else
        {
            Debug.LogError("Invalid resolution index: " + resolutionIndex);
        }
    }

   
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void ApplyResolution(int resolutionIndex)
    {
        if (resolutionIndex >= 0 && resolutionIndex < resolutions.Length)
        {
            Resolution selectedResolution = resolutions[resolutionIndex];
            Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreen);
        }
        else
        {
            Debug.LogError("Invalid resolution index for apply: " + resolutionIndex);
        }
    }
}
