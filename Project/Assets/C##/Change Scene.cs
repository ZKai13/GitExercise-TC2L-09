using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


    
public class Finish : MonoBehaviour
{
    public AchievementsMenu achievementsMenu;
    private bool levelCompleted = false;


    // This method will be triggered when the player reaches the finish point
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "PLAYER" && !levelCompleted)
        {
            levelCompleted = true;
            UnlockNewLevel(); // Unlock the new level before transitioning
            Invoke("CompleteLevel", 0f); // Transition to the next level
        }
    }

    private void CompleteLevel()
    {
        // Move to the next scene (next level)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void UnlockNewLevel()
    {
        // Check if the current level is at or above the reached level in PlayerPrefs
        if (SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("ReachedIndex"))
        {
            // Save the new reached index (current scene index + 1)
            PlayerPrefs.SetInt("ReachedIndex", SceneManager.GetActiveScene().buildIndex + 1);
            
            // Unlock the next level by incrementing the unlocked level count
            PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel", 1) + 1);
            
            
            // Save the PlayerPrefs changes
            PlayerPrefs.Save();

            if (PlayerPrefs.GetInt("UnlockedLevel", 1) == 2)
            {
                Debug.Log("Achievement Unlocked: Into The Dungeon");
            }
        }
    }
}
