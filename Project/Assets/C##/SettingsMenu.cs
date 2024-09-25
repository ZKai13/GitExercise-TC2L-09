using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Threading.Tasks;



public class SettingsMenu : MonoBehaviour
{


    //Settings
    [SerializeField] GameObject settingsMenu;
    [SerializeField] RectTransform settingsPanelRect;
    [SerializeField] float leftPosX, middlePosX;
    [SerializeField] float settingsTweenDuration;
    [SerializeField] CanvasGroup settingsCanvasGroup;

    //Sound
    [SerializeField] GameObject soundMenu;
    [SerializeField] RectTransform soundPanelRect;
    [SerializeField] float soundLeftPosX, soundMiddlePosX;
    [SerializeField] float soundTweenDuration;
    [SerializeField] CanvasGroup soundCanvasGroup;

    //Graphics
    [SerializeField] GameObject graphicsMenu;
    [SerializeField] RectTransform graphicsPanelRect;
    [SerializeField] float graphicsLeftPosX, graphicsMiddlePosX;
    [SerializeField] float graphicsTweenDuration;
    [SerializeField] CanvasGroup graphicsCanvasGroup;

    //Controls
    [SerializeField] GameObject controlsMenu;
    [SerializeField] RectTransform controlsPanelRect;
    [SerializeField] float controlssLeftPosX, controlsMiddlePosX;
    [SerializeField] float controlsTweenDuration;
    [SerializeField] CanvasGroup controlsCanvasGroup;


    public GameObject canvasGroupToMove;
    public int newLayerIndex;

    private AudioManager audioManager; // Declare audioManager as a private field within the class

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void Settings()
    {
        settingsMenu.SetActive(true);
        audioManager.PlaySFX(audioManager.buttonClick);
        Time.timeScale = 1;
        ChangeLayerOrder();
        SettingsPanelIntro();
    }


    public async void Back()
    {
        audioManager.PlaySFX(audioManager.buttonClick);
        await SettingsPanelOutro();
        settingsMenu.SetActive(false);
        Time.timeScale = 1; 
        
    }

    void SettingsPanelIntro()
    {
        settingsCanvasGroup.DOFade(1, settingsTweenDuration).SetUpdate(true);
        settingsPanelRect.DOAnchorPosX(middlePosX, settingsTweenDuration).SetUpdate(true);
    }

    async Task SettingsPanelOutro()
    {
        settingsCanvasGroup.DOFade(0, settingsTweenDuration).SetUpdate(true);
        await settingsPanelRect.DOAnchorPosX(leftPosX, settingsTweenDuration).SetUpdate(true).AsyncWaitForCompletion();
    }



    public async void Sound()
    {
        audioManager.PlaySFX(audioManager.buttonClick);
        await SettingsPanelOutro();
        settingsMenu.SetActive(false);
        SoundPanelIntro();
        soundMenu.SetActive(true);
        Time.timeScale = 0;
        
    }


    public async void SoundBack()
    {
        audioManager.PlaySFX(audioManager.buttonClick);
        await SoundPanelOutro();
        soundMenu.SetActive(false);
        Time.timeScale = 1; 
        
    }


    void SoundPanelIntro()
    {
        soundCanvasGroup.DOFade(1, soundTweenDuration).SetUpdate(true);
        soundPanelRect.DOAnchorPosX(middlePosX, soundTweenDuration).SetUpdate(true);
    }


    async Task SoundPanelOutro()
    {
        soundCanvasGroup.DOFade(0, settingsTweenDuration).SetUpdate(true);
        await soundPanelRect.DOAnchorPosX(leftPosX, settingsTweenDuration).SetUpdate(true).AsyncWaitForCompletion();
    }

    void ChangeLayerOrder()
    {
        RectTransform rectTransform = canvasGroupToMove.GetComponent<RectTransform>();
        rectTransform.SetSiblingIndex(newLayerIndex);
    }

    public async void Graphics()
    {
        audioManager.PlaySFX(audioManager.buttonClick);
        await SettingsPanelOutro();
        settingsMenu.SetActive(false);
        GraphicsPanelIntro();
        graphicsMenu.SetActive(true);
        Time.timeScale = 0;
        
    }


    public async void GraphicsBack()
    {
        audioManager.PlaySFX(audioManager.buttonClick);
        await GraphicsPanelOutro();
        graphicsMenu.SetActive(false);
        Time.timeScale = 1; 
        
    }


    void GraphicsPanelIntro()
    {
        graphicsCanvasGroup.DOFade(1, graphicsTweenDuration).SetUpdate(true);
        graphicsPanelRect.DOAnchorPosX(middlePosX, graphicsTweenDuration).SetUpdate(true);
    }


    async Task GraphicsPanelOutro()
    {
        graphicsCanvasGroup.DOFade(0, graphicsTweenDuration).SetUpdate(true);
        await graphicsPanelRect.DOAnchorPosX(leftPosX, graphicsTweenDuration).SetUpdate(true).AsyncWaitForCompletion();
    }

    public async void Controls()
    {
        audioManager.PlaySFX(audioManager.buttonClick);
        await SettingsPanelOutro();
        settingsMenu.SetActive(false);
        ControlsPanelIntro();
        controlsMenu.SetActive(true);
        Time.timeScale = 0;
        
    }

    public async void ControlsBack()
    {
        audioManager.PlaySFX(audioManager.buttonClick);
        await ControlsPanelOutro();
        controlsMenu.SetActive(false);
        Time.timeScale = 1; 
    }

    void ControlsPanelIntro()
    {
        controlsCanvasGroup.DOFade(1, controlsTweenDuration).SetUpdate(true);
        controlsPanelRect.DOAnchorPosX(middlePosX, controlsTweenDuration).SetUpdate(true);
    }


    async Task ControlsPanelOutro()
    {
        controlsCanvasGroup.DOFade(0, controlsTweenDuration).SetUpdate(true);
        await controlsPanelRect.DOAnchorPosX(leftPosX, controlsTweenDuration).SetUpdate(true).AsyncWaitForCompletion();
    }

}
