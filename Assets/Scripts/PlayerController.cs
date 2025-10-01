using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 6f;
    public float flapForce = 6f; // new force for flappy bird movement

    public Sprite squareSprite;
    public Sprite flyingSprite;
    public Sprite smallSprite;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private enum MorphState { Square, Flying, Small }
    private MorphState currentState = MorphState.Square;
    
    private GameObject carriedBlock;
    public Transform carryPoint; // empty child transform where the block will sit

    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        SetMorphState(MorphState.Square);
    }

    void Update()
    {
        // Movement (all forms)
        float move = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(move * moveSpeed, rb.linearVelocity.y);

        // Jump (Square + Small only)
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && currentState != MorphState.Flying)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // Flying (Flappy Bird style)
        if (currentState == MorphState.Flying && Input.GetKeyDown(KeyCode.Space))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, flapForce);
        }

        // Morph switching
        if (Input.GetKeyDown(KeyCode.Alpha1)) SetMorphState(MorphState.Square);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SetMorphState(MorphState.Flying);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SetMorphState(MorphState.Small);

        // If carrying a block and we switch away from Square, auto-drop
        if (carriedBlock != null && currentState != MorphState.Square)
        {
            DropBlock();
        }

        // Grab / Drop (only in Square form)
        if (currentState == MorphState.Square && Input.GetKeyDown(KeyCode.E))
        {
            if (carriedBlock == null)
            {
                TryPickupBlock();
            }
            else
            {
                DropBlock();
            }
        }
    }

    void TryPickupBlock()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, 1f, LayerMask.GetMask("Block"));
        if (hit == null) return;

        carriedBlock = hit.gameObject;
        Rigidbody2D blockRb = carriedBlock.GetComponent<Rigidbody2D>();
        Collider2D blockCol = carriedBlock.GetComponent<Collider2D>();
        Collider2D playerCol = GetComponent<Collider2D>();

        blockRb.bodyType = RigidbodyType2D.Kinematic;
        Physics2D.IgnoreCollision(blockCol, playerCol, true);

        carriedBlock.transform.SetParent(carryPoint);
        carriedBlock.transform.localPosition = Vector3.zero;
    }

    void DropBlock()
    {
        if (carriedBlock == null) return;

        Rigidbody2D blockRb = carriedBlock.GetComponent<Rigidbody2D>();
        Collider2D blockCol = carriedBlock.GetComponent<Collider2D>();
        Collider2D playerCol = GetComponent<Collider2D>();

        Physics2D.IgnoreCollision(blockCol, playerCol, false);
        blockRb.bodyType = RigidbodyType2D.Static;

        carriedBlock.transform.SetParent(null);
        carriedBlock = null;
    }

    void SetMorphState(MorphState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case MorphState.Square:
                spriteRenderer.sprite = squareSprite;
                transform.localScale = Vector3.one * 1f;
                rb.gravityScale = 2f;
                break;

            case MorphState.Flying:
                spriteRenderer.sprite = flyingSprite;
                transform.localScale = new Vector3(1f, 0.5f, 1f);
                rb.gravityScale = 2f; // normal gravity, works with flaps
                break;

            case MorphState.Small:
                spriteRenderer.sprite = smallSprite;
                transform.localScale = Vector3.one * 0.2f;
                rb.gravityScale = 2f;
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.5f)
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
}
