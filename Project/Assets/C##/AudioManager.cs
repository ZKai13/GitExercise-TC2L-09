using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{


    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;


    public AudioClip BGM;
    public AudioClip buttonClick;
    public AudioClip playButtonClick;
    public AudioClip unlockChest;
    public AudioClip coinCollect;


    private void Start()
    {
        musicSource.clip = BGM;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

}
