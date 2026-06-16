using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;  
public class PlayerAim : MonoBehaviour
{
    private CinemachineCamera vCam;
    private Transform playerTransform;

    [Header("Aim Settings")]
    public float aimFov ;
    public float originalFov;
    public float aimSensMultiplierX;
    public float aimSensMultiplierY;
    public float aimMoveSpeedMultiplier;

    [Header("Shoulder is which which")]
     private bool isRightShoulder = true;
    [SerializeField] private Vector3 rightShoulderOffset = new Vector3(1.5f, 1.5f, -2f);  
    [SerializeField] private Vector3 leftShoulderOffset = new Vector3(-1.5f, 1.5f, -2f);   
    [SerializeField] private Vector3 centerOffset = new Vector3(0, 1.5f, -3f);             


    private PlayerControls playerControls;
    private CinemachineOrbitalFollow thirdPersonCamera;
    private PlayerMovement playerMovement;
    private PlayerCam playerCam;    
    private bool isAiming = false;
    private float originalMoveSpeed;
    private float originalSensitivityX;
    private float originalSensitivityY;

    void Awake()
    {
        playerControls = new PlayerControls();
        playerMovement = FindAnyObjectByType<PlayerMovement>();
        playerCam = FindAnyObjectByType<PlayerCam>();  
        vCam= FindAnyObjectByType<CinemachineCamera>();
        thirdPersonCamera = vCam.GetComponent<CinemachineOrbitalFollow>();
        originalMoveSpeed = playerMovement.movementSpeed;
        originalSensitivityX = playerCam.multiplierX;
        originalSensitivityY = playerCam.multiplierY;
        
    }

    void OnEnable()
    {
        playerControls.Enable();
    }

    void OnDisable()
    {
        playerControls.Disable();
    }
    void Update()
    {
        bool aimInput = playerControls.Gameplay.Aim.ReadValue<float>() > 0.1f;

        if (aimInput && !isAiming)
        {
            EnterAimMode();
        }
        else if (!aimInput && isAiming)
        {
            ExitAimMode();
        }


        bool shoulderSwap = playerControls.Gameplay.SwapShoulder.ReadValue<float>() > 0.1f;

        if (isAiming)
        {
          

            if (shoulderSwap)
            {
                // Toggle shoulder
                isRightShoulder = !isRightShoulder;
                UpdateShoulderOffset();
                Debug.Log($"Swapped to {(isRightShoulder ? "Right" : "Left")} shoulder");
            }
        }
        float targetFOV = isAiming ? aimFov : originalFov;
        vCam.Lens.FieldOfView = Mathf.Lerp(vCam.Lens.FieldOfView, targetFOV, Time.deltaTime * 10f);

    }


    void EnterAimMode()
    {
        isAiming = true;
        playerMovement.movementSpeed = originalMoveSpeed * aimMoveSpeedMultiplier;
    
        playerCam.multiplierX = originalSensitivityX -   aimSensMultiplierX;
        playerCam.multiplierY = originalSensitivityY - aimSensMultiplierY;
       
        playerCam.UpdateGainMultiplier();
        UpdateShoulderOffset();
    }

    void ExitAimMode()
    {
        isAiming = false;
        playerMovement.movementSpeed = originalMoveSpeed;
        playerCam.multiplierX = originalSensitivityX;
        playerCam.multiplierY = originalSensitivityY;
        
        playerCam.UpdateGainMultiplier();
        UpdateShoulderOffset();
    }


    void UpdateShoulderOffset()
    {

       

        // Set target offset based on shoulder
        if (isRightShoulder)
        {
            centerOffset = rightShoulderOffset;
           
        }
        else
        {
            centerOffset = leftShoulderOffset;
         
        }
    }

    // Public methods for external access
    public bool IsAiming() => isAiming;
    public bool IsRightShoulder() => isRightShoulder;
}


