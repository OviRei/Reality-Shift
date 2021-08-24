using UnityEngine;

public class WallRun : MonoBehaviour
{
    //Variables
    [Header("References")]
    private Rigidbody rb;
    private PlayerMovement playerMovement;

    [Header("Movement")]
    [SerializeField] private Transform orientation;

    [Header("Detection")]
    [SerializeField] private float wallDistance = .6f;
    [SerializeField] private float minimumJumpHeight = 1f;

    [Header("Wall Running")]
    [SerializeField] private float wallRunGravity = 2;
    [SerializeField] private float wallRunJumpForce = 4;
    public bool wallLeft = false;
    public bool wallRight = false;
    private bool CanWallRun() { return !Physics.Raycast(transform.position, Vector3.down, minimumJumpHeight); }

    [Header("Camera")]
    [SerializeField] private Camera cam;
    [SerializeField] private float wallRunfov = 60f;
    [SerializeField] private float wallRunfovTime = 10f;
    [SerializeField] private float camTilt = 10f;
    [SerializeField] private float camTiltTime = 10f;
    public float tilt { get; private set; }

    [Header("Misc")]
    [SerializeField] private RaycastHit leftWallHit;
    [SerializeField] private RaycastHit rightWallHit;

    //Unity Functions
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        CheckWall();

        if(CanWallRun())
        {
            if(wallLeft || wallRight) StartWallRun();
            else StopWallRun();
        }
        else StopWallRun();
    }

    //My Functions
    private void CheckWall()
    {
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallDistance);
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallDistance);
    }

    private void StartWallRun()
    {
        rb.useGravity = false;
        rb.AddForce(Vector3.down * wallRunGravity, ForceMode.Acceleration);

        playerMovement.canDoubleJump = true;

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, wallRunfov, wallRunfovTime * Time.deltaTime);

        if(wallLeft) tilt = Mathf.Lerp(tilt, -camTilt, camTiltTime * Time.deltaTime);
        else if(wallRight) tilt = Mathf.Lerp(tilt, camTilt, camTiltTime * Time.deltaTime);

        if(Input.GetKeyDown(playerMovement.jumpKey))
        {
            if(wallLeft)
            {
                Vector3 wallRunJumpDirection = transform.up + leftWallHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(wallRunJumpDirection * wallRunJumpForce * 100, ForceMode.Force);
            }
            else if(wallRight)
            {
                Vector3 wallRunJumpDirection = transform.up + rightWallHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(wallRunJumpDirection * wallRunJumpForce * 100, ForceMode.Force);
            }
        }
    }

    private void StopWallRun()
    {
        rb.useGravity = true;

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, playerMovement.defaultFov, wallRunfovTime * Time.deltaTime);
        tilt = Mathf.Lerp(tilt, 0, camTiltTime * Time.deltaTime);
    }    
}