using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMusic : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    public AudioClip BGM;

    private void Start()
    {
        musicSource.clip = BGM;
        musicSource.Play();
    }

}
