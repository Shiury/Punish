using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    public float moveSpeed;
    public float jumpForce = 14f;
    public SpriteRenderer sr;
    public int amountOfJump = 1;
    public float wallSlideSpeed;
    public float wallJumpForce;
    public float jumpTimerSet = 0.15f;

    private Rigidbody2D rb;
    private Animator anim;
    private int facingDirection = 1;
    private float moveInput;
    private int amountOfJumpLeft;
    private float jumpTimer;

    private bool isGrounded;
    private bool isWalking;
    private bool isTouchingWall;
    private bool isWallSliding;
    private bool isFacingRight = true;
    private bool canNormalJump;
    private bool canWallJump;
    private bool isAttemptingToJump = false;

    public float wallCheckDistance;
    public Transform groundCheck;
    public Transform wallCheck;
    public float checkRadius;
    public LayerMask Ground;
    public Vector2 wallJumpDirection;


  
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        amountOfJumpLeft = amountOfJump;
        wallJumpDirection.Normalize();

    }

    void Update()
    {
        
        checkInput();
        checkifcanjump();
        checkIfWallSliding();
        checkJump();
        FlipPlayer();
        animationTrigger();
        checkDirection();

        
        

    }

    void FixedUpdate()
    {
        
        applyMove();
        checkingSurroundings();

    }

    private void checkInput()
    {

        moveInput = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded || (amountOfJumpLeft > 0 && isTouchingWall))
            {
                normalJump();
            }
            else
            {
                jumpTimer = jumpTimerSet;
                isAttemptingToJump = true;
            }

        }
    }
      

    private void checkJump(){
        if (jumpTimer > 0)
        {
            if (!isGrounded && isTouchingWall)
            {
                wallJump();
            }
            else if (isGrounded)
            {
                normalJump();
            }
        }
            
        

        if (isAttemptingToJump)
        {
            jumpTimer -= Time.deltaTime;
        }
            

    }

    private void normalJump()
    {

        if (canNormalJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            amountOfJumpLeft--;
            jumpTimer = 0.0f;
            isAttemptingToJump = false;
        }
    }



    private void wallJump()
    {
        if (canWallJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0.0f);
            isWallSliding = false;
            amountOfJumpLeft = amountOfJump;
            amountOfJumpLeft--;
            Vector2 forceToAdd = new Vector2(wallJumpForce * wallJumpDirection.x * moveInput, wallJumpForce * wallJumpDirection.y);
            rb.AddForce(forceToAdd, ForceMode2D.Impulse);
            jumpTimer = 0.0f;
            isAttemptingToJump = false;
        }
    }

        
            

        
    

    private void checkifcanjump(){
        if (isGrounded && rb.velocity.y <= 0.01f){
            amountOfJumpLeft = amountOfJump;
        }


        if (isTouchingWall){
            canWallJump = true;
        }


        if (amountOfJumpLeft <= 0){
            canNormalJump = false;
        }
        else {
            canNormalJump = true;
        }
    }

    private void checkingSurroundings(){
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, Ground);
        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, Ground);

    }
   



    private void FlipPlayer()
    {
        if (moveInput > 0 && !isFacingRight || moveInput < 0 && isFacingRight )
        {
            facingDirection *= -1;
            isFacingRight = !isFacingRight;
            transform.Rotate(new Vector3(0, 180, 0));
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z)); 
    }

    private void checkIfWallSliding()
    {
        if (isTouchingWall && moveInput == facingDirection)
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void animationTrigger(){

        if (moveInput == 0)
        {
            anim.SetBool("isWalking", false);
        }
        else
        {
            anim.SetBool("isWalking", true);
        }

        if (isWallSliding){
            anim.SetBool("isSliding", true);
        }
        else {
            anim.SetBool("isSliding", false);
        }


        if (isGrounded != true)
        {
            anim.SetBool("isJumping", true);
        }
        else
        {
            anim.SetBool("isJumping", false);
        }
    }

    private void checkDirection(){
        if (moveInput < 0)
        {
            isFacingRight = false;
        }
        else if (moveInput > 0)
        {
            isFacingRight = true;
        }

        // checking is walking
        if (rb.velocity.x != 0){
            isWalking = true;
        }
        else {
            isWalking = false;
        }
    }       



    private void applyMove(){
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
             

        if (isWallSliding){
            if (rb.velocity.y < -wallSlideSpeed){
                rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
            }
        }
    }

}

    
    
    
    


