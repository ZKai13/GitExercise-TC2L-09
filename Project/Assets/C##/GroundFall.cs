using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundFall : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (rb != null && rb.bodyType == RigidbodyType2D.Static)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
            }
        }
    }
}
