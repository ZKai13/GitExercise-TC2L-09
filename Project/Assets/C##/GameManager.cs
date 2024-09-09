using UnityEngine;  
using UnityEngine.SceneManagement;  
using System.Collections;  

public class GameManager : MonoBehaviour  
{  
    public GameObject deathUI; // 拖拽死亡UI的Panel到这个变量  
    private CanvasGroup canvasGroup;  
    private AudioSource audioSource; // 声明AudioSource变量  

    void Start()  
    {  
        canvasGroup = deathUI.GetComponent<CanvasGroup>();  
        canvasGroup.alpha = 0; // 游戏开始时隐藏死亡UI  
        deathUI.SetActive(false); // 确保UI对象不激活  
        audioSource = GetComponent<AudioSource>(); // 获取AudioSource组件  
    }  

    public void ShowDeathUI()  
    {  
         // 暂停游戏  
        deathUI.SetActive(true); // 激活UI对象  
        Debug.Log("Death UI is now active."); // 添加调试信息  
        audioSource.Play(); // 播放音乐  
        StartCoroutine(FadeIn()); // 开始淡入效果  
    }  

    private IEnumerator FadeIn()  
    {  
        Debug.Log("Starting FadeIn Coroutine."); // 添加调试信息  
        float duration = 1f; // 淡入持续时间  
        float startAlpha = 0f;  
        float endAlpha = 1f;  
        float elapsed = 0f;  

        while (elapsed < duration)  
        {  
            elapsed += Time.deltaTime;  
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);  
            yield return null; // 等待下一帧  
        }  

        canvasGroup.alpha = endAlpha;
        Time.timeScale = 0; // 确保最终透明度为1  
        Debug.Log("FadeIn Coroutine completed."); // 添加调试信息  
    }  

    public void RestartGame()  
    {  
        Time.timeScale = 1; // 恢复游戏  
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // 重新加载当前场景  
    }  
}