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

    // public GameObject toTheDungeonAchievementUI;
    // public GameObject firstKillAchievementUI;
    // public GameObject completeLevel1AchievementUI;
    // public GameObject collect10CoinsAchievementUI;

    // public enum Achievement
    // {
    //     ToTheDungeon,
    //     FirstKill,
    //     CompleteLevel1,
    //     Collect10Coins
    // }

    public GameObject canvasGroupToMove;
    public int newLayerIndex;

    private AudioManager audioManager; // Declare audioManager as a private field within the class

    // private void Start()
    // {
    //     UpdateAchievementsUI();
    // }
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

    }
    
    // public static void UnlockAchievement(Achievement achievement)
    // {
    //     if (!IsAchievementUnlocked(achievement))
    //     {
    //         PlayerPrefs.SetInt(achievement.ToString(), 1);
    //         PlayerPrefs.Save();
    //         // Optionally notify the player about the achievement
    //         Debug.Log("Achievement Unlocked: " + achievement.ToString());
    //     }
    // }

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

//Achievements Mechanics
    // public static bool IsAchievementUnlocked(Achievement achievement)
    // {
    //     return PlayerPrefs.GetInt(achievement.ToString(), 0) == 1;
    // }


    // public void CheckAchievements(bool level1Completed, int coinsCollected)
    // {
    //     if (level1Completed)
    //     {
    //         if (!IsAchievementUnlocked(Achievement.CompleteLevel1))
    //         {
    //             UnlockAchievement(Achievement.CompleteLevel1);
    //         }
    //     }

    //     if (coinsCollected >= 10)
    //     {
    //         if (!IsAchievementUnlocked(Achievement.Collect10Coins))
    //         {
    //             UnlockAchievement(Achievement.Collect10Coins);
    //         }
    //     }

    //     // Update the UI after checking achievements
    //     UpdateAchievementsUI();
    // }

    // private void UpdateAchievementsUI()
    // {
    //     if (IsAchievementUnlocked(Achievement.ToTheDungeon))
    //     {
    //         toTheDungeonAchievementUI.SetActive(true);
    //     }
    //     else
    //     {
    //         toTheDungeonAchievementUI.SetActive(false);
    //     }
    //     if (IsAchievementUnlocked(Achievement.FirstKill))
    //     {
    //         firstKillAchievementUI.SetActive(true);
    //     }
    //     else
    //     {
    //         firstKillAchievementUI.SetActive(false);
    //     }

    //     if (IsAchievementUnlocked(Achievement.CompleteLevel1))
    //     {
    //         completeLevel1AchievementUI.SetActive(true);
    //     }
    //     else
    //     {
    //         completeLevel1AchievementUI.SetActive(false);
    //     }

    //     if (IsAchievementUnlocked(Achievement.Collect10Coins))
    //     {
    //         collect10CoinsAchievementUI.SetActive(true);
    //     }
    //     else
    //     {
    //         collect10CoinsAchievementUI.SetActive(false);
    //     }
    // }


//Layer Problem
    void ChangeLayerOrder()
    {
        RectTransform rectTransform = canvasGroupToMove.GetComponent<RectTransform>();
        rectTransform.SetSiblingIndex(newLayerIndex);
    }

    
}
