using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Threading.Tasks;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] RectTransform pausePanelRect;
    [SerializeField] float leftPosX, middlePosX;
    [SerializeField] float tweenDuration;
    [SerializeField] CanvasGroup canvasGroup;

    private AudioManager audioManager; // Declare audioManager as a private field within the class

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        audioManager.PlaySFX(audioManager.buttonClick);
        Time.timeScale = 0;
        PausePanelIntro();
    }

    public async void Resume()
    {   
        audioManager.PlaySFX(audioManager.buttonClick);
        await PausePanelOutro();
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void Restart()
    {
        audioManager.PlaySFX(audioManager.buttonClick);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void Menu()
    {
        audioManager.PlaySFX(audioManager.buttonClick);
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    void PausePanelIntro()
    {
        canvasGroup.DOFade(1, tweenDuration).SetUpdate(true);
        pausePanelRect.DOAnchorPosX(middlePosX, tweenDuration).SetUpdate(true);
    }

    async Task PausePanelOutro()
    {
        canvasGroup.DOFade(0, tweenDuration).SetUpdate(true);
        await pausePanelRect.DOAnchorPosX(leftPosX, tweenDuration).SetUpdate(true).AsyncWaitForCompletion();
    }
}
