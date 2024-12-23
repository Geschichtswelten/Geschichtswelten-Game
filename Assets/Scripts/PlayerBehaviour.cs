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
    bool readyToJump;

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
    void Start()
    {
        startPosition = transform.position;

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;

        startYScale = transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        MyInput();
        SpeedControl();
        StateHandler();
        Interact();

        //handle drag
        if (grounded)
        {
            rb.linearDamping = groundDrag;
        }
        else
        {
            rb.linearDamping = 0f;
        }
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

    private void MyInput()
    {
        horizontalInput = movement.action.ReadValue<Vector2>().x;
        verticalInput = movement.action.ReadValue<Vector2>().y;

        bool playerJumped = jump.action.IsPressed();
        if (playerJumped && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);    //um durchgehend zu springen, wenn man gedr�ckt h�lt
        }

        //start crouch
        if (crouchKey.action.WasPressedThisFrame())
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);  //um den schwebenden player schnell wieder auf den boden zu dr�cken
        }

        //stop crouch
        if (crouchKey.action.WasReleasedThisFrame())
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    private void StateHandler()
    {
        if (freeze)
        {
            state = Movementstate.freeze;
            moveSpeed = 0;
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
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);
            if (rb.linearVelocity.y > 0)                                  //wenn der player nach oben geht
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);   //weil gravity aus ist, damit man nicht durchgehen nach oben bounced
        }

        else if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        //limiting speed on slope
        if (OnSlope() && !exitingSlope)
        {
            if (rb.linearVelocity.magnitude > moveSpeed)
                rb.linearVelocity = rb.linearVelocity.normalized * moveSpeed;
        }

        //limiting speed on ground and in air
        else
        {
            Vector3 flatVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

            if (flatVelocity.magnitude > moveSpeed)
            {
                Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
                rb.linearVelocity = new Vector3(limitedVelocity.x, rb.linearVelocity.y, limitedVelocity.z);
            }
        }
    }

    private void Jump()
    {
        exitingSlope = true;

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false;
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);   //slopehit.normal == normale vom boden, auf dem man steht
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }
    #endregion

    private void Interact()
    {
        GameObject target;
        var ray = Camera.main.ViewportPointToRay(Vector3.one * 0.5f);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, interactRange, LayerMask.GetMask("Interactable")))
        {
            target = hit.collider.gameObject;
            fadenkreuz.sprite = interactable_fadenkreuz;
            fadenkreuz_ist_interactable = true;
            if(interactKey.action.WasReleasedThisFrame()) 
            {
                OnInteract doShit = target.GetComponent<OnInteract>();
                if (doShit != null)
                    doShit.Interact();
            }
        }
        else if (fadenkreuz_ist_interactable)
        {
            fadenkreuz.sprite = normal_fadenkreuz;
            fadenkreuz_ist_interactable = false;
        }
    }
}
