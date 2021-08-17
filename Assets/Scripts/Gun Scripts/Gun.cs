using UnityEngine;

public class Gun : MonoBehaviour
{

    public float damage = 10f;
    //public float range = 100f;
    public float impactForce = 30f;
    public float fireRate = 15f;
    public bool singleFire = false;

    public Camera cam;
    public ParticleSystem muzzleFlash;

    private float nextTimeToFire = 0f;

    // Update is called once per frame
    void Update()
    {
        if(singleFire)
        {
            if(Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        else
        {
            if(Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                Shoot();
            }
        }
    }

    void Shoot()
    {
        RaycastHit hit;

        muzzleFlash.Play();

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit/*, range*/))
        {
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }
        }
    }
}
