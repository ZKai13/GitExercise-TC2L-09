using Unity.Mathematics;  
using UnityEngine;  
using UnityEngine.UI;
using System.Collections;  

public class Health : MonoBehaviour  
{  
    [SerializeField] private float startingHealth;  
    public float currentHealth { get; private set; }  
    private Animator anim;  
    public GameObject PLAYER;   
    private GameManager gameManager;  
<<<<<<< Updated upstream
    public Image staminaImage;
    private PotionScript potionScript;
    public CanvasGroup PotionsCanvasGroup;
=======
    public Image HealthImage; 
>>>>>>> Stashed changes

    void Start()  
    {  
        anim = GetComponent<Animator>();  
        gameManager = FindObjectOfType<GameManager>();
        potionScript = FindObjectOfType<PotionScript>();   
    }  

    private void Awake()  
    {  
        currentHealth = startingHealth;  
    }  

    public void Takedamage(float _damage)  
    {  
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);   
        
        if (currentHealth > 0)  
        {  
            // player hurt  
        }  
        else   
        {  
            anim.SetTrigger("Die");
<<<<<<< Updated upstream
            staminaImage.fillAmount = 0;
            ResetPotionCount();
            HidePotionUI(); 
=======
            HealthImage.fillAmount = 0; 
>>>>>>> Stashed changes
            StartCoroutine(HandleDeath());  
        }  
    }  

    private IEnumerator HandleDeath()   
    {  
        yield return new WaitForSeconds(1f);   
        gameManager.ShowDeathUI();  
        
        HealthImage.fillAmount = 0; 
        this.enabled = false;
        GetComponent<Rigidbody2D>().simulated = false;   
    }   

    public void Heal(float _healAmount)  
    {  
        currentHealth = Mathf.Clamp(currentHealth + _healAmount, 0, startingHealth);  
    }  

    private void Update()  
    {  
   
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