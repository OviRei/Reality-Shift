using UnityEngine;

public class Target : MonoBehaviour
{
    //Variables
    [Header("Health")]
    [SerializeField] private float maxHealth = 50f;
    [SerializeField] private float currentHealth = 50f;

    [Header("References")]
    [SerializeField] private HealthBar healthBar;

    //Unity Functions
    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.gameObject.SetActive(false);
    }

    //My Functions
    public void TakeDamage(float ammount)
    {
        currentHealth -= ammount;
        healthBar.gameObject.SetActive(true);
        healthBar.SetHealth(currentHealth, maxHealth);
        if(currentHealth <= 0f)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}