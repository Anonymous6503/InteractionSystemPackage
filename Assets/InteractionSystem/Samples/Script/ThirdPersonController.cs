using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSmoothTime = 0.1f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    [Header("Camera Settings")]
    public Transform cameraTransform;
    public Vector3 cameraOffset = new Vector3(0f, 2f, -4f);
    public float mouseSensitivity = 150f;
    public float minYAngle = -20f;
    public float maxYAngle = 60f;
    public float cameraDistance = 4f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private float turnSmoothVelocity;
    private float rotationX = 0f;
    private float rotationY = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Auto-assign main camera if not set
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMovement();
        HandleJumpAndGravity();
    }

    void LateUpdate()
    {
        HandleCamera();
    }

    void HandleMovement()
    {
        // Ground check
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        // Input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // Move relative to camera
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
        }
    }

    void HandleJumpAndGravity()
    {
        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        // Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleCamera()
    {
        if (cameraTransform == null) return;

        // Mouse input
        rotationX += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        rotationY -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        rotationY = Mathf.Clamp(rotationY, minYAngle, maxYAngle);

        // Calculate rotation
        Quaternion rotation = Quaternion.Euler(rotationY, rotationX, 0f);
        Vector3 desiredPosition = transform.position + rotation * new Vector3(0f, 0f, -cameraDistance);
        desiredPosition += cameraOffset;

        // Apply position & rotation
        cameraTransform.position = desiredPosition;
        cameraTransform.LookAt(transform.position + Vector3.up * 1.5f);
    }
}
