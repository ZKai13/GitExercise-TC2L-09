using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Threading.Tasks;



public class LevelMenu : MonoBehaviour
{


    //Level
    [SerializeField] GameObject levelMenu;
    [SerializeField] RectTransform levelPanelRect;
    [SerializeField] float leftPosX, middlePosX;
    [SerializeField] float levelTweenDuration;
    [SerializeField] CanvasGroup levelCanvasGroup;

    public GameObject canvasGroupToMove;
    public int newLayerIndex;

    private AudioManager audioManager; // Declare audioManager as a private field within the class

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void Level()
    {
        levelMenu.SetActive(true);
        audioManager.PlaySFX(audioManager.buttonClick);
        Time.timeScale = 0;
        ChangeLayerOrder();
        LevelPanelIntro();
    }

    public void OpenLevel(string levelName)
    {
        // Load the scene with the specified name
        audioManager.PlaySFX(audioManager.buttonClick);
        SceneManager.LoadScene(levelName);
        Time.timeScale = 1;
        
    }

    public async void Back()
    {
        audioManager.PlaySFX(audioManager.buttonClick);
        await LevelPanelOutro();
        levelMenu.SetActive(false);
        Time.timeScale = 1; 
        
    }

    void LevelPanelIntro()
    {
        levelCanvasGroup.DOFade(1, levelTweenDuration).SetUpdate(true);
        levelPanelRect.DOAnchorPosX(middlePosX, levelTweenDuration).SetUpdate(true);
    }

    async Task LevelPanelOutro()
    {
        levelCanvasGroup.DOFade(0, levelTweenDuration).SetUpdate(true);
        await levelPanelRect.DOAnchorPosX(leftPosX, levelTweenDuration).SetUpdate(true).AsyncWaitForCompletion();
    }



    void ChangeLayerOrder()
    {
        RectTransform rectTransform = canvasGroupToMove.GetComponent<RectTransform>();
        rectTransform.SetSiblingIndex(newLayerIndex);
    }
}
