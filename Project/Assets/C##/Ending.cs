using UnityEngine;  
using UnityEngine.SceneManagement;  
using System.Collections;  

public class Ending : MonoBehaviour  
{  
    public GameObject ending1UI;  
    public GameObject ending2UI;
    public GameObject ending3UI;  
    public GameObject ending4UI;
    public GameObject ending5UI;
    public GameObject ending6UI;
    public GameObject ending7UI;
    public GameObject ending75UI;
    public GameObject ending8UI;

    private CanvasGroup ending1CanvasGroup;  
    private CanvasGroup ending2CanvasGroup;
    private CanvasGroup ending3CanvasGroup;  
    private CanvasGroup ending4CanvasGroup;
    private CanvasGroup ending5CanvasGroup;
    private CanvasGroup ending6CanvasGroup;
    private CanvasGroup ending7CanvasGroup;
    private CanvasGroup ending75CanvasGroup;
    private CanvasGroup ending8CanvasGroup;

    void Start()  
    {  
        ending1CanvasGroup = ending1UI.GetComponent<CanvasGroup>();  
        ending2CanvasGroup = ending2UI.GetComponent<CanvasGroup>(); 
        ending3CanvasGroup = ending3UI.GetComponent<CanvasGroup>();  
        ending4CanvasGroup = ending4UI.GetComponent<CanvasGroup>();  
        ending5CanvasGroup = ending5UI.GetComponent<CanvasGroup>();
        ending6CanvasGroup = ending6UI.GetComponent<CanvasGroup>();
        ending7CanvasGroup = ending7UI.GetComponent<CanvasGroup>();
        ending75CanvasGroup = ending75UI.GetComponent<CanvasGroup>();
        ending8CanvasGroup = ending8UI.GetComponent<CanvasGroup>();

        ending1CanvasGroup.alpha = 0;  
        ending2CanvasGroup.alpha = 0;  
        ending3CanvasGroup.alpha = 0;  
        ending4CanvasGroup.alpha = 0;  
        ending5CanvasGroup.alpha = 0;  
        ending6CanvasGroup.alpha = 0;  
        ending7CanvasGroup.alpha = 0;  
        ending75CanvasGroup.alpha = 0;  
        ending8CanvasGroup.alpha = 0;  

        ending1UI.SetActive(false);   
        ending2UI.SetActive(false); 
        ending3UI.SetActive(false);  
        ending4UI.SetActive(false);  
        ending5UI.SetActive(false);  
        ending6UI.SetActive(false);  
        ending7UI.SetActive(false);
        ending75UI.SetActive(false);   
        ending8UI.SetActive(false);  
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
        StartCoroutine(FadeInAfterEnding3(nextCanvasGroup, ending4CanvasGroup)); 
    }  

    private IEnumerator FadeInAfterEnding3(CanvasGroup currentCanvasGroup, CanvasGroup nextCanvasGroup)  
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
        ShowEnding4UI(nextCanvasGroup);    
    }  

    private void ShowEnding4UI(CanvasGroup nextCanvasGroup)  
    {  
        nextCanvasGroup.alpha = 0;  
        ending4UI.SetActive(true);  
        StartCoroutine(FadeInNextEnding(nextCanvasGroup, ending5CanvasGroup));  
    }  

    private IEnumerator FadeInNextEnding(CanvasGroup currentCanvasGroup, CanvasGroup nextCanvasGroup)  
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
        ShowEnding5UI(nextCanvasGroup);  
    }  

    private void ShowEnding5UI(CanvasGroup nextCanvasGroup)  
    {  
        nextCanvasGroup.alpha = 0;  
        ending5UI.SetActive(true);  
        StartCoroutine(FadeInNextEnding1(nextCanvasGroup, ending6CanvasGroup));  
    } 

    private IEnumerator FadeInNextEnding1(CanvasGroup currentCanvasGroup, CanvasGroup nextCanvasGroup)  
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
        ShowEnding6UI(nextCanvasGroup);  
    }

    private void ShowEnding6UI(CanvasGroup nextCanvasGroup)  
    {  
        nextCanvasGroup.alpha = 0;  
        ending6UI.SetActive(true);  
        StartCoroutine(FadeInNextEnding2(nextCanvasGroup, ending7CanvasGroup));  
    }  

    private IEnumerator FadeInNextEnding2(CanvasGroup currentCanvasGroup, CanvasGroup nextCanvasGroup)  
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
        ShowEnding7UI(nextCanvasGroup);  
    }

    private void ShowEnding7UI(CanvasGroup nextCanvasGroup)  
    {  
        nextCanvasGroup.alpha = 0;  
        ending7UI.SetActive(true);  
        StartCoroutine(FadeInNextEnding3(nextCanvasGroup, ending75CanvasGroup));  
    }  

    private IEnumerator FadeInNextEnding3(CanvasGroup currentCanvasGroup, CanvasGroup nextCanvasGroup)  
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
        ShowEnding75UI(nextCanvasGroup);  
    }

    private void ShowEnding75UI(CanvasGroup nextCanvasGroup)  
    {  
        nextCanvasGroup.alpha = 0;  
        ending75UI.SetActive(true);  
        StartCoroutine(FadeInNextEnding4(nextCanvasGroup, ending8CanvasGroup));
    }

    private IEnumerator FadeInNextEnding4(CanvasGroup currentCanvasGroup, CanvasGroup nextCanvasGroup)  
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
        ShowEnding8UI(nextCanvasGroup);  
    } 

    private void ShowEnding8UI(CanvasGroup nextCanvasGroup)  
    {  
        nextCanvasGroup.alpha = 0;  
        ending8UI.SetActive(true);  
        StartCoroutine(FadeInLast(nextCanvasGroup));  
    }  

    

    private IEnumerator FadeInLast(CanvasGroup nextCanvasGroup)  
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

        yield return new WaitForSecondsRealtime(2f);

        SceneManager.LoadScene("MainMenu");  
    }  
}
