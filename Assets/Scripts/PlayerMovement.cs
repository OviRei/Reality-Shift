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

    [Header("Keybinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] KeyCode slideKey = KeyCode.LeftControl;

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

    private bool OnSlope()
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
        ControlSpeed();

        if (Input.GetKeyDown(jumpKey))
        {
            Jump();
        }

        if(Input.GetKey(sprintKey) && Input.GetKey(slideKey) && isGrounded)
        {
            Slide();
        }
        else 
        {
            isSliding = false;
            slideTime = lengthOfSlide;
        }

        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
    }

    void MyInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        if(isSliding)
        {
            moveDirection = orientation.forward * verticalMovement;
        }
        else
        {
            moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;
        }
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

        if(slideTime <= 0)
        {
            isSliding = false;
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, slidingfovTime * Time.deltaTime);
        }
    }

    void ControlSpeed()
    {
        if (Input.GetKey(sprintKey) && !Input.GetKey(slideKey))
        {
            moveSpeed = Mathf.Lerp(moveSpeed, sprintSpeed, acceleration * Time.deltaTime);
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

    private void FixedUpdate()
    {
        MovePlayer();
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