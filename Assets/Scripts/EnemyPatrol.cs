using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float speed = 2f;               // Patrol speed
    public Transform groundCheck;          // Empty child transform to detect ground
    public float groundCheckDistance = 1f; // How far to check below
    public LayerMask groundLayer;          // What counts as ground

    private Rigidbody2D rb;
    private bool movingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Move enemy left or right
        rb.linearVelocity = new Vector2((movingRight ? 1 : -1) * speed, rb.linearVelocity.y);

        // Check ground ahead
        RaycastHit2D groundInfo = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);

        if (groundInfo.collider == false) // No ground? Turn around
        {
            Flip();
        }
    }

    void Flip()
    {
        movingRight = !movingRight;

        // Mirror the enemy sprite
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Turn around if hit a wall
        if (collision.collider.CompareTag("Wall"))
        {
            Flip();
        }
    }
}