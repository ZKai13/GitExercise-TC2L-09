using System.Collections;
using UnityEngine;  

public class Treasure : MonoBehaviour  
{  
    private PopUp popUp;
    public AudioClip openSound;  
    private bool isOpen = false;  
    private Animator animator;  
    private bool playerInRange = false;   
    private AudioSource audioSource;   
    private UIManager uiManager;

    private void Start()  
    {  
        popUp = FindObjectOfType<PopUp>();
        animator = GetComponent<Animator>();  
        audioSource = GetComponent<AudioSource>();  
        uiManager = FindObjectOfType<UIManager>();

        if (PlayerPrefs.GetInt("ChestOpened", 0) == 1)
        {
            gameObject.SetActive(false);
        }
    }  

    private void Update()  
    {  
        if (playerInRange && Input.GetKeyDown(KeyCode.F) && !isOpen)  
        {  
            OpenChest();  
        }  
    }  

    private void OnTriggerEnter2D(Collider2D collision)  
    {  
        if (collision.CompareTag("Player"))  
        {  
            playerInRange = true;  
        }  
    }  

    private void OnTriggerExit2D(Collider2D collision)  
    {  
        if (collision.CompareTag("Player"))  
        {  
            playerInRange = false;   
        }  
    }  

    private void OpenChest()  
    {  
        isOpen = true;  
        animator.SetTrigger("Open");  
        audioSource.PlayOneShot(openSound);  
        StartCoroutine(ShowTreasureUIWithDelay());
        
        PlayerPrefs.SetInt("ChestOpened", 1);  

        if (!PlayerPrefs.HasKey("FirstTreasure"))
        {
            PlayerPrefs.SetInt("FirstTreasure", 1); // Achievement unlocked
            PlayerPrefs.Save(); // Ensure changes are saved
            Debug.Log("Achievement Unlocked: First Treasure");
            popUp.DisplayAchievement(popUp.intoTheDungeonSprite);
        }
        
    }  

    private IEnumerator ShowTreasureUIWithDelay()  
    {   
        yield return new WaitForSecondsRealtime(1f);   

        
        uiManager.ShowTreasureUI(); 
        Time.timeScale = 0;    

    }  
}