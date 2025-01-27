using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCam : MonoBehaviour
{
    private float sensX;
    private float sensY;

    public Transform orientation;

    float xRotation;
    float yRotation;

    [SerializeField] private InputActionReference look;

    private void Start()
    {
        sensX = ButtonHandler.settings.mouseSensitivity;
        sensY = sensX;
    }

    private void OnEnable()
    {
        ButtonHandler.OnSettingsChanged += HandleSettingsChanged;
        look.action.Enable();
    }

    private void OnDisable()
    {
        ButtonHandler.OnSettingsChanged -= HandleSettingsChanged;
        look.action.Disable();
    }

    private void HandleSettingsChanged()
    {
        sensX = ButtonHandler.settings.mouseSensitivity;
        sensY = sensX;
    }

    private void Update()
    {
        if (Inventory.inventoryOpen) 
            return;
        
        var mouseDelta = look.action.ReadValue<Vector2>();
        var mouseX = mouseDelta.x * Time.deltaTime * (sensX / 3);
        var mouseY = mouseDelta.y * Time.deltaTime * (sensY / 3);

        yRotation = (yRotation + mouseX) % 360f;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
