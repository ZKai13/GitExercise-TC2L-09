using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float HP;
    public float maxHP;
    public Transform target;
    public float enemyMoveSpeed;
    public float FollowDistance;

    void Start()
    {
        HP = maxHP;
        target = GameObject.FindGameObjectWithTag("Player").transform;
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
