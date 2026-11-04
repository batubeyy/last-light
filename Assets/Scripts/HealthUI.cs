using TMPro;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private TMP_Text healthText;

    private void Update()
    {
        if (playerHealth == null || healthText == null)
        {
            return;
        }

        healthText.text = $"Health: {playerHealth.CurrentHealth}";
    }
}