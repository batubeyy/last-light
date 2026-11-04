using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AttackPulse : MonoBehaviour
{
    [SerializeField] private float pulseDuration = 0.3f;
    [SerializeField] private float startScale = 0.2f;
    [SerializeField] private float endScale = 6f;
    [SerializeField] private float startAlpha = 0.35f;

    private SpriteRenderer spriteRenderer;
    private Coroutine pulseRoutine;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameObject.SetActive(false);
    }

    public void Play()
    {
        gameObject.SetActive(true);

        if (pulseRoutine != null)
        {
            StopCoroutine(pulseRoutine);
        }

        pulseRoutine = StartCoroutine(PlayPulse());
    }

    private IEnumerator PlayPulse()
    {
        float elapsedTime = 0f;

        while (elapsedTime < pulseDuration)
        {
            elapsedTime += Time.deltaTime;

            float progress =
                Mathf.Clamp01(elapsedTime / pulseDuration);

            float scale =
                Mathf.Lerp(startScale, endScale, progress);

            transform.localScale =
                new Vector3(scale, scale, 1f);

            Color color = spriteRenderer.color;
            color.a = Mathf.Lerp(
                startAlpha,
                0f,
                progress
            );
            spriteRenderer.color = color;

            yield return null;
        }

        pulseRoutine = null;
        gameObject.SetActive(false);
    }
}