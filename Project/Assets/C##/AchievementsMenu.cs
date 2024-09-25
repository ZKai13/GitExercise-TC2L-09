using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine.UI;



public class AchievementsMenu : MonoBehaviour
{


    //Level
    [SerializeField] GameObject achievementsMenu;
    [SerializeField] RectTransform achievementsPanelRect;
    [SerializeField] float leftPosY, middlePosY;
    [SerializeField] float achievementsTweenDuration;
    [SerializeField] CanvasGroup achievementsCanvasGroup;

    public Button intoTheDungeonButton;
    public Button firstTreasureButton;
    public Button theMissingCoinButton;
    public Button theGoblinButton;
     public Button heroKnightButton;
    public Button ahYesButton;

    void Start()
    {
        intoTheDungeonButton.gameObject.SetActive(false);
        firstTreasureButton.gameObject.SetActive(false);
        theMissingCoinButton.gameObject.SetActive(false);
        theGoblinButton.gameObject.SetActive(false);
        heroKnightButton.gameObject.SetActive(false);
        ahYesButton.gameObject.SetActive(false);

        if (PlayerPrefs.GetInt("UnlockedLevel", 1) >= 2)
        {
            intoTheDungeonButton.gameObject.SetActive(true);
        }

        if (PlayerPrefs.GetInt("FirstTreasure", 0) == 1)
        {
            firstTreasureButton.gameObject.SetActive(true);
        }

        if (PlayerPrefs.GetInt("TheMissingCoin", 0) == 3)
        {
            theMissingCoinButton.gameObject.SetActive(true);
        }

        if (PlayerPrefs.GetInt("TheGoblin", 0) >= 1)
        {
            theGoblinButton.gameObject.SetActive(true);
        }

        if (PlayerPrefs.GetInt("AhhYes", 0) >= 1)
        {
            ahYesButton.gameObject.SetActive(true);
        }

        if (PlayerPrefs.GetInt("UnlockedLevel", 1) == 6)
        {
            heroKnightButton.gameObject.SetActive(true);
        }

    
    }



    public GameObject canvasGroupToMove;
    public int newLayerIndex;

    private AudioManager audioManager; // Declare audioManager as a private field within the class

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

    }
    

    public void Achievements()
    {
        achievementsMenu.SetActive(true);
        audioManager.PlaySFX(audioManager.buttonClick);
        Time.timeScale = 0;
        ChangeLayerOrder();
        AchievementsPanelIntro();
    }


    public async void Back()
    {
        audioManager.PlaySFX(audioManager.buttonClick);
        await AchievementsPanelOutro();
        achievementsMenu.SetActive(false);
        Time.timeScale = 1; 
        
    }

    void AchievementsPanelIntro()
    {
        achievementsCanvasGroup.DOFade(1, achievementsTweenDuration).SetUpdate(true);
        achievementsPanelRect.DOAnchorPosY(middlePosY, achievementsTweenDuration).SetUpdate(true);
    }

    async Task AchievementsPanelOutro()
    {
        achievementsCanvasGroup.DOFade(0, achievementsTweenDuration).SetUpdate(true);
        await achievementsPanelRect.DOAnchorPosY(leftPosY, achievementsTweenDuration).SetUpdate(true).AsyncWaitForCompletion();
    }


//Layer Problem
    void ChangeLayerOrder()
    {
        RectTransform rectTransform = canvasGroupToMove.GetComponent<RectTransform>();
        rectTransform.SetSiblingIndex(newLayerIndex);
    }

    
}
