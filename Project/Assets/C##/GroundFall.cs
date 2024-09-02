using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundFall : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Start()
    {
        // 获取物品的 Rigidbody2D 组件
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 如果物品的 Rigidbody2D 是静态的，将其切换为动态
            if (rb != null && rb.bodyType == RigidbodyType2D.Static)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
            }
        }
    }
}
