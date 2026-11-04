using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundManager : MonoBehaviour
{
    [Header("Round")]
    [SerializeField] private float roundDuration = 60f;

    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text victoryText;

    private float remainingTime;
    private int defeatedCount;
    private bool roundFinished;

    private void OnEnable()
    {
        EnemyHealth.EnemyDefeated += HandleEnemyDefeated;
    }

    private void OnDisable()
    {
        EnemyHealth.EnemyDefeated -= HandleEnemyDefeated;
    }

    private void Start()
    {
        Time.timeScale = 1f;

        remainingTime = roundDuration;
        defeatedCount = 0;
        roundFinished = false;

        if (victoryText != null)
        {
            victoryText.gameObject.SetActive(false);
        }

        UpdateUI();
    }

    private void Update()
    {
        if (roundFinished)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                RestartRound();
            }

            return;
        }

        if (playerHealth != null && playerHealth.IsDead)
        {
            return;
        }

        remainingTime -= Time.deltaTime;
        remainingTime = Mathf.Max(remainingTime, 0f);

        UpdateUI();

        if (remainingTime <= 0f)
        {
            FinishRound();
        }
    }

    private void HandleEnemyDefeated()
    {
        if (roundFinished)
        {
            return;
        }

        defeatedCount++;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (timerText != null)
        {
            timerText.text =
                $"Time: {Mathf.CeilToInt(remainingTime)}";
        }

        if (scoreText != null)
        {
            scoreText.text =
                $"Defeated: {defeatedCount}";
        }
    }

    private void FinishRound()
    {
        roundFinished = true;

        if (victoryText != null)
        {
            victoryText.gameObject.SetActive(true);
        }

        Time.timeScale = 0f;

        Debug.Log(
            $"ROUND COMPLETE. Enemies defeated: {defeatedCount}"
        );
    }

    private void RestartRound()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}