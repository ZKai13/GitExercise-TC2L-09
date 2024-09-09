using Unity.Mathematics;
using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    [SerializeField]private float startingHealth;
    public float currentHealth {get; private set;}
    private Animator anim;
    public GameObject PLAYER; 
    private GameManager gameManager;

    void Start()
    {
        anim = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Awake()
    {
        currentHealth= startingHealth;
    }
    public void Takedamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth); 
        
        if (currentHealth>0)
        {
            //player hurt
        }
        else 
        {
            anim.SetTrigger("Die");
            StartCoroutine(HandleDeath());
        }
    }

    private IEnumerator HandleDeath() 
    {  
        yield return new WaitForSeconds(1f); 
        //gameManager.ShowDeathUI();
        gameManager.ShowDeathUI();
        
        
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

}