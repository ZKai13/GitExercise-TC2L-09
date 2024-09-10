using UnityEngine;  
using UnityEngine.SceneManagement;  
using System.Collections;  

public class GameManager : MonoBehaviour  
{  
    public GameObject deathUI;  
    public GameObject newUI;
    public GameObject afterDeathUI;  
    private CanvasGroup deathCanvasGroup;  
    private CanvasGroup newCanvasGroup;
    private CanvasGroup afterDeathCanvasGroup;  
    private AudioSource audioSource;   

    void Start()  
    {  
        deathCanvasGroup = deathUI.GetComponent<CanvasGroup>();  
        newCanvasGroup = newUI.GetComponent<CanvasGroup>(); 
        afterDeathCanvasGroup = afterDeathUI.GetComponent<CanvasGroup>();  
        deathCanvasGroup.alpha = 0;  
        newCanvasGroup.alpha = 0;  
        deathUI.SetActive(false);   
        newUI.SetActive(false); 
        afterDeathUI.SetActive(false);  
        audioSource = GetComponent<AudioSource>();  
    }  

    public void ShowDeathUI()  
    {     
        deathUI.SetActive(true);   
        audioSource.Play();  
        StartCoroutine(FadeIn(deathCanvasGroup, newCanvasGroup));  
    }  

    private IEnumerator FadeIn(CanvasGroup currentCanvasGroup, CanvasGroup nextCanvasGroup)  
    {  
        float duration = 1f;  
        float startAlpha = 0f;  
        float endAlpha = 1f;  
        float elapsed = 0f;  

        while (elapsed < duration)  
        {  
            elapsed += Time.unscaledDeltaTime;  
            currentCanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);  
            yield return null;  
        }  

        currentCanvasGroup.alpha = endAlpha;
        Time.timeScale = 0;     

        yield return new WaitForSecondsRealtime(2f);  
        ShowNewUI(nextCanvasGroup);  
    }  

    private void ShowNewUI(CanvasGroup nextCanvasGroup)  
    {  
        nextCanvasGroup.alpha = 0;  
        newUI.SetActive(true);
        StartCoroutine(FadeInNext(nextCanvasGroup, afterDeathCanvasGroup));
    }  

    private IEnumerator FadeInNext(CanvasGroup currentCanvasGroup, CanvasGroup nextCanvasGroup)  
    {   
        float duration = 1f;  
        float startAlpha = 0f;  
        float endAlpha = 1f;  
        float elapsed = 0f;  

        while (elapsed < duration)  
        {  
            elapsed += Time.unscaledDeltaTime; 
            currentCanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);  
            yield return null;  
        }  

        currentCanvasGroup.alpha = endAlpha;   

        yield return new WaitForSecondsRealtime(2f); 
        ShowAfterDeathUI(nextCanvasGroup);  
    }  

    private void ShowAfterDeathUI(CanvasGroup nextCanvasGroup)  
    {  
        nextCanvasGroup.alpha = 0;  
        afterDeathUI.SetActive(true);  
        StartCoroutine(FadeInNext(nextCanvasGroup));  
    }  

    private IEnumerator FadeInNext(CanvasGroup nextCanvasGroup)  
    {   
        float duration = 1f;  
        float startAlpha = 0f;  
        float endAlpha = 1f;  
        float elapsed = 0f;  

        while (elapsed < duration)  
        {  
            elapsed += Time.unscaledDeltaTime; 
            nextCanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);  
            yield return null;  
        }  

        nextCanvasGroup.alpha = endAlpha;   

        Time.timeScale = 1;   
    }  
}