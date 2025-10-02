using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class DefaultMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 6f;

    public Transform visual;        // assign Visual child
    public Transform groundCheck;   // assign GroundCheck child
    public float groundCheckRadius = 0.12f;
    public LayerMask groundLayer;   // set this to your ground layer in inspector

    Rigidbody2D rb;
    SpriteRenderer visualSprite;
    float horizontal;
    bool wantJump;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (visual != null) visualSprite = visual.GetComponent<SpriteRenderer>();
        else visualSprite = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        // Jump input
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            wantJump = true;
        }

        // flip visual
        if (visualSprite != null)
        {
            if (horizontal > 0.01f) visualSprite.flipX = false;
            else if (horizontal < -0.01f) visualSprite.flipX = true;
        }
    }

    void FixedUpdate()
    {
        // move
        rb.linearVelocity = new Vector2(horizontal * moveSpeed, rb.linearVelocity.y);

        // jump applied in physics
        if (wantJump && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            wantJump = false;
        }
    }

    bool IsGrounded()
    {
        if (groundCheck == null) return false;
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer) != null;
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}