using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class PopUp : MonoBehaviour
{

    public Sprite intoTheDungeonSprite;
    public Sprite firstTreasureSprite;
    public Sprite missingCoinSprite;
    public Sprite goblinSprite;
    public Sprite heroKnightSprite;
    public Sprite campfireSprite;
    [SerializeField] private CanvasGroup achievementsCanvasGroup;
    [SerializeField] private RectTransform achievementsPanelRect;
    [SerializeField] private float achievementsTweenDuration = 0.5f;
    [SerializeField] private float autoHideDelay = 3f; // Time to show the achievement
    [SerializeField] private float middlePosX = 180f; // Adjust according to your layout
    [SerializeField] private float leftPosX = 270f; // Adjust to slide out of view
    [SerializeField] private Image achievementImage; // Reference to the Image component that displays the achievement sprite

    internal static void DisplayAchievement(object intoTheDungeonSprite)
    {
        throw new NotImplementedException();
    }

    // Call this method to display a specific achievement
    public void DisplayAchievement(Sprite achievementSprite)
    {
        achievementImage.sprite = achievementSprite; // Set the achievement sprite
        StartCoroutine(AchievementDisplayCoroutine());
    }



    private IEnumerator AchievementDisplayCoroutine()
    {
        // Slide in
        AchievementsPanelIntro();
        yield return new WaitForSeconds(autoHideDelay);
        
        // Slide out
        yield return AchievementsPanelOutro();
    }

    private void AchievementsPanelIntro()
    {
        achievementsCanvasGroup.alpha = 1; // Ensure it's visible
        achievementsCanvasGroup.DOFade(1, achievementsTweenDuration).SetUpdate(true);
        achievementsPanelRect.DOAnchorPosX(middlePosX, achievementsTweenDuration).SetUpdate(true);
    }

    private IEnumerator AchievementsPanelOutro()
    {
        achievementsCanvasGroup.DOFade(0, achievementsTweenDuration).SetUpdate(true);
        yield return achievementsPanelRect.DOAnchorPosX(leftPosX, achievementsTweenDuration).SetUpdate(true).AsyncWaitForCompletion();
    }

    
}
