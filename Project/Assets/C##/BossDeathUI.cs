using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeathUI : MonoBehaviour
{
    public CanvasGroup BossCanvasGroup;
    public float fadeDuration = 1f;  // 控制淡入淡出的时间
    public AudioSource audioSource;  // 音频源
    public AudioClip fadeInSound;    // 淡入音效
    public AudioClip fadeOutSound;   // 淡出音效

    // ShowBossUI with FadeIn effect and sound
    public void ShowBossUI()
    {
        StartCoroutine(FadeIn());
    }

    // HideBossUI with FadeOut effect and sound
    public void HideBossUI()
    {
        StartCoroutine(FadeOut());
    }

    // Coroutine for FadeIn
    private IEnumerator FadeIn()
    {
        // 播放淡入音效
        if (audioSource != null && fadeInSound != null)
        {
            audioSource.clip = fadeInSound;
            audioSource.Play();
        }

        float startAlpha = BossCanvasGroup.alpha;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            BossCanvasGroup.alpha = Mathf.Lerp(startAlpha, 1, elapsedTime / fadeDuration);
            yield return null;
        }

        BossCanvasGroup.alpha = 1;
        BossCanvasGroup.interactable = true;
        BossCanvasGroup.blocksRaycasts = true;
    }

    // Coroutine for FadeOut
    private IEnumerator FadeOut()
    {
        // 播放淡出音效
        if (audioSource != null && fadeOutSound != null)
        {
            audioSource.clip = fadeOutSound;
            audioSource.Play();
        }

        float startAlpha = BossCanvasGroup.alpha;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            BossCanvasGroup.alpha = Mathf.Lerp(startAlpha, 0, elapsedTime / fadeDuration);
            yield return null;
        }

        BossCanvasGroup.alpha = 0;
        BossCanvasGroup.interactable = false;
        BossCanvasGroup.blocksRaycasts = false;
    }
}
