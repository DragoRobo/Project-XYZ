using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FlyingMovement : MonoBehaviour
{
    public float moveSpeed = 4f;      // horizontal drift
    public float flapForce = 6f;      // upward force
    public Transform visual;

    private Rigidbody2D rb;
    private SpriteRenderer visualSprite;
    private float horizontal;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (visual != null) visualSprite = visual.GetComponent<SpriteRenderer>();
        else visualSprite = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        // Flap input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, flapForce);
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
        // apply horizontal movement
        rb.linearVelocity = new Vector2(horizontal * moveSpeed, rb.linearVelocity.y);
    }
}