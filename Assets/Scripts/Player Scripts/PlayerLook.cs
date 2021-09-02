using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    //Variables
    [Header("References")]
    [SerializeField] private WallRun wallRun;
    private Transform gun;
    private WeponSwitching weponSwitching;
    private PlayerDie playerDie;

    [Header("Sensitivity")]
    public float sensX = 300f;
    public float sensY = 300f;
    public float handgunADSSensX = 150f;
    public float handgunADSSensY = 125f;
    public float arADSSensX = 100f;
    public float arADSSensY = 75f;

    [Header("Camera")]
    [SerializeField] private Camera cam;
    [SerializeField] private Transform camTransform;
    [SerializeField] private Transform orientation;
    private float cameraFov;
    private float xRotation;
    private float yRotation;
    
    [Header("Fovs")]
    public float defaultFov = 80f;
    public float aimingFov = 1f;
    public float slidingFov = 1f;
    public float wallrunFov = 1f;

    [Header("Misc")]
    private float mouseX;
    private float mouseY;
    private float multiplier = 0.01f;

    //Unity Functions
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerDie = GetComponent<PlayerDie>();
        gun = transform.Find("CameraRecoilSway").Find("Camera").Find("RecoilSway").Find("WeponHolder");
        weponSwitching = transform.Find("CameraRecoilSway").Find("Camera").Find("RecoilSway").Find("WeponHolder").GetComponent<WeponSwitching>();
    }

    private void Update()
    {
        if(playerDie.playerDead) return;
        
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");
        
        if(weponSwitching.selectedWepon == 1 && gun.Find("Handgun").GetComponent<Gun>().isAiming)
        {
            yRotation += mouseX * handgunADSSensX * multiplier;
            xRotation -= mouseY * handgunADSSensY * multiplier;
        }
        else if(weponSwitching.selectedWepon == 2 && gun.Find("GunHeavy").GetComponent<Gun>().isAiming)
        {
            yRotation += mouseX * arADSSensX * multiplier;
            xRotation -= mouseY * arADSSensY * multiplier;
        }
        else
        {
            yRotation += mouseX * sensX * multiplier;
            xRotation -= mouseY * sensY * multiplier;            
        }

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        camTransform.transform.rotation = Quaternion.Euler(xRotation, yRotation, wallRun.tilt);
        orientation.transform.rotation = Quaternion.Euler(0, yRotation, 0);

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, defaultFov * aimingFov * slidingFov * wallrunFov, 10 * Time.deltaTime);
    }
}