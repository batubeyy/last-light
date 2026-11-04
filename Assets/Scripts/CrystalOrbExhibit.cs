using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CrystalOrbExhibit : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private AttackPulse slowPulse;

    [Header("Interaction")]
    [SerializeField] private float interactionRange = 2.5f;
    [SerializeField] private KeyCode activationKey = KeyCode.E;

    [Header("Slow Effect")]
    [SerializeField] private float effectRadius = 3f;
    [SerializeField] private float slowMultiplier = 0.35f;
    [SerializeField] private float slowDuration = 3f;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Visual Feedback")]
    [SerializeField] private Color activeColor = Color.white;
    [SerializeField] private Color cooldownColor = Color.gray;
    [SerializeField] private float activeColorDuration = 0.2f;

    [Header("Cooldown")]
    [SerializeField] private float cooldownDuration = 5f;

    private SpriteRenderer spriteRenderer;
    private Color readyColor;

    private float nextActivationTime;
    private bool isCoolingDown;
    private bool isShowingActiveColor;

    private Coroutine activeColorRoutine;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        readyColor = spriteRenderer.color;
    }

    private void Update()
    {
        if (player == null)
        {
            return;
        }

        HandleActivationInput();
        UpdateVisualState();
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

    private void UpdateVisualState()
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
            spriteRenderer.color = readyColor;
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

        if (slowPulse != null)
        {
            slowPulse.Play();
        }

        SlowEnemiesInRange();
    }

    private void SlowEnemiesInRange()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            effectRadius,
            enemyLayer
        );

        int enemiesSlowed = 0;

        foreach (Collider2D hit in hits)
        {
            EnemyMovement enemyMovement =
                hit.GetComponent<EnemyMovement>();

            if (enemyMovement == null)
            {
                continue;
            }

            enemyMovement.ApplySlow(
                slowMultiplier,
                slowDuration
            );

            enemiesSlowed++;
        }

        Debug.Log(
            $"Crystal Orb activated. Enemies slowed: {enemiesSlowed}"
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

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(
            transform.position,
            effectRadius
        );
    }
}