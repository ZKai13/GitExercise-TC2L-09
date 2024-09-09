using UnityEngine;  
using UnityEngine.SceneManagement;  
using System.Collections; // Ensure this is included  

public class Teleporter : MonoBehaviour  
{  
    public GameObject PLAYER;  
    
    private Animator anim;  
    private Rigidbody2D rb;  

    void Start()  
    {  
        anim = GetComponent<Animator>();  
        rb = GetComponent<Rigidbody2D>();  
    }  

    private void OnTriggerEnter2D(Collider2D other)  
    {  
        if (other.gameObject.CompareTag("Player"))
        {      
            Death();  
        }  
    }  

    private void Death()  
    {  
        rb.bodyType = RigidbodyType2D.Static;  
        Destroy(PLAYER, 1f);  
        StartCoroutine(Restart()); // Start the coroutine  
    }  

    private IEnumerator Restart() // Change to IEnumerator  
    {  
        yield return new WaitForSeconds(1f); // Wait for 1 second  
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  
    }  
}