using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(gameObject.name + " took " + damage + " damage! Remaining HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // For now just destroy, later we can add death animation
        Destroy(gameObject);
    }
}