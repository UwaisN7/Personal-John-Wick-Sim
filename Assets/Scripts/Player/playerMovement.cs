using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float rotationSpeed = 10f;
    public float aimRotationSpeed = 25f;

    private CharacterController cc;
    private PlayerControls playerControls;
    private Transform cameraTransform;
    private PlayerAim playerAim;

    public Vector3 moveDirection;

    void Awake()
    {
        playerControls = new PlayerControls();
        cc = GetComponent<CharacterController>();
        playerAim = FindAnyObjectByType<PlayerAim>();
    }

    void Start()
    {
        if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
        else
        {
            Debug.LogError("No Main Camera found. Make sure your actual camera is tagged MainCamera.");
        }
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    void Update()
    {

        Cursor.lockState = CursorLockMode.Locked;   
        OnMove();
        RotatePlayer();
    }

    void OnMove()
    {
        if (cameraTransform == null) return;

        Vector2 movementInput = playerControls.Gameplay.Move.ReadValue<Vector2>();
        Vector3 move = new Vector3(movementInput.x, 0f, movementInput.y);

        if (move.magnitude > 0.1f)
        {
            Vector3 cameraForward = cameraTransform.forward;
            cameraForward.y = 0f;
            cameraForward.Normalize();

            Vector3 cameraRight = cameraTransform.right;
            cameraRight.y = 0f;
            cameraRight.Normalize();

            moveDirection = (cameraForward * move.z + cameraRight * move.x).normalized;

            cc.Move(moveDirection * movementSpeed * Time.deltaTime);
        }
        else
        {
            moveDirection = Vector3.zero;
        }
    }

    void RotatePlayer()
    {
        if (cameraTransform == null) return;

        bool aiming = playerAim != null && playerAim.IsAiming();

        if (aiming)
        {
            Vector3 cameraForward = cameraTransform.forward;
            cameraForward.y = 0f;

            if (cameraForward.sqrMagnitude < 0.001f) return;

            cameraForward.Normalize();

            Quaternion targetRotation = Quaternion.LookRotation(cameraForward);

            // Snappier aim rotation
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                aimRotationSpeed * Time.deltaTime
            );
        }
        else
        {
            if (moveDirection.sqrMagnitude < 0.001f) return;

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }
}