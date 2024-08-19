using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector3 flippedScale = new Vector3(-1, 1, 1);

    private Rigidbody2D rigi;
    private Animator animator;

    float movesSpeed = 7f;
    float jumpForce = 5f;
    private int moveChangesAni;

    public float moveX;

    private Vector3 originalScale;
    private bool jump = true; // Declare the jump variable

    // Start is called before the first frame update
    void Start()
    {
        rigi = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Direction();
        Jump();
    }

    private void Movement()
    {
        moveX = Input.GetAxis("Horizontal");

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
        if (Input.GetKeyDown(KeyCode.Space) && jump)
        {
            rigi.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            animator.SetTrigger("jump");
            jump = false; // Disable jumping until player lands
        }

        // Reset jumping ability when the player is on the ground
        if (IsGrounded())
        {
            jump = true;
        }
    }

    private bool IsGrounded()
    {
        // Use a ground check method to determine if the player is on the ground
        // This example uses a simple raycast, adjust as needed
        return Physics2D.Raycast(transform.position, Vector2.down, 0.1f);
    }
}
