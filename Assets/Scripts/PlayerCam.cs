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

    // Start is called before the first frame update
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

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
        var mouseDelta = look.action.ReadValue<Vector2>();
        var mouseX = mouseDelta.x * Time.deltaTime * sensX;
        var mouseY = mouseDelta.y * Time.deltaTime * sensY;

        yRotation = (yRotation + mouseX) % 360f;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
