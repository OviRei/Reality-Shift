using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float playerHeight = 2f;

    [SerializeField] Transform orientation;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 4f;
    [SerializeField] float airMultiplier = 0.4f;
    float movementMultiplier = 10f;

    [Header("Sprinting")]
    [SerializeField] float walkSpeed = 4f;
    [SerializeField] float sprintSpeed = 6f;
    [SerializeField] float acceleration = 10f;

    [Header("Jumping")]
    public float jumpForce = 5f;
    public bool canDoubleJump = true;

    [Header("Sliding")]
    public bool isSliding;
    public float lengthOfSlide;
    public float slideTime;
    public float slidingfov;
    public float slidingfovTime;
    public float slideSpeed;
    public bool isCrouching;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Camera")]
   
    [SerializeField] private Camera cam;
    [SerializeField] private float fov;

    [Header("Drag")]
    [SerializeField] float groundDrag = 6f;
    [SerializeField] float airDrag = 2f;

    float horizontalMovement;
    float verticalMovement;

    [Header("Ground Detection")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float groundDistance = 0.2f;
    bool isGrounded;

    Vector3 moveDirection;
    Vector3 slopeMoveDirection;

    Rigidbody rb;

    RaycastHit slopeHit;

    // added a reference to the WallRun script so it can be used to access wall run properties
    WallRun wallRun;

    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            if (slopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        // sets the WallRun script reference to the one that's being used by the player
        wallRun = GetComponent<WallRun>();
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        MyInput();
        ControlDrag();
        Crouch();

        if (Input.GetKeyDown(jumpKey)) Jump();

        if(!Input.GetKey(sprintKey) && Input.GetKey(crouchKey) && isGrounded) isCrouching = true;
        else isCrouching = false;

        if(Input.GetKey(sprintKey) && Input.GetKey(crouchKey) && isGrounded) Slide();
        else 
        {
            isSliding = false;
            slideTime = lengthOfSlide;
        }

        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
    }
    private void FixedUpdate()
    {
        MovePlayer();
        ControlSpeed();
    }
    void MyInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        if(isSliding) moveDirection = orientation.forward * verticalMovement;
        else moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;
    }

    void Crouch()
    {
        if(isCrouching) transform.Find("Camera").localPosition = new Vector3(0, 0.4f, 0) /*Vector3.down * 0.3f*/;
        else transform.Find("Camera").localPosition = new Vector3(0, 1, 0);
    }

    void Jump()
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

    void Slide()
    {
        isSliding = true;
        slideTime -= Time.deltaTime;

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, slidingfov, slidingfovTime * Time.deltaTime);
        transform.Find("Camera").localPosition = new Vector3(0, 0.4f, 0)/*Vector3.down * 0.3f*/;
        
        if(slideTime <= 0)
        {
            isSliding = false;
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, slidingfovTime * Time.deltaTime);
            transform.Find("Camera").localPosition = new Vector3(0, 1, 0);
        }
    }

    void ControlSpeed()
    {
        if (Input.GetKey(sprintKey) && !Input.GetKey(crouchKey))
        {
            moveSpeed = Mathf.Lerp(moveSpeed, sprintSpeed, acceleration * Time.deltaTime);
        }
        else if(isCrouching || !isSliding && Input.GetKey(crouchKey))
        {
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed/2, acceleration * Time.deltaTime);
        }
        else if(isSliding)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, slideSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, acceleration * Time.deltaTime);
        }
    }

    void ControlDrag()
    {
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
        }
    }

    void MovePlayer()
    {
        if(isGrounded && !OnSlope())
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if(isGrounded && OnSlope())
        {
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (!isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
        }
    }
}