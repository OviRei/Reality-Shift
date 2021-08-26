using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Variables
    [Header("Player")]
    [SerializeField] private float playerHeight = 2f;
    [SerializeField] private Transform orientation;
    
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float airMultiplier = 0.2f;
    [SerializeField] private float movementMultiplier = 10f;
    private float horizontalMovement;
    private float verticalMovement;

    [Header("Walking/Sprinting")]
    [SerializeField] private float walkSpeed = 4f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private float acceleration = 10f;

    [Header("Jumping")]
    [SerializeField] private float jumpForce = 12f;
    public bool canDoubleJump;

    [Header("Crouching")]
    [SerializeField] private float crouchSpeed = 2f;
    [SerializeField] private float crouchHeight = 0.4f;  

    [Header("Sliding")]
    [SerializeField] private float slideSpeed = 10f;    
    [SerializeField] private float lengthOfSlide = 1f;
    [SerializeField] private float slideTime = 0f;
    [SerializeField] private float slidingFovMultiplier = 1.9f;

    [Header("Drag")]
    [SerializeField] private float groundDrag = 6f;
    [SerializeField] private float airDrag = 0.4f;

    [Header("States")]
    public bool isSliding;
    public bool isCrouching;
    public bool isGrounded;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Camera")]
    [SerializeField] private Camera cam;
    [SerializeField] private float cameraHeight = 1f;  

    [Header("Ground Detection")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundDistance = 0.4f;
    
    [Header("Slopes")]
    private Vector3 moveDirection;
    private Vector3 slopeMoveDirection;
    private RaycastHit slopeHit;

    [Header("References")]
    private PlayerLook playerLook;
    private Rigidbody rb;
    private WallRun wallRun;


    //Unity Functions
    private void Start()
    {
        //Sets References
        playerLook = GetComponent<PlayerLook>();
        wallRun = GetComponent<WallRun>();
        rb = GetComponent<Rigidbody>();

        rb.freezeRotation = true;
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        MovementInput();
        Crouch();
    }

    private void FixedUpdate()
    {
        ControlMovement();
        ControlSpeed();
    }

    //My Functions
    private void MovementInput()
    {
        if(Input.GetKeyDown(jumpKey)) Jump();

        if(!Input.GetKey(sprintKey) && Input.GetKey(crouchKey) && isGrounded) isCrouching = true;
        else isCrouching = false;

        if(Input.GetKey(sprintKey) && Input.GetKey(crouchKey) && isGrounded) Slide();
        else
        {
            isSliding = false;
            playerLook.slidingFov = 1f;
            slideTime = lengthOfSlide;
        }
    }

    private void ControlMovement()
    {
        if(isSliding) moveDirection = orientation.forward * verticalMovement;
        else moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;

        if(isGrounded && !OnSlope()) 
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        else if(isGrounded && OnSlope()) 
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        else if(!isGrounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
    }

    private void ControlSpeed()
    {
        if(isGrounded) rb.drag = groundDrag;
        else rb.drag = airDrag;

        if(Input.GetKey(sprintKey) && !Input.GetKey(crouchKey))
            moveSpeed = Mathf.Lerp(moveSpeed, sprintSpeed, acceleration * Time.deltaTime);
        else if(Input.GetKey(crouchKey) && !isSliding || isCrouching)
            moveSpeed = Mathf.Lerp(moveSpeed, crouchSpeed, acceleration * Time.deltaTime);
        else if(isSliding)
            moveSpeed = Mathf.Lerp(moveSpeed, slideSpeed, acceleration * Time.deltaTime);
        else
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, acceleration * Time.deltaTime);
    }

    private void Jump()
    {
        if(isGrounded)
        {
            canDoubleJump = true;
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
        else if(canDoubleJump && !(wallRun.wallLeft || wallRun.wallRight))
        {
            canDoubleJump = false;
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }   
    }

    private void Crouch()
    {
        if(isCrouching) transform.Find("CameraRecoilSway").Find("Camera").localPosition = new Vector3(0, crouchHeight, 0);
        else transform.Find("CameraRecoilSway").Find("Camera").localPosition = new Vector3(0, cameraHeight, 0);
    }

    public bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            if(slopeHit.normal != Vector3.up) return true;
            else return false;
        }
        return false;
    }

    private void Slide()
    {
        isSliding = true;
        slideTime -= Time.deltaTime;

        playerLook.slidingFov = slidingFovMultiplier;
        transform.Find("CameraRecoilSway").Find("Camera").localPosition = new Vector3(0, crouchHeight, 0);
        
        if(slideTime <= 0)
        {
            isSliding = false;
            playerLook.slidingFov = 1f;
            transform.Find("CameraRecoilSway").Find("Camera").localPosition = new Vector3(0, cameraHeight, 0);
        }
    }
}