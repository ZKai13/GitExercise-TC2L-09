using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine.UI;



public class LevelMenu : MonoBehaviour
{


    //Level
    [SerializeField] GameObject levelMenu;
    [SerializeField] RectTransform levelPanelRect;
    [SerializeField] float leftPosX, middlePosX;
    [SerializeField] float levelTweenDuration;
    [SerializeField] CanvasGroup levelCanvasGroup;

    public Image fadeImage; // add in the picture to fade (which is black)
    public float fadeDuration = 1f;
    public GameObject canvasGroupToMove;
    public int newLayerIndex;

    private AudioManager audioManager; // Declare audioManager as a private field within the class

    public Button[] buttons;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }
        for (int i = 0; i < unlockedLevel; i++)
        {
            buttons[i].interactable = true;
        }
    }

    public void Level()
    {
        levelMenu.SetActive(true);
        audioManager.PlaySFX(audioManager.buttonClick);
        Time.timeScale = 0;
        ChangeLayerOrder();
        LevelPanelIntro();
    }

    public void OpenLevel(int levelId)
    {
        audioManager.PlaySFX(audioManager.buttonClick);
        string levelName = "Level " + levelId;
        StartCoroutine(FadeOut(levelName));
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

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f; 
        Color color = fadeImage.color; 
        while (elapsedTime < fadeDuration) //this is a loop, check if the fade duration is > the elapsed time
        {
            elapsedTime += Time.deltaTime;
            color.a = 1f - Mathf.Clamp01(elapsedTime / fadeDuration); //when the color is fully solid, it turns back transparent
            fadeImage.color = color;
            yield return null;
        }
        fadeImage.color = new Color(color.r, color.g, color.b, 0f); 
    }

    private IEnumerator FadeOut(string levelName)
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }
        SceneManager.LoadScene(levelName); //same as gotoscene function (changescene)
    }
}
