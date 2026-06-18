using UnityEngine;


//Stuff to add , Rolling A . Dodging B, Reload X If extra ammo, Player can only have 2 mags at a time so if player has a shotgun and runs over shotugun shotgun will give amount of bullets left in gun 
//Crouch RS,Slide if crouch while running, ehehhehehehe 


public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float movementSpeed = 5f;
    public float sprintSpeed = 8f;
    public float rotationSpeed = 10f;
    public float aimRotationSpeed = 25f;

    [Header("Gravity")]
    public float gravity = -25f;
    public float groundedGravity = -2f;

    private float verticalVelocity;

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
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
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
        OnMove();
        ApplyGravity();
        RotatePlayer();
    }

    void OnMove()
    {
        if (cameraTransform == null) return;

        Vector2 movementInput = playerControls.Gameplay.Move.ReadValue<Vector2>();
        Vector3 move = new Vector3(movementInput.x, 0f, movementInput.y);

        bool isAiming = playerAim != null && playerAim.IsAiming();

        // Change Sprint to whatever your input action is called.
        bool sprintInput = playerControls.Gameplay.Sprint.ReadValue<float>() > 0.1f;

        bool canSprint = sprintInput && !isAiming && move.magnitude > 0.1f;

        float currentSpeed = canSprint ? sprintSpeed : movementSpeed;

        if (move.magnitude > 0.1f)
        {
            Vector3 cameraForward = cameraTransform.forward;
            cameraForward.y = 0f;
            cameraForward.Normalize();

            Vector3 cameraRight = cameraTransform.right;
            cameraRight.y = 0f;
            cameraRight.Normalize();

            moveDirection = (cameraForward * move.z + cameraRight * move.x).normalized;

            cc.Move(moveDirection * currentSpeed * Time.deltaTime);
        }
        else
        {
            moveDirection = Vector3.zero;
        }
    }

    void ApplyGravity()
    {
        if (cc.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = groundedGravity;
        }

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 gravityMove = new Vector3(0f, verticalVelocity, 0f);

        cc.Move(gravityMove * Time.deltaTime);
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