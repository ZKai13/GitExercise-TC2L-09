using UnityEngine;  

public class Treasure : MonoBehaviour  
{  
    public AudioClip openSound;
    private bool isOpen = false;  
    private Animator animator;  
    private bool playerInRange = false; 
    private AudioSource audioSource; 

    private void Start()  
    {  
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
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
            playerInRange = false; // 玩家离开范围  
        }  
    }  

    private void OpenChest()  
    {  
        isOpen = true;  
        animator.SetTrigger("Open");
        audioSource.PlayOneShot(openSound);
    }  
}