// using System.Collections.Generic;  
// using UnityEngine;  

// public class MagicBallPool : MonoBehaviour  
// {  
//     // Public variable to assign magic ball prefab from the Inspector  
//     public GameObject magicBallPrefab;  

//     // List to keep track of available magic balls  
//     private List<GameObject> availableBalls = new List<GameObject>();  

//     // List to keep track of active magic balls  
//     private List<GameObject> activeBalls = new List<GameObject>();  

//     // Method to get an available magic ball from the pool  
//     public GameObject GetAvailableMagicBall()  
//     {  
//         // If there are available balls, return one  
//         if (availableBalls.Count > 0)  
//         {  
//             GameObject ball = availableBalls[0];  
//             availableBalls.RemoveAt(0);  
//             activeBalls.Add(ball);  
//             ball.SetActive(true); // Ensure the ball is active  
//             return ball;  
//         }  

//         // If no available balls, create a new one  
//         return SpawnMagicBall(Vector2.zero, Vector2.zero); // Default position and velocity  
//     }  

//     // Method to spawn a magic ball at specified position and velocity  
//     public GameObject SpawnMagicBall(Vector2 position, Vector2 velocity)  
//     {  
//         GameObject ball = Instantiate(magicBallPrefab, position, Quaternion.identity);  
//         Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();  
//         if (rb != null)  
//         {  
//             rb.velocity = velocity; // Set initial velocity  
//         }  
        
//         // Add to active list  
//         activeBalls.Add(ball);  
//         return ball;  
//     }  

//     // Method to return a magic ball to the pool  
//     public void ReturnMagicBall(GameObject ball)  
//     {  
//         // Disable the ball and remove from active list  
//         ball.SetActive(false);  
//         activeBalls.Remove(ball);  
//         availableBalls.Add(ball); // Add to available pool  
//     }  

//     // Method to resize the pool  
//     public void ResizePool(int count)  
//     {  
//         for (int i = 0; i < count; i++)  
//         {  
//             GameObject newBall = Instantiate(magicBallPrefab);  
//             newBall.SetActive(false); // Disable immediately  
//             availableBalls.Add(newBall); // Add to available ball list  
//         }  
//     }  
// }
using System.Collections.Generic;  
using UnityEngine;  

public class MagicBallPool : MonoBehaviour  
{  
    public GameObject magicBallPrefab; // The prefab to pool  
    public int poolSize = 10; // Number of magic balls to create in the pool  

    private Queue<GameObject> magicBallPool = new Queue<GameObject>();  

    private void Start()  
    {  
        // Initialize the pool  
        for (int i = 0; i < poolSize; i++)  
        {  
            GameObject magicBall = Instantiate(magicBallPrefab);  
            magicBall.SetActive(false); // Deactivate the magic ball  
            magicBallPool.Enqueue(magicBall); // Add to the pool  
        }  
    }  

    public GameObject GetMagicBall()  
    {  
        if (magicBallPool.Count > 0)  
        {  
            GameObject magicBall = magicBallPool.Dequeue(); // Get a magic ball from the pool  
            magicBall.SetActive(true); // Activate the magic ball  
            return magicBall;  
        }  
        else  
        {  
            // Optionally, instantiate a new magic ball if the pool is empty  
            GameObject magicBall = Instantiate(magicBallPrefab);  
            return magicBall;  
        }  
    }  

    public void ReturnMagicBall(GameObject magicBall)  
    {  
        magicBall.SetActive(false); // Deactivate the magic ball  
        magicBallPool.Enqueue(magicBall); // Return it to the pool  
    }  
}