using UnityEngine; 
using System.Collections; 

public class LaserBehavior : MonoBehaviour  
{  
    public float damageAmount   = 10f;  
    private Coroutine laserCoroutine;  // To manage laser activation timing  

    private void Start()  
    {  
        // Ensure this is not activating the laser; it should remain inactive until commanded.  
        gameObject.SetActive(false); 
         
    }

    private void OnTriggerEnter2D(Collider2D collision)  
    {  
        Debug.Log("Laser collided with: " + collision.gameObject.name); // Log the collision  
        if (collision.CompareTag("Player"))  
        {  
            Debug.Log("Laser hit the player!"); // Confirm laser hit  

            Health playerHealth = collision.GetComponent<Health>(); // Get the Health component  
            if (playerHealth != null)  
            {  
                Debug.Log("Applying damage to the player: " + damageAmount);  
                playerHealth.Takedamage((int)damageAmount); // Apply damage to the player as int  
            }  
            else  
            {  
                Debug.LogError("Health component not found on player!");  
            }  
        }  
    }

    public void ShowLaserBeam()  
    {  
        gameObject.SetActive(true);  // Activate the laser beam  
        // Optionally, start a coroutine to manage the beam's duration or activation  
        if (laserCoroutine != null)  
        {  
            StopCoroutine(laserCoroutine);  
        }  
        laserCoroutine = StartCoroutine(DeactivateLaserAfterTime(5f)); // Example duration  
    }  

    public void HideLaserBeam()  
    {  
        gameObject.SetActive(false);  // Deactivate the laser beam  
    }  

    private IEnumerator DeactivateLaserAfterTime(float time)  
    {  
        yield return new WaitForSeconds(time);  
        HideLaserBeam();  // Automatically hide the laser after a set time  
    }  
}