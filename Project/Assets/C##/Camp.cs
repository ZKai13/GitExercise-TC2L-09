using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camp : MonoBehaviour
{  
    private PopUp popUp;
    private bool isNearInteractable = false;
    private bool hasInteracted = false;  
    public Health playerHealth;
    public PotionScript potionScript;
    public CanvasGroup campCanvasGroup;
    public GameObject itemToHide;

    public float fadeDuration = 1.0f;

    private void Start()
    {
        popUp = FindObjectOfType<PopUp>();
    }
    void Update()  
    {  
        if (isNearInteractable && Input.GetKeyDown(KeyCode.F) && !hasInteracted)  
        {  

            StartCoroutine(FadeInAndInteract());
            Interact();
            if (!PlayerPrefs.HasKey("AhhYes"))
            {
                PlayerPrefs.SetInt("AhhYes", PlayerPrefs.GetInt("AhhYes", 0) + 1); // Achievement unlocked
                PlayerPrefs.Save(); // Ensure changes are saved
                Debug.Log("Achievement Unlocked: Ahh..Yes, The campfire");
                popUp.DisplayAchievement(popUp.campfireSprite);
            }
              
        }  
    }

     private IEnumerator FadeInAndInteract()
     {
        yield return StartCoroutine(FadeCanvas(campCanvasGroup, 0, 1, fadeDuration)); 
        Interact();
        yield return new WaitForSeconds(2.0f); 

        if (itemToHide != null)
        {
            var spriteRenderer = itemToHide.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sortingLayerName = "New Layer 8"; // 修改为你需要的 Sorting Layer
                Debug.Log("Item sorting layer changed to New Layer 8");
            }
        }

        yield return StartCoroutine(FadeCanvas(campCanvasGroup, 1, 0, fadeDuration)); 
    }


     private void Interact()
    {  
        if (playerHealth != null)  
        {  
            playerHealth.Heal(100);
            potionScript.ResetPotionCount();
            hasInteracted = true;
        }  
    } 

    private IEnumerator FadeCanvas(CanvasGroup canvasGroup, float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = endAlpha; 
    } 

    private void OnTriggerEnter2D(Collider2D other)  
    {  
        if (other.CompareTag("Player")) 
        {  
            isNearInteractable = true;  
        }  
    }  

    private void OnTriggerExit2D(Collider2D other)  
    {  
        if (other.CompareTag("Player"))  
        {  
            isNearInteractable = false;  
        }  
    }  
}
