using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DinosaurExhibit : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private AttackPulse attackPulse;

    [Header("Interaction")]
    [SerializeField] private float interactionRange = 2.5f;
    [SerializeField] private KeyCode activationKey = KeyCode.E;

    [Header("Attack")]
    [SerializeField] private float attackRadius = 3f;
    [SerializeField] private int damageAmount = 50;
    [SerializeField] private float slowedDamageMultiplier = 1.5f;

    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float knockbackForce = 8f;
    [SerializeField] private float knockbackDuration = 0.25f; 


    [Header("Visual Feedback")]
    [SerializeField] private Color activeColor = Color.yellow;
    [SerializeField] private Color cooldownColor = Color.gray;
    [SerializeField] private float activeColorDuration = 0.2f;

    [Header("Cooldown")]
    [SerializeField] private float cooldownDuration = 3f;

    private SpriteRenderer spriteRenderer;
    private Color inactiveColor;

    private float nextActivationTime;
    private bool isCoolingDown;
    private bool isShowingActiveColor;

    private Coroutine activeColorRoutine;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        inactiveColor = spriteRenderer.color;
    }

    private void Update()
    {
        if (player == null)
        {
            return;
        }

        HandleActivationInput();
        UpdateCooldownColor();
    }

    private void HandleActivationInput()
    {
        float distanceToPlayer = Vector2.Distance(
            transform.position,
            player.position
        );

        bool playerIsInRange =
            distanceToPlayer <= interactionRange;

        bool cooldownFinished =
            Time.time >= nextActivationTime;

        if (playerIsInRange &&
            cooldownFinished &&
            Input.GetKeyDown(activationKey))
        {
            Activate();
        }
    }

    private void UpdateCooldownColor()
    {
        if (isShowingActiveColor)
        {
            return;
        }

        if (Time.time < nextActivationTime)
        {
            spriteRenderer.color = cooldownColor;
            return;
        }

        if (isCoolingDown)
        {
            spriteRenderer.color = inactiveColor;
            isCoolingDown = false;
        }
    }

    private void Activate()
    {
        nextActivationTime =
            Time.time + cooldownDuration;

        isCoolingDown = true;

        if (activeColorRoutine != null)
        {
            StopCoroutine(activeColorRoutine);
        }

        activeColorRoutine =
            StartCoroutine(ShowActiveColor());

        if (attackPulse != null)
        {
            attackPulse.Play();
        }

        DamageEnemiesInRange();

        Debug.Log("Dinosaur activated.");
    }

    private void DamageEnemiesInRange()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            attackRadius,
            enemyLayer
        );

        int enemiesHit = 0;
        int combosTriggered = 0;

        foreach (Collider2D hit in hits)
        {
            EnemyHealth enemyHealth =
                hit.GetComponent<EnemyHealth>();

            EnemyMovement enemyMovement =
                hit.GetComponent<EnemyMovement>();

            if (enemyHealth == null)
            {
                continue;
            }

            int finalDamage = damageAmount;
            bool comboTriggered = false;

            if (enemyMovement != null &&
                enemyMovement.IsSlowed)
            {
                finalDamage = Mathf.RoundToInt(
                    damageAmount * slowedDamageMultiplier
                );

                comboTriggered = true;
                combosTriggered++;
            }

            Vector2 knockbackDirection =
                ((Vector2)hit.transform.position -
                 (Vector2)transform.position).normalized;

            enemyHealth.TakeDamage(finalDamage);
            enemiesHit++;

            if (!enemyHealth.IsDead &&
                enemyMovement != null)
            {
                enemyMovement.ApplyKnockback(
                    knockbackDirection,
                    knockbackForce,
                    knockbackDuration
                );
            }

            if (comboTriggered)
            {
                Debug.Log(
                    $"LIGHT COMBO! Dinozor dealt {finalDamage} damage."
                );
            }
        }

        Debug.Log(
            $"Dinosaur attack completed. " +
            $"Enemies hit: {enemiesHit}, " +
            $"Combos: {combosTriggered}"
        );
    }

    private IEnumerator ShowActiveColor()
    {
        isShowingActiveColor = true;
        spriteRenderer.color = activeColor;

        yield return new WaitForSeconds(
            activeColorDuration
        );

        isShowingActiveColor = false;
        spriteRenderer.color = cooldownColor;
        activeColorRoutine = null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(
            transform.position,
            interactionRange
        );

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(
            transform.position,
            attackRadius
        );
    }
}