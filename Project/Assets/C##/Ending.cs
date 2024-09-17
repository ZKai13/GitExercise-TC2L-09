using UnityEngine;  
using UnityEngine.SceneManagement;  
using System.Collections;  

public class Ending : MonoBehaviour  
{  
    public GameObject ending1UI;  
    public GameObject ending2UI;
    public GameObject ending3UI;  
    private CanvasGroup ending1CanvasGroup;  
    private CanvasGroup ending2CanvasGroup;
    private CanvasGroup ending3CanvasGroup;   

    void Start()  
    {  
        ending1CanvasGroup = ending1UI.GetComponent<CanvasGroup>();  
        ending2CanvasGroup = ending2UI.GetComponent<CanvasGroup>(); 
        ending3CanvasGroup = ending3UI.GetComponent<CanvasGroup>();  
        ending1CanvasGroup.alpha = 0;  
        ending2CanvasGroup.alpha = 0;  
        ending1UI.SetActive(false);   
        ending2UI.SetActive(false); 
        ending3UI.SetActive(false);   
    }  

    public void ShowEnding1UI()  
    {     
        ending1UI.SetActive(true);     
        StartCoroutine(FadeIn(ending1CanvasGroup, ending2CanvasGroup));  
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
        ShowEnding2UI(nextCanvasGroup);  
    }  

    private void ShowEnding2UI(CanvasGroup nextCanvasGroup)  
    {  
        nextCanvasGroup.alpha = 0;  
        ending2UI.SetActive(true);
        StartCoroutine(FadeInNext(nextCanvasGroup, ending3CanvasGroup));
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
        ShowEnding3UI(nextCanvasGroup);  
    }  

    private void ShowEnding3UI(CanvasGroup nextCanvasGroup)  
    {  
        nextCanvasGroup.alpha = 0;  
        ending3UI.SetActive(true);  
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
