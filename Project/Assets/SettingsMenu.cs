using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Threading.Tasks;



public class SettingsMenu : MonoBehaviour
{

    [SerializeField] GameObject settingsMenu;
    [SerializeField] RectTransform settingsPanelRect;
    [SerializeField] float leftPosX, middlePosX;
    [SerializeField] float tweenDuration;
    [SerializeField] CanvasGroup canvasGroup;

    public void Settings()
    {
        settingsMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void Volume()
    {
        
    }

    public void Restart()
    {

    }

    public void Back()
    {
        settingsMenu.SetActive(false);
        Time.timeScale = 1;
        
    }



}
