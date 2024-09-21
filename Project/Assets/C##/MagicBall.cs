// // // using UnityEngine;  

// // // public class MagicBall : MonoBehaviour  
// // // {  
// // //     public float speed = 5f; // Speed of the magic ball  
// // //     public float lifetime = 10f; // Lifetime of the magic ball before it is returned to the pool  
// // //     private Transform player; // Reference to the player  
// // //     private Vector2 direction; // Direction towards the player  

// // //     private void OnEnable()  
// // //     {  
// // //         // Find the player when the magic ball is enabled  
// // //         player = GameObject.FindGameObjectWithTag("Player").transform;  
// // //         if (player != null)  
// // //         {  
// // //             // Calculate the direction towards the player  
// // //             direction = (player.position - transform.position).normalized;  
// // //             // Set the velocity of the magic ball  
// // //             GetComponent<Rigidbody2D>().velocity = direction * speed;  
// // //         }  
// // //         else  
// // //         {  
// // //             Debug.LogError("Player not found!");  
// // //         }  

// // //         // Start the lifetime countdown when the magic ball is enabled  
// // //         Invoke("ReturnToPool", lifetime);  
// // //     }  

// // //     private void OnDisable()  
// // //     {  
// // //         // Cancel the invoke if the ball is disabled before the lifetime is up  
// // //         CancelInvoke();  
// // //     }  

// // //     private void ReturnToPool()  
// // //     {  
// // //         // Get the MagicBallPool reference  
// // //         MagicBallPool magicBallPool = FindObjectOfType<MagicBallPool>();  
// // //         if (magicBallPool != null)  
// // //         {  
// // //             magicBallPool.ReturnMagicBall(gameObject); // Return this magic ball to the pool  
// // //         }  
// // //     }  

// // //     private void OnCollisionEnter2D(Collision2D collision)  
// // //     {  
// // //         if (collision.gameObject.CompareTag("Player"))  
// // //         {  
// // //             // Handle damage to the player if needed  
// // //             PlayerCombat playerCombat = collision.gameObject.GetComponent<PlayerCombat>();  
// // //             if (playerCombat != null)  
// // //             {  
// // //                 playerCombat.ReceiveAttack(null, 10, false); // Adjust damage as needed  
// // //             }  
// // //             // Return the magic ball to the pool instead of destroying it  
// // //             ReturnToPool();  
// // //         }  
// // //         else if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Obstacle"))  
// // //         {  
// // //             // Return the magic ball to the pool upon collision with the ground or obstacles  
// // //             ReturnToPool();  
// // //         }  
// // //     }  
// // // }
// // using UnityEngine;  

// // public class MagicBall : MonoBehaviour  
// // {  
// //     public float speed = 5f; // Speed of the magic ball  
// //     public float lifetime = 10f; // Lifetime of the magic ball before it is returned to the pool  

// //     private Vector2 direction; // Direction towards the player  

// //     // This method initializes the magic ball to shoot towards the player  
// //     public void Initialize(Vector2 shootDirection)  
// //     {  
// //         direction = shootDirection.normalized; // Normalize the direction for consistent velocity  
// //         GetComponent<Rigidbody2D>().velocity = direction * speed; // Set the velocity  
// //     }  

// //     private void OnEnable()  
// //     {  
// //         // Start the lifetime countdown when the magic ball is enabled  
// //         Invoke("ReturnToPool", lifetime);  
// //     }  

// //     private void OnDisable()  
// //     {  
// //         // Cancel the invoke if the ball is disabled before the lifetime is up  
// //         CancelInvoke();  
// //     }  

// //     private void ReturnToPool()  
// //     {  
// //         // Get the MagicBallPool reference  
// //         MagicBallPool magicBallPool = FindObjectOfType<MagicBallPool>();  
// //         if (magicBallPool != null)  
// //         {  
// //             magicBallPool.ReturnMagicBall(gameObject); // Return this magic ball to the pool  
// //         }  
// //     }  

