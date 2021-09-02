using UnityEngine;

public class PlayerDie : MonoBehaviour
{
    [Header("Death")]
    public bool playerDead = false;

    [Header("References")]
    private Rigidbody rb;
    [SerializeField] private GameObject respawnBar;
    [SerializeField] private Transform respawnPoint;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        respawnPoint = GameObject.Find("RespawnPoint").transform;
    }

    private void Update()
    {
        if(playerDead && Input.GetKeyDown(KeyCode.Space)) UnDie();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 15) Die();
        if(collision.gameObject.layer == 16) respawnPoint = collision.gameObject.transform;
    }

    public void Die()
    {
        playerDead = true;
        rb.freezeRotation = false;

        respawnBar.SetActive(true);
    }

    public void UnDie()
    {
        transform.position = respawnPoint.transform.position;
        transform.rotation = Quaternion.Euler(0, 0, 0);

        playerDead = false;
        rb.freezeRotation = true;
        respawnBar.SetActive(false);
    }
}