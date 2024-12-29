using System.Collections;
using Mono.CSharp;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour
{
    #region type_defs

    private Vector3 startPosition;
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    private Coroutine jumpCoroutine;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    public Transform orientation;

    [Header("Keybinds")]
    [SerializeField] private InputActionReference movement;
    [SerializeField] private InputActionReference jump;
    [SerializeField] private InputActionReference sprintKey;
    [SerializeField] private InputActionReference crouchKey;
    [SerializeField] private InputActionReference interactKey;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    [Header("UI_stuff")]
    public Image fadenkreuz;
    public Sprite normal_fadenkreuz;
    public Sprite interactable_fadenkreuz;
    private bool fadenkreuz_ist_interactable;

    [Header("Interact")]
    [SerializeField] private float interactRange;

    [Header("Misc")]

    public Movementstate state;
    public enum Movementstate
    {
        freeze,
        walking,
        sprinting,
        crouching,
        air
    }
    public bool freeze;

    #endregion

    private void OnEnable()
    {
        movement.action.Enable();
        jump.action.Enable();
        sprintKey.action.Enable();
        crouchKey.action.Enable();
        interactKey.action.Enable();
    }

    private void OnDisable()
    {
        movement.action.Disable();
        jump.action.Disable();
        sprintKey.action.Disable();
        crouchKey.action.Disable();
        interactKey.action.Disable();
    }
    // Start is called before the first frame update
    private void Start()
    {
        startPosition = transform.position;

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        jumpCoroutine = null;
        UnityEngine.Physics.gravity = new Vector3(0, -18.62f, 0);

        startYScale = transform.localScale.y;
    }

    // Update is called once per frame
    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down,
            playerHeight * 0.5f + 0.2f, whatIsGround);
        HandleMovementState();
        HandleInput();
        SpeedControl();
        Interact();

        //handle drag
        rb.linearDamping = grounded ? groundDrag : 0f;
    }

    private void FixedUpdate()
    {
        MovePlayer();
        //Debug.Log(state + " " + moveSpeed);
    }

    public void resetPosition()
    {
        transform.position = startPosition;
    }

    private void HandleInput()
    {
        horizontalInput = movement.action.ReadValue<Vector2>().x;
        verticalInput = movement.action.ReadValue<Vector2>().y;

        var playerJumped = jump.action.IsPressed();

        // Invoke() ist teuer
        if (playerJumped && jumpCoroutine == null && grounded)
        {
            jumpCoroutine = StartCoroutine(Jump());
        }
        
        //start crouch
        if (crouchKey.action.WasPressedThisFrame())
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);  //um den schwebenden player schnell wieder auf den boden zu dr�cken
        } //stop crouch
        else if (crouchKey.action.WasReleasedThisFrame())
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    private void HandleMovementState()
    {
        if (freeze)
        {
            state = Movementstate.freeze;
            moveSpeed = 0f;
            rb.linearVelocity = Vector3.zero;
        }
        else if (crouchKey.action.IsPressed())
        {
            state = Movementstate.crouching;
            moveSpeed = crouchSpeed;
        }
        else if (grounded && sprintKey.action.IsPressed())
        {
            state = Movementstate.sprinting;
            moveSpeed = sprintSpeed;
        }
        else if (grounded)
        {
            state = Movementstate.walking;
            moveSpeed = walkSpeed;
        }
        else
        {
            state = Movementstate.air;
        }
    }

    #region movement
    private void MovePlayer()
    {
        moveDirection = (orientation.forward * verticalInput + orientation.right * horizontalInput).normalized;

        var isOnSlope = OnSlope();
        if (isOnSlope && !exitingSlope)
        {
            //moveDirection = GetSlopeMoveDirection();
            rb.AddForce(GetSlopeMoveDirection() * (moveSpeed * 20f), ForceMode.Force);
            if (rb.linearVelocity.y > 0 /*wenn der player nach oben geht*/)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);   //weil gravity aus ist, damit man nicht durchgehend nach oben bounced
                
        }
        else switch (grounded)
        {
            case true:
                rb.AddForce(moveDirection.normalized * (moveSpeed * 10f), ForceMode.Force);
                break;
            case false:
                rb.AddForce(moveDirection.normalized * (moveSpeed * 10f * airMultiplier), ForceMode.Force);
                break;
        }

        rb.useGravity = !isOnSlope;
    }

    private void SpeedControl()
    {
        if (OnSlope() && !exitingSlope) //limit speed on slope
        {
            if (rb.linearVelocity.magnitude > moveSpeed)
                rb.linearVelocity = rb.linearVelocity.normalized * moveSpeed;
        } 
        else //limit speed on ground and in air
        {
            var flatVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

            if (flatVelocity.magnitude <= moveSpeed)
                return;
            var limitedVelocity = flatVelocity.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVelocity.x, rb.linearVelocity.y, limitedVelocity.z);
        }
    }

    private IEnumerator Jump()
    {
        exitingSlope = true;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        
        yield return new WaitForSeconds(jumpCooldown);
        
        exitingSlope = false;
        
        jumpCoroutine = null;
    }

    private bool OnSlope()
    {
        if (!Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f)) 
            return false;
        var angle = Vector3.Angle(Vector3.up, slopeHit.normal);   //slopehit.normal == normale vom boden, auf dem man steht
        return angle != 0 && angle < maxSlopeAngle;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }
    #endregion

    // ReSharper disable Unity.PerformanceAnalysis
    private void Interact()
    {
        var ray = Camera.main.ViewportPointToRay(Vector3.one * 0.5f);
        if (Physics.Raycast(ray, out var hit, interactRange, LayerMask.GetMask("Interactable")))
        {
            var target = hit.collider.gameObject;
            fadenkreuz.sprite = interactable_fadenkreuz;
            fadenkreuz_ist_interactable = true;
            
            if (!interactKey.action.WasPressedThisFrame()) 
                return;
            var action = target.GetComponent<OnInteract>();
            action?.Interact();
        } // reset UI:
        else if (fadenkreuz_ist_interactable) 
        {
            fadenkreuz.sprite = normal_fadenkreuz;
            fadenkreuz_ist_interactable = false;
        }
    }
}
