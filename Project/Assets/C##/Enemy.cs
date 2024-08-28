using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;// The maximum health of the enemy
    int currentHealth;// The current health of the enemy, starts at maxHealth
    public Transform target;// The player or object the enemy will follow
    public float enemyMoveSpeed;// The speed at which the enemy moves towards the target
    public float FollowDistance;// The distance within which the enemy starts following the player
    void Start()
    {
        currentHealth = maxHealth;// Initialize the enemy's current health to maxHealth
        target = GameObject.FindGameObjectWithTag("Player").transform;// Find the player by tag and set it as the target
    }
    public void Takedamage(int damage)
    {
        currentHealth -= damage;// Reduce the enemy's health by the damage amount
        if(currentHealth <= 0)// Reduce the enemy's health by the damage amount
        {
            Die();// Call the Die method if health is depleted
        }
        
        void Die()
        {
            //Die animation
            Debug.Log("Enemy Died!");// Log message when the enemy dies
        }
    }
    // Update is called once per frame
    void Update()
    {
        FollowPlayer();// Calls the FollowPlayer method every frame
    }

    void FollowPlayer()// Check if the player is within the follow distance
    {
        if (transform.position.x - target.position.x < FollowDistance)
        {
        transform.position = Vector2.MoveTowards(transform.position, target.position, enemyMoveSpeed * Time.deltaTime);
        if (transform.position.x - target.position.x < 0) transform.eulerAngles = new Vector3(0, 180, 0);
        if (transform.position.x - target.position.x > 0) transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }
}
