using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Slider slider;
    [SerializeField] private Gradient gradient;
    [SerializeField] private Image fill;
    [SerializeField] private TextMeshProUGUI healthText;

    public void SetMaxHealth(float maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;

        healthText.text = maxHealth + "/" + maxHealth;

        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(float currentHealth, float maxHealth)
    {
        slider.value = currentHealth;
        healthText.text = currentHealth + "/" + maxHealth;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}