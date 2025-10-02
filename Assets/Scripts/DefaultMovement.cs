using UnityEngine;

public class DefaultMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float attackRange = 1f;
    public int attackDamage = 1;
    public float attackCooldown = 0.5f;
    public LayerMask enemyLayer;

    public LineRenderer attackLine;   // Drag in inspector
    public float lineDuration = 0.1f; // How long the line shows

    private float lastAttackTime;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Setup LineRenderer if not set
        if (attackLine != null)
        {
            attackLine.enabled = false;
            attackLine.positionCount = 2;
            attackLine.startWidth = 0.05f;
            attackLine.endWidth = 0.05f;
            attackLine.startColor = Color.red;
            attackLine.endColor = Color.red;
        }
    }

    void Update()
    {
        float move = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(move * moveSpeed, rb.linearVelocity.y);

        if (Input.GetMouseButtonDown(0) && Time.time > lastAttackTime + attackCooldown)
        {
            Attack();
        }
    }

    void Attack()
    {
        lastAttackTime = Time.time;

        // Check direction (facing right if localScale.x > 0)
        Vector2 attackDir = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        Vector2 attackOrigin = rb.position;

        // Raycast for enemies
        RaycastHit2D hit = Physics2D.Raycast(attackOrigin, attackDir, attackRange, enemyLayer);

        if (hit.collider != null)
        {
            EnemyHealth enemy = hit.collider.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(attackDamage);
            }
        }

        // Show red line
        if (attackLine != null)
        {
            StartCoroutine(ShowAttackLine(attackOrigin, attackOrigin + attackDir * attackRange));
        }
    }

    System.Collections.IEnumerator ShowAttackLine(Vector2 start, Vector2 end)
    {
        attackLine.SetPosition(0, start);
        attackLine.SetPosition(1, end);
        attackLine.enabled = true;

        yield return new WaitForSeconds(lineDuration);

        attackLine.enabled = false;
    }
}
