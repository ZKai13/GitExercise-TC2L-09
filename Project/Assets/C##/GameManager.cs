using UnityEngine;  
using UnityEngine.SceneManagement;  
using System.Collections;  

public class GameManager : MonoBehaviour  
{  
    public GameObject deathUI;  
    public GameObject afterDeathUI;
    private CanvasGroup deathCanvasGroup;  
    private CanvasGroup afterDeathCanvasGroup;
    private AudioSource audioSource;   

    void Start()  
    {  
        deathCanvasGroup = deathUI.GetComponent<CanvasGroup>();  
        afterDeathCanvasGroup = afterDeathUI.GetComponent<CanvasGroup>();
        deathCanvasGroup.alpha = 0;  
        deathUI.SetActive(false);   
        afterDeathUI.SetActive(false);
        audioSource = GetComponent<AudioSource>();  
    }  

    public void ShowDeathUI()  
    {    
        deathUI.SetActive(true);   
        audioSource.Play();  
        StartCoroutine(FadeIn(deathCanvasGroup, afterDeathCanvasGroup));  
    }  

    private IEnumerator FadeIn(CanvasGroup currentCanvasGroup, CanvasGroup nextCanvasGroup)  
    {  
        float duration = 1f;  
        float startAlpha = 0f;  
        float endAlpha = 1f;  
        float elapsed = 0f;  

        while (elapsed < duration)  
        {  
            elapsed += Time.deltaTime;  
            currentCanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);  
            yield return null;  
        }  

        currentCanvasGroup.alpha = endAlpha;    

    
        yield return new WaitForSeconds(2f);
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
            elapsed += Time.deltaTime;  
            nextCanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);  
            yield return null;  
        }  

        nextCanvasGroup.alpha = endAlpha;   
    }  
}