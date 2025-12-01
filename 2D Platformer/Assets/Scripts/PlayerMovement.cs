using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    public Rigidbody2D rb;
    public bool isGrounded;
    private float moveInput;

    public Animator animator;

    [Header("Dash Settings")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    private bool isDashing = false;
    private float dashTimeLeft;
    public bool canHurt = true;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get horizontal input (-1 to 1)
        moveInput = Input.GetAxisRaw("Horizontal");

        animator.SetFloat("Speed", Mathf.Abs(moveInput));

        // Flip sprite
        if (moveInput < 0)
            GetComponent<SpriteRenderer>().flipX = true;
        else if (moveInput > 0)
            GetComponent<SpriteRenderer>().flipX = false;

        // Jump input
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        animator.SetBool("IsJumping", !isGrounded && rb.linearVelocity.y > 0);
        animator.SetBool("IsFalling", !isGrounded && rb.linearVelocity.y < 0);

        // Dash input
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
        {
            StartDash();
            
            animator.SetBool("isDashing", isDashing);
        }

        if (isDashing)
        {
            dashTimeLeft -= Time.deltaTime;
            
            canHurt = false;

            if (dashTimeLeft <= 0)
            {
                isDashing = false;
                canHurt = true;
                
                animator.SetBool("isDashing", isDashing);
            }
        }
    }

    void FixedUpdate()
    {
        if (!isDashing)
        {
            // Apply normal movement
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        }

        // Ground check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded)
        {
            animator.SetBool("IsJumping", false);
        }
    }

    private void StartDash()
    {
        isDashing = true;
        dashTimeLeft = dashDuration;

        // Dash direction based on input or facing
        float dashDirection = moveInput != 0 ? moveInput : (GetComponent<SpriteRenderer>().flipX ? -1 : 1);
        rb.linearVelocity = new Vector2(dashDirection * dashSpeed, 0f);
    }
}
