using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playercontroller : MonoBehaviour
{
    Vector3 flippedScale=new Vector3(-1,1,1);

    private Rigidbody2D rigi;
    private Animator animator;

    float movesSpeed= 10f;  
    float jumpForce = 7f;
    private int moveChangesAni;

    public float moveX;

    private Vector3 originalScale;


    float moveY;
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
        moveX= Input.GetAxis("Horizontal");
        moveY= Input.GetAxisRaw("Vertical");

        rigi.velocity= new Vector2(moveX * movesSpeed, rigi.velocity.y);

        if(moveX>0)
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

        animator.SetInteger("movement",moveChangesAni);
    }

    private void Direction()
    {
        if (moveX > 0)
        {
            transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
        }
        else if (moveX < 0 )
        {
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
        }
    }
private bool jump = true;

private void Jump()
{
    if (Input.GetKeyDown(KeyCode.Space) && jump)
    {
        rigi.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        animator.SetTrigger("jump");
        jump = false; // Disable jumping until player lands
    }

    // Reset jumping ability when the player is on the ground
    if (rigi.velocity.y == 0)
    {
        jump = true;
    }
    
}
}

