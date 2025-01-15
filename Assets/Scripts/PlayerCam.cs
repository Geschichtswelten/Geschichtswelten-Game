using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;

    float xRotation;
    float yRotation;

    [SerializeField] private InputActionReference look;

    private void OnEnable()
    {
        look.action.Enable();
    }

    private void OnDisable()
    {
        look.action.Disable();
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
