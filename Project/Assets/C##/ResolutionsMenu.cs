using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ResolutionsMenu : MonoBehaviour
{
    public Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;  // Added for fullscreen toggle

    private Resolution[] resolutions;

    void Start()
    {
        // Get all available resolutions
        resolutions = Screen.resolutions;

        // Clear any existing options in the dropdown
        resolutionDropdown.ClearOptions();

        // Create a list to hold the resolution options as strings
        List<string> options = new List<string>();

        // Populate the dropdown with available resolutions
        foreach (Resolution resolution in resolutions)
        {
            string option = resolution.width + " x " + resolution.height + " @ " + resolution.refreshRate + "Hz";
            options.Add(option);
        }

        // Add the options to the dropdown
        resolutionDropdown.AddOptions(options);

        // Set the current resolution and fullscreen state
        int savedResolutionIndex = GetSavedResolutionIndex();
        resolutionDropdown.value = savedResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        bool isFullscreen = GetSavedFullscreenState();
        fullscreenToggle.isOn = isFullscreen;

        // Apply the saved resolution and fullscreen state
        ApplyResolution(savedResolutionIndex);
        Screen.fullScreen = isFullscreen;

        // Add listeners for dropdown and toggle changes
        resolutionDropdown.onValueChanged.AddListener(delegate { SetResolution(resolutionDropdown.value); });
        fullscreenToggle.onValueChanged.AddListener(delegate { SetFullscreen(fullscreenToggle.isOn); });
    }

    // Get the index of the saved resolution
    private int GetSavedResolutionIndex()
    {
        // Default to 0 if no saved index is found
        return PlayerPrefs.GetInt("ResolutionIndex", 0);
    }

    // Get the saved fullscreen state
    private bool GetSavedFullscreenState()
    {
        // Default to true if no saved state is found
        return PlayerPrefs.GetInt("Fullscreen", 1) == 1;
    }

    // Method to set the resolution based on the selected dropdown option
    public void SetResolution(int resolutionIndex)
    {
        if (resolutionIndex >= 0 && resolutionIndex < resolutions.Length)
        {
            Resolution selectedResolution = resolutions[resolutionIndex];
            Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreen);
            // Save the selected resolution index
            PlayerPrefs.SetInt("ResolutionIndex", resolutionIndex);
            PlayerPrefs.Save();
        }
        else
        {
            Debug.LogError("Invalid resolution index: " + resolutionIndex);
        }
    }

    // Method to toggle fullscreen mode
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        // Save the fullscreen state
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }

    // Apply the resolution based on the saved index
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
