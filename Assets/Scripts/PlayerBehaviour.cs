using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Mono.CSharp;
using TMPro;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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
    [Header("Pause Menu")]
    [SerializeField] private PlayerPauseMenu pauseMenu;

    [Header("Keybinds")]
    [SerializeField] private InputActionReference movement;
    [SerializeField] private InputActionReference jump;
    [SerializeField] private InputActionReference sprintKey;
    [SerializeField] private InputActionReference crouchKey;
    [SerializeField] private InputActionReference interactKey;
    [SerializeField] private InputActionReference action1Key;
    [SerializeField] private InputActionReference action2Key;
    [SerializeField] private InputActionReference pauseMenuKey;
    [SerializeField] private InputActionReference consoleKey;
    [SerializeField] private InputActionReference escKey;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    [Header("UI_stuff")]
    public Image fadenkreuz;
    public Sprite normal_fadenkreuz;
    public Sprite interactable_fadenkreuz;
    private bool fadenkreuz_ist_interactable;
    [SerializeField] private Canvas HUD_Canvas;
    [SerializeField] private TMP_Text hp_label;
    [SerializeField] private TMP_Text equipped_item_name;

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
    
    private ushort _equippedItemId;

    [SerializeField] private Transform itemAnker;
    [SerializeField] private GameObject equippedItem;
    [SerializeField] private GameObject fists;
    private ItemBehaviour _equippedItemBehaviour;
    
    public bool _inventoryOpen;
    [SerializeField] private Inventory inventory;
    [SerializeField] private Inventory equipment;
    [SerializeField] private Inventory crafting;
    [SerializeField] private Inventory hotbar;
    public int activeHotbarSlot = 0;
    [SerializeField] public GameObject storageInv;

    private StorageInventory _lastContainer = null;
    
    private const float MaxHp = 100f;
    private float _hp = MaxHp;
    private List<Armor> _armor = new List<Armor>();

    [SerializeField] private GameObject drop;
    
    [SerializeField] private Transform respawnPoint;
    
    public bool _consoleOpen = false;

    [SerializeField] private float walkCooldown;
    [SerializeField] private float sprintCooldown;
    [SerializeField] private Animator animator;

    
    private float _wC = 0, _sC = 0;

    private bool _takeDamage = true;

    [Header("Audio")] 
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioSource _combatSource;
    [SerializeField] private AudioClip[] woodClips;
    [SerializeField] private AudioClip[] grassClips;
    [SerializeField] private AudioClip[] gruntClips;
    [SerializeField] private AudioClip[] deathClips;
    [SerializeField] private AudioClip eatClip;
    

    #endregion

    private void OnEnable()
    {
        movement.action.Enable();
        jump.action.Enable();
        sprintKey.action.Enable();
        crouchKey.action.Enable();
        interactKey.action.Enable();
        action1Key.action.Enable();
        action2Key.action.Enable();
        pauseMenuKey.action.Enable();
        escKey.action.Enable();
        ButtonHandler.OnSettingsChanged += HandleVolumeChange;
    }

    private void OnDisable()
    {
        movement.action.Disable();
        jump.action.Disable();
        sprintKey.action.Disable();
        crouchKey.action.Disable();
        interactKey.action.Disable();
        action1Key.action.Disable();
        action2Key.action.Disable();
        pauseMenuKey.action.Disable();
        escKey.action.Disable();
        ButtonHandler.OnSettingsChanged -= HandleVolumeChange;
    }
    // Start is called before the first frame update
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        _combatSource.volume = ButtonHandler.settings.dialogueVolume;
        _source.volume = ButtonHandler.settings.masterVolume;
        
        startPosition = transform.position;

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        jumpCoroutine = null;
        UnityEngine.Physics.gravity = new Vector3(0, -18.62f, 0);

        startYScale = transform.localScale.y;
        
        EquipItem(0);
        
        _inventoryOpen = Inventory.inventoryOpen;
        var dmgMul = 1f;
        _armor.ForEach(x => dmgMul *= x.Multiplier);
        hp_label.text = _hp.ToString() + " / 100\t" + (1f - dmgMul).ToString() + "%";
    }

    private void Update()
    {
        _sC -= Time.deltaTime;
        _wC -= Time.deltaTime;
        if (_inventoryOpen != Inventory.inventoryOpen)
        {
            if (_lastContainer != null) _lastContainer.closeInventory();
            toggleInventory();
        }

        if (consoleKey.action.WasPressedThisFrame())
        {
            _consoleOpen = !_consoleOpen;
            pauseMenu.tooglePauseMenu();
        }
        
        var targetRotation = orientation.rotation;
        transform.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
        
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

    private void HandleVolumeChange()
    {
            _source.volume = ButtonHandler.settings.masterVolume;
            _combatSource.volume = ButtonHandler.settings.dialogueVolume;
    }

    public void resetPosition()
    {
        transform.position = startPosition;
    }

    private void toggleInventory()
    {
        _inventoryOpen = Inventory.inventoryOpen;
        if (_inventoryOpen)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            HUD_Canvas.gameObject.SetActive(false);
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            HUD_Canvas.gameObject.SetActive(true);
        }
    }

    public void OpenInventory()
    {
        if (_inventoryOpen) 
            return;
        inventory.openInventory();
        equipment.openInventory();
        crafting.openInventory();
        toggleInventory();
    }
    public void OpenInventory(StorageInventory container)
    {
        if (_inventoryOpen) 
            return;
        inventory.openInventory();
        equipment.openInventory();
        crafting.openInventory();
        _lastContainer = container;
        toggleInventory();
    }

    public void CloseInventory()
    {
        if (!_inventoryOpen)
            return;
        inventory.closeInventory();
        equipment.closeInventory();
        crafting.closeInventory();
        if (_lastContainer != null) 
            _lastContainer.closeInventory();
        toggleInventory();
    }

    private void HandleInput()
    {
        var pauseMenuOpened = pauseMenuKey.action.WasPressedThisFrame() || escKey.action.WasPressedThisFrame();
        switch (pauseMenuOpened)
        {
            case true when _inventoryOpen:
                CloseInventory();
                break;
            case true when !_consoleOpen:
                pauseMenu.tooglePauseMenu();
                break;
            case false when _inventoryOpen || pauseMenu.isPaused || _consoleOpen:
                return;
        }
        var action1Input = action1Key.action.WasPressedThisFrame();
        var action2Input = action2Key.action.WasPressedThisFrame();
        if (action1Input)
        {
            _equippedItemBehaviour.Action1();
        }
        else if (action2Input)
        {
            _equippedItemBehaviour.Action2();
        }


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
        if (crouchKey.action.IsPressed())
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
        if (_inventoryOpen || freeze)
        {
            verticalInput = 0;
            horizontalInput = 0;
        }

       
        
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

        if (Physics.Raycast(gameObject.transform.position, Vector3.down, out RaycastHit hit, 4f,
                LayerMask.GetMask("whatIsGround")))
        {
            if (grounded && moveDirection.magnitude != 0)
            {
                switch (state)
                {
                    case Movementstate.walking:
                        if (_wC <= 0)
                        {
                            if (hit.collider.gameObject.CompareTag("Wood"))
                            {
                                _source.clip = woodClips[Random.Range(0, woodClips.Length)];
                                _source.Play();
                            }
                            else
                            {
                                _source.clip = grassClips[Random.Range(0, grassClips.Length)];
                                _source.Play();
                            }
                            _wC = walkCooldown;
                        }
                        break;
                    case Movementstate.sprinting:
                        if (_sC <= 0)
                        {
                            
                                if (hit.collider.gameObject.CompareTag("Wood"))
                                {
                                    _source.clip = woodClips[Random.Range(0, woodClips.Length)];
                                    _source.Play();
                                    //Debug.Log("played sprint clip");
                                }
                                else
                                {
                                    _source.clip = grassClips[Random.Range(0, grassClips.Length)];
                                    _source.Play();
                                    //Debug.Log("played sprint clip");
                                }
                                _sC = sprintCooldown;
                            
                        }
                        break;
                }
            }
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
        return false;
        /*
        if (!Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
            return false;
        var angle = Vector3.Angle(Vector3.up, slopeHit.normal);   //slopehit.normal == normale vom boden, auf dem man steht
        return angle != 0 && angle < maxSlopeAngle;*/
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }
    #endregion

    // ReSharper disable Unity.PerformanceAnalysis
    public void Interact()
    {
        var ray = Camera.main.ViewportPointToRay(Vector3.one * 0.5f);
        if (Physics.Raycast(ray, out var hit, interactRange, LayerMask.GetMask("Interactable")))
        {
            var target = hit.collider.gameObject;
            fadenkreuz.sprite = interactable_fadenkreuz;
            fadenkreuz_ist_interactable = true;

            if (!interactKey.action.WasPressedThisFrame() && 
                ((_equippedItemId != 0 && _equippedItemBehaviour.GetType() != typeof(FoodBehaviour)
                                       && _equippedItemBehaviour.GetType() != typeof(dumbItemBehaviour)) 
                 || !action2Key.action.WasPressedThisFrame())) return;
            var action = target.GetComponent<OnInteract>();
            action?.Interact();
        } // reset UI:
        else if (fadenkreuz_ist_interactable) 
        {
            fadenkreuz.sprite = normal_fadenkreuz;
            fadenkreuz_ist_interactable = false;
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void EquipItem(ushort id)
    {
        //Debug.Log("equipped item " + id);
        Destroy(itemAnker.GetChild(0).gameObject);
        
        var item = inventory.GetItemFromId(id).itemModel;
        if (item == null) 
            item = fists;
        equippedItem = Instantiate(item, itemAnker.transform, false);
        equippedItem.layer = gameObject.layer;
        if (equippedItem.TryGetComponent<PickUpItem>(out var pickUpScript))
        {
            pickUpScript.enabled = false;
            Destroy(equippedItem.GetComponent<Rigidbody>());
        }
        _equippedItemBehaviour = equippedItem.GetComponent<ItemBehaviour>();

        switch (id)
        {
            case 1:
                _equippedItemBehaviour.Id = 1;
                _equippedItemBehaviour.damage = 10;
                break;
            case 2:
                _equippedItemBehaviour.Id = 2;
                _equippedItemBehaviour.damage = 5;
                break;
            case 7:
                _equippedItemBehaviour.Id = 7;
                _equippedItemBehaviour.damage = 17;
                break;
            case 25:
                _equippedItemBehaviour.Id = 25;
                _equippedItemBehaviour.damage = 5;
                break;
        }
        _equippedItemId = _equippedItemBehaviour.Id;
        equipped_item_name.text = item.name;
        //possibly do some inventory logic here
    }
    
    public void EquipItem(ushort id, int slot)
    {
        EquipItem(id);
        if (slot is >= 0 and < 5)
            activeHotbarSlot = slot;
    }

    
    public void LoadPosition(Vector3 position, Quaternion rotation)
    {
        //Debug.Log("Loaded Position");
        transform.position = new Vector3(position.x, position.y + 3f, position.z);
        transform.rotation = rotation;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Hit by " + other.name);
        if (other.TryGetComponent<AbstractEnemyBehaviour>(out var enemy))
        {
            TakeDamage(enemy._damage);
        }   
        else if (other.TryGetComponent<ArrowScript>(out var arrowScript)) {
            TakeDamage(arrowScript.archer._damage);
        }
        else if (other.CompareTag("killbox")) {
            Die();
        }
        else
        {
            var e = other.transform.root.gameObject;
            enemy = e.GetComponentInChildren<AbstractEnemyBehaviour>();
            TakeDamage(enemy == null ? 0 : enemy._damage);
        }
    }

    public void Eat(float val)
    {
        _combatSource.clip = eatClip;
        _combatSource.Play();
        Heal(val);
    }

    public void Heal(float val)
    {
        if (val <= 0) 
            return;
        _hp += val;
        if (_hp > MaxHp) _hp = MaxHp;
        var dmgMul = 1f;
        _armor.ForEach(x => dmgMul *= x.Multiplier);
        var res = $"{(1f - dmgMul) * 100f,6:##0.0}";
        hp_label.text = (int) _hp + " / 100\t" + res + "%";
    }

    public struct Armor
    {
        public int ItemId;
        // in range of [0; 1] for reduction
        public float Multiplier;
    }

    public void RegisterArmor(Armor a)
    {
        //Debug.Log("Registering armor [" + a.ItemId + "]");
        _armor.Add(a);
        var dmgMul = 1f;
        _armor.ForEach(x => dmgMul *= x.Multiplier);
        var res = $"{(1f - dmgMul) * 100f,6:##0.0}";
        hp_label.text = (int) _hp + " / 100\t" + res + "%";
    }
    
    public void RemoveArmor(int id)
    {
        //Debug.Log("Removing armor [" + id + "]");
        _armor.RemoveAll(x => x.ItemId == id);
        var dmgMul = 1f;
        _armor.ForEach(x => dmgMul *= x.Multiplier);
        var res = $"{(1f - dmgMul) * 100f,6:##0.0}";
        hp_label.text = (int) _hp + " / 100\t" + res + "%";
    }

    public void TakeDamage(float val)
    {
        if (!_takeDamage) return;
        
        if (!_combatSource.isPlaying)
        {
            _combatSource.clip = gruntClips[Random.Range(0, gruntClips.Length)];
            _combatSource.Play();
        }
        if (val <= 0) 
            return;
        var dmgMul = 1f;
        _armor.ForEach(x => dmgMul *= x.Multiplier);
        //Debug.Log("Incoming dmg " + val + ", after armor " + val*dmgMul);
        _hp -= val * dmgMul;
        if (_hp <= 0.05)
        {
            Die();
            _hp = 0;
        }
        var res = $"{(1f - dmgMul) * 100f,6:##0.0}";
        hp_label.text = (int) _hp + " / 100\t" + res + "%";
    }

    private void Die()
    {
        _takeDamage = false;
        _combatSource.clip = deathClips[Random.Range(0, deathClips.Length)];
        _combatSource.Play();
        var pouchInv = Instantiate(drop, transform.position, Quaternion.identity).GetComponent<StorageInventory>();
        pouchInv.player = gameObject;
        pouchInv.inventory = storageInv;
        pouchInv.inv = pouchInv.inventory.GetComponent<Inventory>();
        inventory.ItemsInInventory.ForEach(x => pouchInv.addItemToStorage(x.itemID, x.itemValue));
        inventory.deleteAllItems();
        Heal(float.MaxValue);
        StartCoroutine(Respawn());
    }
    
    public IEnumerator Respawn()
    {
        animator.SetTrigger("FadeTrigger");
        yield return new WaitForSecondsRealtime(4.4f);
        var dmgMul = 1f;
        _armor.ForEach(x => dmgMul *= x.Multiplier);
        _hp = MaxHp;
        var res = $"{(1f - dmgMul) * 100f,6:##0.0}";
        hp_label.text = (int) _hp + " / 100\t" + res + "%";
        if (respawnPoint != null)
            LoadPosition(respawnPoint.position, respawnPoint.rotation);

        _takeDamage = true;
        animator.SetTrigger("FadeTrigger");
    }

    public Inventory GetHotbar() => hotbar;
}
