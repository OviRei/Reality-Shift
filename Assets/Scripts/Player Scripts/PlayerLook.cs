using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [Header("References")]
    [SerializeField] WallRun wallRun;
    [SerializeField] Gun gun;
    [SerializeField] WeponSwitching weponSwitching;

    public float sensX = 100f;
    public float sensY = 100f;

    [SerializeField] Transform cam = null;
    [SerializeField] Transform orientation = null;

    float mouseX;
    float mouseY;

    float multiplier = 0.01f;

    float xRotation;
    float yRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        weponSwitching = transform.Find("Camera").Find("WeponHolder").GetComponent<WeponSwitching>();
    }

    private void Update()
    {
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");

        if(weponSwitching.selectedWepon == 0 && transform.Find("Camera").Find("WeponHolder").Find("Handgun").GetComponent<Gun>().isAiming)
        {
            yRotation += mouseX * (sensX/2f) * multiplier;
            xRotation -= mouseY * (sensY/2.5f) * multiplier;
        }
        else if(weponSwitching.selectedWepon == 1 && transform.Find("Camera").Find("WeponHolder").Find("GunHeavy").GetComponent<Gun>().isAiming)
        {
            yRotation += mouseX * (sensX/2.5f) * multiplier;
            xRotation -= mouseY * (sensY/3f) * multiplier;
        }
        else
        {
            yRotation += mouseX * sensX * multiplier;
            xRotation -= mouseY * sensY * multiplier;            
        }

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cam.transform.rotation = Quaternion.Euler(xRotation, yRotation, wallRun.tilt);
        orientation.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}