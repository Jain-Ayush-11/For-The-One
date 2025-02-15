using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public bool isFacingRight = true,
        canFlip = true,
        hasClaimedShards = false,
        hasClaimedReward = false;

    private Rigidbody2D rb;
    private Animator anim;

    private float movementInputDirection;
    private float jumpForce = 16f;
    private bool isGrounded;
    private int numberOfJumpsLeft;
    private bool isWalking = false;

    public Transform groundCheck;
    public LayerMask whatIsGround;

    public float movementSpeed = 10f;
    public float groundCheckRadius;
    public int numberOfJumps = 2;
    public float playerHealth = 20;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        numberOfJumpsLeft = numberOfJumps;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        CheckMovementDirection();
        ApplyAnimations();
    }

    void FixedUpdate()
    {
        ApplyMovement();
        CheckSurrounding();
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    private void CheckInput()
    {
        movementInputDirection = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump")) {
            Jump();
        }
    }

    private void CheckMovementDirection()
    {
        if (
            (isFacingRight && movementInputDirection<0) ||
            (!isFacingRight && movementInputDirection>0)) {
            Flip();
        }
    }

    private void ApplyMovement()
    {
        rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);
        if(movementInputDirection!=0) {
            isWalking = true;
        } else {
            isWalking = false;
        }
    }

    private void Jump()
    {
        if (numberOfJumpsLeft>0) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            numberOfJumpsLeft--;
        }
    }

    private void Flip()
    {
        if (canFlip) {
            isFacingRight = !isFacingRight;
            transform.Rotate(0f, 180f, 0);
        }
    }

    private void DisableFlip()
    {
        canFlip = false;
    }

    private void EnableFlip()
    {
        canFlip = true;
    }

    private void CheckSurrounding()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        if (isGrounded && rb.velocity.y <= 0) {
            numberOfJumpsLeft = numberOfJumps;
        }
    }

    private void ApplyAnimations() {
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isWalking", isWalking);
        anim.SetFloat("PlayerVerticleVelocity", rb.velocity.y);
    }

    private void KillPlayer()
    {
        if (playerHealth <= 0) {
            gameObject.SetActive(false);
        }
    }
}
