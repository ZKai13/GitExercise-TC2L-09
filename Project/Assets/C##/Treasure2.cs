using System.Collections;
using UnityEngine;  

public class Treasure2 : MonoBehaviour  
{  
    public AudioClip openSound;  
    private bool isOpen = false;  
    private Animator animator;  
    private bool playerInRange = false;   
    private AudioSource audioSource;   
    private UIManager1 uiManager;

    private void Start()  
    {  
        animator = GetComponent<Animator>();  
        audioSource = GetComponent<AudioSource>();  
        uiManager = FindObjectOfType<UIManager1>();

        if (PlayerPrefs.GetInt("Chest2Opened", 0) == 1)
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
        
        PlayerPrefs.SetInt("Chest2Opened", 1);  
        
    }  

    private IEnumerator ShowTreasureUIWithDelay()  
    {   
        yield return new WaitForSecondsRealtime(1f);   

        
        uiManager.ShowTreasureUI(); 
        Time.timeScale = 0;    

    }  
}