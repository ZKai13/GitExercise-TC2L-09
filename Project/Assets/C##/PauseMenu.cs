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
    [SerializeField] CanvasGroup settingsCanvasGroup;

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        PausePanelIntro();
    }

    public async void Resume()
    {
        await PausePanelOutro();
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void Menu()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    void PausePanelIntro()
    {
        settingsCanvasGroup.DOFade(1, tweenDuration).SetUpdate(true);
        pausePanelRect.DOAnchorPosX(middlePosX, tweenDuration).SetUpdate(true);
    }

    async Task PausePanelOutro()
    {
        settingsCanvasGroup.DOFade(0, tweenDuration).SetUpdate(true);
        await pausePanelRect.DOAnchorPosX(leftPosX, tweenDuration).SetUpdate(true).AsyncWaitForCompletion();
    }


}
