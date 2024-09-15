using System.Collections;
using UnityEngine;  

public class Treasure : MonoBehaviour  
{  
    public AudioClip openSound;  
    private bool isOpen = false;  
    private Animator animator;  
    private bool playerInRange = false;   
    private AudioSource audioSource;   
    private UIManager uiManager;

    private void Start()  
    {  
        animator = GetComponent<Animator>();  
        audioSource = GetComponent<AudioSource>();  
        uiManager = FindObjectOfType<UIManager>();
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

        if (!PlayerPrefs.HasKey("FirstTreasure"))
        {
            PlayerPrefs.SetInt("FirstTreasure", 1); // Achievement unlocked
            PlayerPrefs.Save(); // Ensure changes are saved
            Debug.Log("Achievement Unlocked: First Treasure");
        }
    }  

    private IEnumerator ShowTreasureUIWithDelay()  
    {   
        yield return new WaitForSecondsRealtime(1f);   

        
        uiManager.ShowTreasureUI(); 
        Time.timeScale = 0;    

    }  
}