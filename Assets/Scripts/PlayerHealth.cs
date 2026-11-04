using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;

    private int currentHealth;
    private bool isDead;

    public int CurrentHealth => currentHealth;
    public bool IsDead => isDead;

    private void Awake()
    {
        currentHealth = maxHealth;
        Debug.Log($"Player Health: {currentHealth}");
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead || damageAmount <= 0)
        {
            return;
        }

        currentHealth = Mathf.Max(currentHealth - damageAmount, 0);

        Debug.Log($"Player took {damageAmount} damage. Health: {currentHealth}");

        if (currentHealth == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("GAME OVER");
    }
}