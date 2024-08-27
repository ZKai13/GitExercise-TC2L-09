using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    int currentHealth;
    public Transform target;
    public float enemyMoveSpeed;
    public float FollowDistance;
    void Start()
    {
        currentHealth = maxHealth;
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    public void Takedamage(int damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            Die();
        }
        
        void Die()
        {
            //Die animation
            Debug.Log("Enemy Died!");
        }
    }
    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
    }

    void FollowPlayer()
    {
        if (transform.position.x - target.position.x < FollowDistance)
        {
        transform.position = Vector2.MoveTowards(transform.position, target.position, enemyMoveSpeed * Time.deltaTime);
        if (transform.position.x - target.position.x < 0) transform.eulerAngles = new Vector3(0, 0, 0);
        if (transform.position.x - target.position.x > 0) transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }
}
