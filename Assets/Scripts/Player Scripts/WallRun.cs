using UnityEngine;

public class WallRun : MonoBehaviour
{
    //Variables
    [Header("References")]
    private PlayerLook playerLook;
    private Rigidbody rb;
    private PlayerMovement playerMovement;

    [Header("Movement")]
    [SerializeField] private Transform orientation;

    [Header("Detection")]
    [SerializeField] private float wallDistance = .6f;
    [SerializeField] private float minimumJumpHeight = 1f;

    [Header("Wall Running")]
    public bool isWallrunning;
    [SerializeField] private float wallRunGravity = 2;
    [SerializeField] private float wallRunJumpForce = 4;
    public bool wallLeft = false;
    public bool wallRight = false;
    private bool CanWallRun() { return !Physics.Raycast(transform.position, Vector3.down, minimumJumpHeight); }

    [Header("Camera")]
    [SerializeField] private Camera cam;
    [SerializeField] private float wallRunFovMultiplier = 1.1f;
    //[SerializeField] private float wallRunfov = 60f;
    //[SerializeField] private float wallRunfovTime = 10f;
    [SerializeField] private float camTilt = 10f;
    [SerializeField] private float camTiltTime = 10f;
    public float tilt { get; private set; }

    [Header("Misc")]
    [SerializeField] private RaycastHit leftWallHit;
    [SerializeField] private RaycastHit rightWallHit;
    [SerializeField] private int lastWallID;
    [SerializeField] private int leftWallID;
    [SerializeField] private int rightWallID;
    private AudioSource jumpJetAudioSource;

    //Unity Functions
    private void Start()
    {
        playerLook = GetComponent<PlayerLook>();
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
        jumpJetAudioSource = GameObject.Find("JumpJet").GetComponent<AudioSource>();
    }

    private void Update()
    {
        CheckWall();

        if(playerMovement.isGrounded)
        {
            leftWallID = 1;
            rightWallID = 1;
            lastWallID = 0;
        }
        
        if(!isWallrunning && !playerMovement.isGrounded && !wallLeft && !wallRight)
        {
            if(leftWallID != 1) lastWallID = leftWallID;
            if(rightWallID != 1) lastWallID = rightWallID;
            leftWallID = 1;
            rightWallID = 1;
        }

        if(wallLeft) leftWallID = leftWallHit.transform.GetInstanceID();
        else if(wallRight) rightWallID = rightWallHit.transform.GetInstanceID();

        if(CanWallRun())
        {
            if(lastWallID == leftWallID || lastWallID == rightWallID) StopWallRun();
            else
            {
                if(wallLeft || wallRight) StartWallRun();
                else StopWallRun();
            }
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
        if(jumpJetAudioSource.clip.name != "Jumpjet_Wallrun_1" && !jumpJetAudioSource.isPlaying) FindObjectOfType<AudioManager>().Play("Jumpjet_Wallrun_1");

        isWallrunning = true;
        rb.useGravity = false;
        rb.AddForce(Vector3.down * wallRunGravity, ForceMode.Acceleration);

        playerLook.wallrunFov = wallRunFovMultiplier;
        playerMovement.canDoubleJump = true;

        if(wallLeft) tilt = Mathf.Lerp(tilt, -camTilt, camTiltTime * Time.deltaTime);
        else if(wallRight) tilt = Mathf.Lerp(tilt, camTilt, camTiltTime * Time.deltaTime);

        if(Input.GetKeyDown(playerMovement.jumpKey))
        {
            int rnd = Random.Range(1, 5);
            switch(rnd)
            {
            case 1:
                FindObjectOfType<AudioManager>().Play("Jump_1");
                break;
            case 2:
                FindObjectOfType<AudioManager>().Play("Jump_2");
                break;
            case 3:
                FindObjectOfType<AudioManager>().Play("Jump_3");
                break;
            case 4:
                FindObjectOfType<AudioManager>().Play("Jump_4");
                break;
            }

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
        isWallrunning = false;

        playerLook.wallrunFov = 1f;
        tilt = Mathf.Lerp(tilt, 0, camTiltTime * Time.deltaTime);
    }    
}