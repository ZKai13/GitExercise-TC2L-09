using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PotionScript : MonoBehaviour
{
    public int potionCount = 3; 
    public TextMeshProUGUI potionCountText;
    public Health playerHealth;

    void Start()
    {
        potionCount = PlayerPrefs.GetInt("PotionCount", 3);
        UpdatePotionCountUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && potionCount > 0)
        {
            UsePotion();
        }
    }

    public void AddPotion(int amount)
    {
        potionCount += amount;
        UpdatePotionCountUI();
        SavePotionCount();
    }

    void UsePotion()
    {
        if (potionCount > 0)
        {
            potionCount--; 
            UpdatePotionCountUI(); 
            playerHealth.Heal(100);
            SavePotionCount();
        }
    }

    void UpdatePotionCountUI()
    {
        potionCountText.text = "x " + potionCount.ToString();
    }

    void SavePotionCount()
    {
        PlayerPrefs.SetInt("PotionCount", potionCount);
        PlayerPrefs.Save();
    }

    public void ResetPotionCount()
    {
        potionCount = 3;
        UpdatePotionCountUI();
        PlayerPrefs.DeleteKey("PotionCount");
    }
}
