using UnityEngine;
using UnityEngine.UI;

public class KeyRebinding : MonoBehaviour
{
    public Text moveLeftText;
    public Text moveRightText;
    public Text jumpText;

    private KeyCode moveLeftKey;
    private KeyCode moveRightKey;
    private KeyCode jumpKey;

    private string currentKey;

    public static KeyRebinding Instance { get; private set; }



    void Start()
    {
        // Load saved key bindings or use default ones
        moveLeftKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("MoveLeftKey", KeyCode.A.ToString()));
        moveRightKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("MoveRightKey", KeyCode.D.ToString()));
        jumpKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("JumpKey", KeyCode.Space.ToString()));

        // Update UI text
        moveLeftText.text = moveLeftKey.ToString();
        moveRightText.text = moveRightKey.ToString();
        jumpText.text = jumpKey.ToString();
    }

    void Update()
    {
        // Test movement using the saved keys
        if (Input.GetKeyDown(moveLeftKey))
        {
            Debug.Log("Move Left");
        }

        if (Input.GetKeyDown(moveRightKey))
        {
            Debug.Log("Move Right");
        }

        if (Input.GetKeyDown(jumpKey))
        {
            Debug.Log("Jump");
        }
    }

    // Called when a key rebind button is clicked in the UI
    public void StartRebinding(string key)
    {
        currentKey = key;
    }

    void OnGUI()
    {
        // Check for the key press during the rebinding process
        Event e = Event.current;
        if (e.isKey && currentKey != null)
        {
            // Assign the new key based on what was pressed
            if (currentKey == "MoveLeft")
            {
                moveLeftKey = e.keyCode;
                moveLeftText.text = moveLeftKey.ToString();
                PlayerPrefs.SetString("MoveLeftKey", moveLeftKey.ToString());
            }
            else if (currentKey == "MoveRight")
            {
                moveRightKey = e.keyCode;
                moveRightText.text = moveRightKey.ToString();
                PlayerPrefs.SetString("MoveRightKey", moveRightKey.ToString());
            }
            else if (currentKey == "Jump")
            {
                jumpKey = e.keyCode;
                jumpText.text = jumpKey.ToString();
                PlayerPrefs.SetString("JumpKey", jumpKey.ToString());
            }

            // Clear current key after assigning
            currentKey = null;
        }
    }

    // Method to get the key for a specific action
    public KeyCode GetKeyForAction(string action)
    {
        if (action == "MoveLeft")
        {
            return moveLeftKey;
        }
        else if (action == "MoveRight")
        {
            return moveRightKey;
        }
        else if (action == "Jump")
        {
            return jumpKey;
        }
        else
        {
            Debug.LogWarning("Unknown action: " + action);
            return KeyCode.None;
        }
    }
}
