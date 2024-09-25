using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollect : MonoBehaviour
{
    private PopUp popUp;
    [SerializeField] private int melonCountToCollect = 3;
    private int Melon = 0;
    public GameObject box2;
    private AudioSource audioSource;

    [SerializeField] private AudioClip collectSound;

    private void Start()
    {
        popUp = FindObjectOfType<PopUp>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Melon"))
        {
            Destroy(collision.gameObject);
            Melon++;
            Debug.Log("Collected Coins: " + Melon);

            if (audioSource && collectSound)  
            {  
                audioSource.PlayOneShot(collectSound); 
            }

            if (Melon >= melonCountToCollect)  
            {   
                HandleThreeMelonsCollected();  
            }      
        }
    }

    private void HandleThreeMelonsCollected()  
    {  
       if (!PlayerPrefs.HasKey("TheMissingCoin"))
        {
            PlayerPrefs.SetInt("TheMissingCoin", 3); 
            PlayerPrefs.Save(); 
            Debug.Log("Achievement Unlocked: The Missing Coin");
            popUp.DisplayAchievement(popUp.missingCoinSprite);
        }

       Debug.Log("Collected three Coins!");  
       Destroy(box2);
    }  
}