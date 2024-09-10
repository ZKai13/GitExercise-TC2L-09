using UnityEngine;
using UnityEngine.UI;

public class ChangeMultipleButtonSprites : MonoBehaviour
{
    // Arrays to hold the buttons and their respective disabled sprites
    public Button[] buttons;
    public Sprite[] disabledSprites;

    private Image[] buttonImages;

    void Start()
    {
        // Initialize button images
        buttonImages = new Image[buttons.Length];
        for (int i = 0; i < buttons.Length; i++)
        {
            buttonImages[i] = buttons[i].GetComponent<Image>();
        }
    }

    void Update()
    {
        // Check each button's interactable status and change its sprite if disabled
        for (int i = 0; i < buttons.Length; i++)
        {
            if (!buttons[i].interactable)
            {
                buttonImages[i].sprite = disabledSprites[i];
            }
        }
    }
}