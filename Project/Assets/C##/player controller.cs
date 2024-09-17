using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector3 flippedScale = new Vector3(-1, 1, 1);
    private Rigidbody2D rigi;
    private Animator animator;
    private Health health;

    float movesSpeed = 7f;  
    float jumpForce = 5f;
    private int moveChangesAni;

    public float moveX;
    private Vector3 originalScale;
    private bool jump = true;

    private KeyRebinding keyRebinding;

    void Start()
    {
        rigi = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
        originalScale = transform.localScale;

        keyRebinding = FindObjectOfType<KeyRebinding>();
        if (keyRebinding == null)
        {
            Debug.LogError("KeyRebinding component not found in the scene.");
        }
    }

    void Update()
    {
        if (health.currentHealth > 0)
        {
            Movement();
            Direction();
            Jump();
        }
    }

    private void Movement()
    {
        if (keyRebinding == null) return;

        if (Input.GetKey(keyRebinding.GetKeyForAction("MoveLeft")))
        {
            moveX = -1f;
        }
        else if (Input.GetKey(keyRebinding.GetKeyForAction("MoveRight")))
        {
            moveX = 1f;
        }
        else
        {
            moveX = 0f;
        }

        rigi.velocity = new Vector2(moveX * movesSpeed, rigi.velocity.y);

        if (moveX > 0)
        {
            moveChangesAni = 1;
        }
        else if (moveX < 0)
        {
            moveChangesAni = -1;
        }
        else
        {
            moveChangesAni = 0;
        }

        animator.SetInteger("movement", moveChangesAni);
    }

    private void Direction()
    {
        if (moveX > 0)
        {
            transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
        }
        else if (moveX < 0)
        {
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
        }
    }

    private void Jump()
    {
        if (keyRebinding == null) return;

        if (Input.GetKeyDown(keyRebinding.GetKeyForAction("Jump")) && jump)
        {
            rigi.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            animator.SetTrigger("jump");
            jump = false;
        }

        if (IsGrounded())
        {
            jump = true;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, 0.1f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            GameObject parentObject = new GameObject("ParentObject");
            parentObject.transform.position = transform.position;
            transform.parent = parentObject.transform;
            parentObject.transform.parent = collision.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            transform.parent = null;
            Destroy(transform.parent.gameObject);
        }
    }
}
