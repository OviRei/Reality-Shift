using UnityEngine;

public class Target : MonoBehaviour
{
    //Variables
    public float health = 50f;

    //My Functions
    public void TakeDamage(float ammount)
    {
        health -= ammount;
        if(health <= 0f)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}