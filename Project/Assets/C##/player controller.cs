using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector3 flippedScale = new Vector3(-1, 1, 1);
    //add a 3d position and direction to flipp the character

    private Rigidbody2D rigi;
    //used for physics calculations.
    private Animator animator;
    private Health health;
    

    float movesSpeed = 7f;
    float jumpForce = 5f;
    //The force applied when the player jumps
    private int moveChangesAni;
    //An integer used to change the player movement animation 

    public float moveX;
    //A float to store the player x axis input.

    private Vector3 originalScale;
    private bool jump = true; // Declare the jump variable

    // Start is called before the first frame update
    void Start()
    {
        rigi = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        //Initializes the Animator and rigidbody2d
        health = GetComponent<Health>();
        originalScale = transform.localScale;
        //Saves the player scale to handle character flipping.
    
    }

    // Update is called once per frame
     void Update()
    {
        if (health.currentHealth > 0)
        {
            Movement();
        //Handle the player movement input and animation
            Direction();
        //Manage the direction based on movement
            Jump();
        //Controls and check the jumping
        }
    }

    private void Movement()
    {
        moveX = Input.GetAxis("Horizontal");
        // X axis input (-1 to 1).

        rigi.velocity = new Vector2(moveX * movesSpeed, rigi.velocity.y);
        //Sets the player velocity based on input and movement speed

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
        //Determines the player's movement animation state(-1/0/1)
        animator.SetInteger("movement", moveChangesAni);
        //Update the animation state based on this movement state
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
        // If the player moves left, their scale on the x-axis is inverted to face left(-1). If moving right, the original scale is restored(1).
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jump)
        //Get Key dowm is to check the space key is pressed
        {
            rigi.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            //applies a upward force to make player jump
            animator.SetTrigger("jump");
            //check the player jump or not
            jump = false; 
            // Disable jumping until player lands the ground
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

private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            GameObject parentObject = new GameObject("ParentObject");
            parentObject.transform.position = transform.position; // 设置位置
            transform.parent = parentObject.transform; // 设置角色的父物体
            parentObject.transform.parent = collision.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            transform.parent = null; // 解除父物体
            Destroy(transform.parent.gameObject); // 删除空物体 
        }
    }
}

