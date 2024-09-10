using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{

    private bool levelCompleted = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "PLAYER" && !levelCompleted)
        {
            levelCompleted = true;
            Invoke("CompleteLevel", 0f);

        }
    }

    private void CompleteLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}