// //     private void OnCollisionEnter2D(Collision2D collision)  
// //     {  
// //         if (collision.gameObject.CompareTag("Player"))  
// //         {  
// //             // Damage the player if needed  
// //             PlayerCombat playerCombat = collision.gameObject.GetComponent<PlayerCombat>();  
// //             if (playerCombat != null)  
// //             {  
// //                 playerCombat.ReceiveAttack(null, 10, false); // Adjust damage as needed  
// //             }  
// //             // Return the magic ball to the pool instead of destroying it  
// //             ReturnToPool();  
// //         }  
// //         else if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Obstacle"))  
// //         {  
// //             // Return the magic ball to the pool upon collision with the ground or obstacles  
// //             ReturnToPool();  
// //         }  
// //     }  
// // }
// using UnityEngine;  

// public class MagicBall : MonoBehaviour  
// {  
//     public float speed = 5f; // Speed of the magic ball  
//     public float lifetime = 10f; // Lifetime of the magic ball before it is returned to the pool  

//     private Vector2 direction; // Direction towards the player  

//     // This method initializes the magic ball to shoot in a straight line  
//     public void Initialize(Vector2 shootDirection)  
//     {  
//         direction = shootDirection.normalized; // Normalize the direction for consistent velocity  
//         GetComponent<Rigidbody2D>().velocity = direction * speed; // Set the velocity  
//     }  

//     private void OnEnable()  
//     {  
//         // Start the lifetime countdown when the magic ball is enabled  
//         Invoke("ReturnToPool", lifetime);  
//     }  

//     private void OnDisable()  
//     {  
//         // Cancel the invoke if the ball is disabled before the lifetime is up  
//         CancelInvoke();  
//     }  

//     private void ReturnToPool()  
//     {  
//         // Get the MagicBallPool reference  
//         MagicBallPool magicBallPool = FindObjectOfType<MagicBallPool>();  
//         if (magicBallPool != null)  
//         {  
//             magicBallPool.ReturnMagicBall(gameObject); // Return this magic ball to the pool  
//         }  
//     }  

//     private void OnCollisionEnter2D(Collision2D collision)  
//     {  
//         if (collision.gameObject.CompareTag("Player"))  
//         {  
//             // Damage the player if needed  
//             PlayerCombat playerCombat = collision.gameObject.GetComponent<PlayerCombat>();  
//             if (playerCombat != null)  
//             {  
//                 playerCombat.ReceiveAttack(null, 10, false); // Adjust damage as needed  
//             }  
//             // Return the magic ball to the pool instead of destroying it  
//             ReturnToPool();  
//         }  
//         else if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Obstacle"))  
//         {  
//             // Return the magic ball to the pool upon collision with the ground or obstacles  
//             ReturnToPool();  
//         }  
//     }  
// }
using UnityEngine;  

public class MagicBall : MonoBehaviour  
{  
    public float speed = 5f; // Speed of the magic ball  
    public float lifetime = 10f; // Lifetime of the magic ball before it is returned to the pool  
    public int damage = 2; // Damage dealt by the magic ball  

    private Vector2 direction; // Direction towards the player  

    // This method initializes the magic ball to shoot in a straight line  
    public void Initialize(Vector2 shootDirection)  
    {  
        direction = shootDirection.normalized; // Normalize the direction for consistent velocity  
        GetComponent<Rigidbody2D>().velocity = direction * speed; // Set the velocity  
    }  

    private void OnEnable()  
    {  
        // Start the lifetime countdown when the magic ball is enabled  
        Invoke("ReturnToPool", lifetime);  
    }  

    private void OnDisable()  
    {  
        // Cancel the invoke if the ball is disabled before the lifetime is up  
        CancelInvoke();  
    }  

    private void ReturnToPool()  
    {  
        // Get the MagicBallPool reference  
        MagicBallPool magicBallPool = FindObjectOfType<MagicBallPool>();  
        if (magicBallPool != null)  
        {  
            magicBallPool.ReturnMagicBall(gameObject); // Return this magic ball to the pool  
        }  
    }  

    private void OnCollisionEnter2D(Collision2D collision)  
    {  
        if (collision.gameObject.CompareTag("Player"))  
        {  
            // Damage the player if needed  
            PlayerCombat playerCombat = collision.gameObject.GetComponent<PlayerCombat>();  
            if (playerCombat != null)  
            {  
                playerCombat.ReceiveAttack(null, damage, false); // Use the damage variable  
            }  
            // Return the magic ball to the pool instead of destroying it  
            ReturnToPool();  
        }  
        //else if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Obstacle"))  
        {  
            // Return the magic ball to the pool upon collision with the ground or obstacles  
            ReturnToPool();  
        }  
    }  
}