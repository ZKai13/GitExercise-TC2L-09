using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2TP1 : MonoBehaviour
{
    public Vector2 teleportPosition;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(TeleportWithDelay(other.transform));
        }
    }

    private IEnumerator TeleportWithDelay(Transform playerTransform)
    {
        yield return new WaitForSeconds(1f);
        playerTransform.position = teleportPosition;
    }
}
