using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EnemyHealth : MonoBehaviour
{
    public static event Action EnemyDefeated;

    [SerializeField] private int maxHealth = 100;
    [SerializeField] private Color hitColor = Color.red;
    [SerializeField] private float hitFlashDuration = 0.15f;

    private int currentHealth;
    private bool isDead;

    private SpriteRenderer spriteRenderer;
    private Color normalColor;
    private Coroutine hitFlashRoutine;

    public int CurrentHealth => currentHealth;
    public bool IsDead => isDead;

    private void Awake()
    {
        currentHealth = maxHealth;

        spriteRenderer = GetComponent<SpriteRenderer>();
        normalColor = spriteRenderer.color;
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead || damageAmount <= 0)
        {
            return;
        }

        currentHealth =
            Mathf.Max(currentHealth - damageAmount, 0);

        Debug.Log(
            $"{gameObject.name} took {damageAmount} damage. " +
            $"Health: {currentHealth}"
        );

        if (currentHealth == 0)
        {
            Die();
            return;
        }

        if (hitFlashRoutine != null)
        {
            StopCoroutine(hitFlashRoutine);
        }

        hitFlashRoutine = StartCoroutine(HitFlash());
    }

    private IEnumerator HitFlash()
    {
        Debug.Log("Enemy hit flash started.");

        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(hitFlashDuration);

        if (!isDead)
        {
            spriteRenderer.color = normalColor;
        }

        hitFlashRoutine = null;
    }

    private void Die()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;

        Debug.Log($"{gameObject.name} defeated.");

        EnemyDefeated?.Invoke();

        Destroy(gameObject);
    }
}
