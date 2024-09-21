using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Health : MonoBehaviour  
{  
    [SerializeField] private float startingHealth = 100f;  
    public float currentHealth { get; private set; }  
    private Animator anim;  
    private GameManager gameManager;  
    public Image staminaImage;  
    private PotionScript potionScript;  
    public CanvasGroup PotionsCanvasGroup;

    void Start()  
    {  
        anim = GetComponent<Animator>();  
        gameManager = FindObjectOfType<GameManager>();
        potionScript = FindObjectOfType<PotionScript>();   

        // Check for null references
        if (anim == null)  
        {  
            Debug.LogError("Animator component not found!");  
        }  
        if (gameManager == null)  
        {  
            Debug.LogError("GameManager not found!");  
        }  
        if (potionScript == null)  
        {  
            Debug.LogError("PotionScript component not found!");  
        }  
        
    }  

    private void Awake()  
    {  
        currentHealth = startingHealth;  
    }  

    public void Takedamage(float _damage)  
{  
    if (_damage < 0) 
    {
        Debug.LogWarning("Damage cannot be negative.");
        return;
    }

    currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);   
//    Debug.Log($"Damage taken: {_damage}. Current health: {currentHealth}");

    if (currentHealth > 0)  
    {  
        // Player hurt  
    }  
    else   
    {  
        if (anim != null)  
        {  
            anim.SetTrigger("Die");  
        }
        else
        {
            Debug.LogError("Animator is null when trying to trigger Die.");
        }

        staminaImage.fillAmount = 0;

        ResetPotionCount();
        HidePotionUI(); 

        // Check for null before starting coroutine
        if (gameManager != null && staminaImage != null)
        {
            StartCoroutine(HandleDeath());
        }
        else
        {
            Debug.LogError("GameManager or StaminaImage is null when starting HandleDeath.");
        }
    }  
}  

    private IEnumerator HandleDeath()   
    {  
        yield return new WaitForSeconds(1f);   

        if (gameManager != null)  
        {
            gameManager.ShowDeathUI();  
        }  
        else  
        {  
            Debug.LogError("GameManager is null when trying to show death UI.");  
        }

        if (staminaImage != null)
        {
            staminaImage.fillAmount = 0; 
        }
        else
        {
            Debug.LogError("StaminaImage is null when trying to reset fill amount.");
        }

        this.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.simulated = false;   
        }
        else
        {
            Debug.LogError("Rigidbody2D is null when trying to disable physics.");
        }
    }   

    public void Heal(float _healAmount)  
    {  
        currentHealth = Mathf.Clamp(currentHealth + _healAmount, 0, startingHealth);  
    }  

    public void ResetPotionCount()
    {
        if (potionScript != null)
        {
            potionScript.ResetPotionCount();
            Debug.Log("Player potion count has been reset.");
        }
        else
        {
            Debug.LogError("PotionScript component is not assigned!");
        }
    }

    public void HidePotionUI()  
    {  
        PotionsCanvasGroup.alpha = 0; 
    } 
}
