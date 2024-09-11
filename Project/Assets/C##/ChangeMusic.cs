using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMusic : MonoBehaviour
{  
    public AudioClip newMusic;
    private AudioSource audioSource; 
    public float lowVolume = 0.2f; 

    void Start()  
    {  
        audioSource = GetComponent<AudioSource>();  
    }  

    private void OnTriggerEnter2D(Collider2D other)
    {  
        if (other.gameObject.CompareTag("Player"))
        {  
            audioSource.clip = newMusic;
            audioSource.volume = lowVolume;  
            audioSource.Play();  
        }  
    }  
}
 