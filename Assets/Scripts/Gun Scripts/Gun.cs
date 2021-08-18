using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    //public float range = 100f;
    public float impactForce = 30f;
    public float fireRate = 15f;
    public bool singleFire = false;

    public int maxAmmo = 10;
    public int currentAmmo;
    public float reloadTime = 1f;
    private bool isReloading = false;
    public bool isAiming = false;

    public Camera cam;
    public ParticleSystem muzzleFlash;
    public Animator animator;

    private float nextTimeToFire = 0f;

    void start()
    {
        currentAmmo = maxAmmo;;
    }

    void Update()
    {
        if(isReloading) return;

        if(Input.GetKeyDown(KeyCode.R) || currentAmmo <= 0)
        {
            if(isAiming) StartCoroutine(ReloadWhileAiming());
            else StartCoroutine(Reload());
            return;
        }

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

        if(Input.GetMouseButton(1))
        {
            isAiming = true;
            animator.SetBool("Aiming", true);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 5, 10 * Time.deltaTime);
        }
        else
        {
            isAiming = false;
            animator.SetBool("Aiming", false);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 80, 10 * Time.deltaTime);
        }
    }

    void OnEnable()
    {
        isReloading = false;
        isAiming = false;
        animator.SetBool("Reloading", false);
        animator.SetBool("Aiming", false);
        animator.SetBool("AimingAndReloading", false);
    }

    IEnumerator Reload()
    {
        isReloading = true;

        animator.SetBool("Reloading", true);

        yield return new WaitForSeconds(reloadTime - .25f);
        animator.SetBool("Reloading", false);
        yield return new WaitForSeconds(.25f);

        currentAmmo = maxAmmo;
        isReloading = false;
    }

    IEnumerator ReloadWhileAiming()
    {
        isReloading = true;

        animator.SetBool("AimingAndReloading", true);

        yield return new WaitForSeconds(reloadTime - .25f);
        animator.SetBool("AimingAndReloading", false);
        yield return new WaitForSeconds(.25f);

        currentAmmo = maxAmmo;
        isReloading = false;
    }

    void Shoot()
    {
        RaycastHit hit;

        muzzleFlash.Play();

        currentAmmo--;

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
