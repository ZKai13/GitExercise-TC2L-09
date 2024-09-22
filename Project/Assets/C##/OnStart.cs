using UnityEngine;

public class OnStart : MonoBehaviour
{
    public PopUp popUp; // Reference to your PopUp script

    private void Start()
    {
        ShowInitialAchievement();
    }

    private void ShowInitialAchievement()
    {
        if (PlayerPrefs.GetInt("UnlockedLevel", 1) == 2 && popUp != null)
            {
                Debug.Log("Achievement Unlocked: Into The Dungeon");
                popUp.DisplayAchievement(popUp.intoTheDungeonSprite); // Correct instance call
            }
        else if (PlayerPrefs.GetInt("UnlockedLevel", 1) == 6 && popUp != null)
            {
                Debug.Log("Achievement Unlocked: HeroKnight");
                popUp.DisplayAchievement(popUp.heroKnightSprite); // Correct instance call
            }

    }
}