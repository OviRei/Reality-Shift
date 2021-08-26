using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public TextMeshProUGUI healthText;

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