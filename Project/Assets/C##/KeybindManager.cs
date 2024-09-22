using UnityEngine;
using UnityEngine.UI;

public class KeybindManager : MonoBehaviour
{
    public KeyRebinding keyRebinding;
    public Button moveLeftButton;
    public Button moveRightButton;
    public Button jumpButton;
    public Button attackButton;
    public Button blockButton;
    //public Text feedbackText;

    void Start()
    {
        moveLeftButton.onClick.AddListener(() => StartRebinding("MoveLeft", moveLeftButton));
        moveRightButton.onClick.AddListener(() => StartRebinding("MoveRight", moveRightButton));
        jumpButton.onClick.AddListener(() => StartRebinding("Jump", jumpButton));
        blockButton.onClick.AddListener(() => StartRebinding("Block", blockButton));
        attackButton.onClick.AddListener(() => StartRebinding("Attack", attackButton));
    }

    private void StartRebinding(string action, Button button)
    {
        keyRebinding.StartRebinding(action);
        //feedbackText.text = $"Press a key for {action}"; // Optional: show feedback text
        UpdateButtonText(button, action);
    }

    private void UpdateButtonText(Button button, string action)
    {
        button.GetComponentInChildren<Text>().text = $"Rebinding {action}";
    }
}