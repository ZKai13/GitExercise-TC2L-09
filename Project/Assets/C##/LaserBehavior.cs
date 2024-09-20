using UnityEngine; 
using System.Collections; 

public class LaserBehavior : MonoBehaviour  
{  
    public float damageAmount = 10f;  
    private Coroutine laserCoroutine;  // To manage laser activation timing  

    private void Start()  
    {  
        // Ensure this is not activating the laser; it should remain inactive until commanded.  
        gameObject.SetActive(false); 
         
    }

    private void OnTriggerEnter2D(Collider2D collision)  
    {  
        // Check if the colliding object is the player  
        if (collision.CompareTag("Player"))  
        {  
            // Apply damage to the player  
            collision.GetComponent<Health>().Takedamage(damageAmount);  
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