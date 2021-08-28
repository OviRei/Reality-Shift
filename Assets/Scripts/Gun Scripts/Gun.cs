using UnityEngine;
using System.Collections;
using TMPro;
public class Gun : MonoBehaviour
{
    //Variables
    [Header("Shooting")]
    [SerializeField] private float damage = 10f;
    //[SerializeField] private float range = 100f;
    [SerializeField] private float impactForce = 30f;
    [SerializeField] private float fireRate = 15f;
    [SerializeField] private bool singleFire = false;
    [SerializeField] private float nextTimeToFire = 0f;
    [SerializeField] private float aimingFovMultiplier = 0.1f;

    [Header("Ammo")]
    public int maxAmmo = 10;
    public int currentAmmo;
    public int maxMags = 10;
    public int currentMags;
    [SerializeField] private float reloadTime = 1f;
    [SerializeField] private bool isReloading = false;
    public bool isAiming;

    [Header("Animator")]
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private Animator animator;
    
    [Header("Gun Recoil")]
    private Transform recoilSwayTransform;
    private Vector3 positionalRecoil = Vector3.zero;
    private Vector3 rotationalRecoil = Vector3.zero;
    private Vector3 recoilSwayTransformRotation = Vector3.zero;

    [Header("Camera Recoil")]
    private Transform cameraRecoilSwayTransform;
    private Vector3 cameraRotationalRecoil = Vector3.zero;
    private Vector3 cameraRecoilSwayTransformRotation = Vector3.zero;

    [Header("References")]
    [SerializeField] private GameObject crosshair;
    [SerializeField] private GameObject bulletComeOutPlace;
    [SerializeField] private Camera cam;
    private PlayerMovement playerMovement;
    private PlayerLook playerLook;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private TextMeshProUGUI magText;

    //Unity Functions
    private void Start()
    {
        currentAmmo = maxAmmo;
        currentMags = maxMags;
        ammoText.text = currentAmmo + "/" + maxAmmo;
        magText.text = currentMags.ToString();

        playerLook = transform.parent.parent.parent.parent.parent.GetComponent<PlayerLook>();
        playerMovement = transform.parent.parent.parent.parent.parent.GetComponent<PlayerMovement>();
        recoilSwayTransform = transform.parent.parent;
        cameraRecoilSwayTransform = transform.parent.parent.parent.parent;
    }

    private void Update()
    {
        ammoText.text = currentAmmo + "/" + maxAmmo;
        magText.text = currentMags.ToString();
        
        if(isReloading) return;

        if(Input.GetKeyDown(KeyCode.R) || currentAmmo <= 0)
        {
            if(currentMags >  0)
            {
                if(isAiming) StartCoroutine(ReloadWhileAiming());
                else StartCoroutine(Reload());
                return;                
            }
        }

        if(singleFire && Input.GetButtonDown("Fire1") && currentAmmo > 0) Shoot();
        else
        {
            if(Input.GetButton("Fire1") && Time.time >= nextTimeToFire && currentAmmo > 0)
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                Shoot();
            }
        }

        if(Input.GetMouseButton(1))
        {
            isAiming = true;
            animator.SetBool("Aiming", true);
            playerLook.aimingFov = aimingFovMultiplier;
            //cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 5, 10 * Time.deltaTime);
            crosshair.SetActive(false);
        }
        else
        {
            isAiming = false;
            animator.SetBool("Aiming", false);
            playerLook.aimingFov = 1f;
            //cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, playerLook.defaultFov, 10 * Time.deltaTime);
            crosshair.SetActive(true);
        }
    }

    private void FixedUpdate()
    {
        positionalRecoil = Vector3.Slerp(positionalRecoil, Vector3.zero, Time.deltaTime * 32);
        rotationalRecoil = Vector3.Slerp(rotationalRecoil, Vector3.zero, Time.deltaTime * 16);
        recoilSwayTransform.localPosition = Vector3.Slerp(recoilSwayTransform.localPosition, positionalRecoil, Time.deltaTime * 4);
        recoilSwayTransformRotation = Vector3.Slerp(recoilSwayTransformRotation, rotationalRecoil, Time.deltaTime * 4);
        recoilSwayTransform.localRotation = Quaternion.Euler(recoilSwayTransformRotation);

        cameraRotationalRecoil = Vector3.Slerp(cameraRotationalRecoil, Vector3.zero, Time.deltaTime * 16);
        cameraRecoilSwayTransformRotation = Vector3.Slerp(cameraRecoilSwayTransformRotation, cameraRotationalRecoil, Time.deltaTime * 4);
        cameraRecoilSwayTransform.localRotation = Quaternion.Euler(cameraRecoilSwayTransformRotation);
    }

    private void OnEnable()
    {
        isReloading = false;
        isAiming = false;

        animator.SetBool("Reloading", false);
        animator.SetBool("Aiming", false);
        animator.SetBool("AimingAndReloading", false);
    }

    private Quaternion GetQuaternion()
    {
        return Quaternion.Euler(cameraRecoilSwayTransformRotation);
    }

    //My Functions
    private void Shoot()
    {
        RaycastHit hit;

        muzzleFlash.Play();

        currentAmmo--;

        positionalRecoil += new Vector3(0, 0.15f, -0.1f);
        rotationalRecoil += new Vector3(-8, Random.Range(-8, 8), 0); 

        cameraRotationalRecoil += new Vector3(-8, Random.Range(-8, 8), 0); 

        if(isAiming && Physics.Raycast(bulletComeOutPlace.transform.position, bulletComeOutPlace.transform.forward, out hit) || Physics.Raycast(cam.transform.position, cam.transform.forward, out hit/*, range*/))
        {
            Target target = hit.transform.GetComponent<Target>();

            if(target != null) target.TakeDamage(damage);
            if(hit.rigidbody != null) hit.rigidbody.AddForce(-hit.normal * impactForce);
        }
    }

    private IEnumerator Reload()
    {
        isReloading = true;

        animator.SetBool("Reloading", true);

        yield return new WaitForSeconds(reloadTime - .25f);
        animator.SetBool("Reloading", false);
        yield return new WaitForSeconds(.25f);

        currentAmmo = maxAmmo;
        currentMags--;
        isReloading = false;
    }

    private IEnumerator ReloadWhileAiming()
    {
        isReloading = true;
        isAiming = false;

        animator.SetBool("AimingAndReloading", true);

        yield return new WaitForSeconds(reloadTime - .25f);
        animator.SetBool("AimingAndReloading", false);
        yield return new WaitForSeconds(.25f);

        currentAmmo = maxAmmo;
        currentMags--;
        isReloading = false;
        isAiming = true;
    }
}