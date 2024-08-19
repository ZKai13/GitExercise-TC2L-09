using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    public float HP;
    public float maxHP;
    public Transform target;
    public float enemyMoveSpeed;

    void Start()
    {
        HP = maxHP;
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FollowPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, enemy)
    }
}
