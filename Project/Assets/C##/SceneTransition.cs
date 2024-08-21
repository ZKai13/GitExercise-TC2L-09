using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public Image fadeImage; // add in the picture to fade (which is black)
    public float fadeDuration = 1f; // duration of crossfade

    private void Start() 
    {
        StartCoroutine(FadeIn()); // fade in fron the play button scene
    }

    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeOut(sceneName)); //fade out to the target scene
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f; //the fade in start immediately
        Color color = fadeImage.color; //use the color of the picture set 
        while (elapsedTime < fadeDuration) //this is a loop, check if the fade duration is > the elapsed time
        {
            elapsedTime += Time.deltaTime;
            color.a = 1f - Mathf.Clamp01(elapsedTime / fadeDuration); //when the color is fully solid, it turns back transparent
            fadeImage.color = color;
            yield return null;
        }
        fadeImage.color = new Color(color.r, color.g, color.b, 0f); //this is cod to change it back to transparent
    }

    private IEnumerator FadeOut(string sceneName)
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }
        SceneManager.LoadScene(sceneName); //same as gotoscene function (changescene)
    }
}