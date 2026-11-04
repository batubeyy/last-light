using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed = 2f;

    private Rigidbody2D rb;

    private float knockbackEndTime;
    private float slowEndTime;
    private float slowMultiplier = 1f;

    public bool IsSlowed =>
        Time.time < slowEndTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void ApplyKnockback(
        Vector2 direction,
        float force,
        float duration
    )
    {
        knockbackEndTime = Time.time + duration;

        rb.linearVelocity = Vector2.zero;

        rb.AddForce(
            direction.normalized * force,
            ForceMode2D.Impulse
        );
    }

    public void ApplySlow(
        float multiplier,
        float duration
    )
    {
        slowMultiplier = Mathf.Clamp(
            multiplier,
            0.1f,
            1f
        );

        slowEndTime = Mathf.Max(
            slowEndTime,
            Time.time + duration
        );

        Debug.Log(
            $"{gameObject.name} slowed for {duration} seconds."
        );
    }

    private void FixedUpdate()
    {
        if (Time.time < knockbackEndTime)
        {
            return;
        }

        if (target == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        float currentSpeed = moveSpeed;

        if (IsSlowed)
        {
            currentSpeed *= slowMultiplier;
        }
        else
        {
            slowMultiplier = 1f;
        }

        Vector2 direction =
            ((Vector2)target.position - rb.position).normalized;

        rb.linearVelocity =
            direction * currentSpeed;
    }
}
