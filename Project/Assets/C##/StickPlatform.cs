using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickPlatform : MonoBehaviour
{
    private Transform playerTransform;
    private Vector3 offset;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "PLAYER")
        {
            playerTransform = collision.gameObject.transform;
            offset = playerTransform.position - transform.position;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "PLAYER")
        {
            playerTransform = null;
        }
    }

    private void LateUpdate()
    {
        if (playerTransform != null)
        {
            playerTransform.position = transform.position + offset;
        }
    }
}
