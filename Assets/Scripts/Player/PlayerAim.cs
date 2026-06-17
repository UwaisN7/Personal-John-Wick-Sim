using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{
    private CinemachineCamera vCam;
    private CinemachineOrbitalFollow orbitalFollow;

    [Header("Aim Settings")]
    public float aimFov = 40f;
    public float originalFov = 60f;
    public float aimSensMultiplierX = 0.5f;
    public float aimSensMultiplierY = 0.5f;
    public float aimMoveSpeedMultiplier = 0.6f;

    [Header("Shoulder Offsets")]
    private bool isRightShoulder = true;

    [SerializeField] private float rightShoulderOffset = 0.7f;
    [SerializeField] private float leftShoulderOffset = -0.7f;
    
    [SerializeField] private Vector3 centerOffset = new Vector3(0f, 1.5f, 0f);

    [Header("Smoothing")]
    [SerializeField] private float offsetLerpSpeed = 12f;
    [SerializeField] private float fovLerpSpeed = 10f;

    private Vector3 targetOffset;

    private PlayerControls playerControls;
    private PlayerMovement playerMovement;
    private PlayerCam playerCam;

    private bool isAiming = false;

    private float originalMoveSpeed;
    private float originalSensitivityX;
    private float originalSensitivityY;

    [Header("Cam target")]
    [SerializeField] private CameraTarget cameraTargetFollow;

    [SerializeField] private WeaponSideSwitcher weaponSideSwitcher;

    private Vector3 originalPositionDamping;
    private Vector3 originalRotationDamping;
    private float originalQuaternionDamping;

    void Awake()
    {
        playerControls = new PlayerControls();

        playerMovement = FindAnyObjectByType<PlayerMovement>();
        playerCam = FindAnyObjectByType<PlayerCam>();
        vCam = FindAnyObjectByType<CinemachineCamera>();

        if (vCam != null)
        {
            orbitalFollow = vCam.GetComponent<CinemachineOrbitalFollow>();
            originalFov = vCam.Lens.FieldOfView;
        }

        if (playerMovement != null)
        {
            originalMoveSpeed = playerMovement.movementSpeed;
        }

        if (playerCam != null)
        {
            originalSensitivityX = playerCam.multiplierX;
            originalSensitivityY = playerCam.multiplierY;
        }

        targetOffset = centerOffset;

        if (cameraTargetFollow == null)
        {
            cameraTargetFollow = FindAnyObjectByType<CameraTarget>();
        }

        if (orbitalFollow != null)
        {
            var settings = orbitalFollow.TrackerSettings;

            originalPositionDamping = settings.PositionDamping;
            originalRotationDamping = settings.RotationDamping;
            originalQuaternionDamping = settings.QuaternionDamping;
        }
        if (weaponSideSwitcher == null)
        {
            weaponSideSwitcher = FindAnyObjectByType<WeaponSideSwitcher>();
        }
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

        if (isAiming && playerControls.Gameplay.SwapShoulder.WasPressedThisFrame())
        {
            isRightShoulder = !isRightShoulder;
            UpdateShoulderOffset();
        }

        UpdateCameraFov();
        UpdateCameraOffset();
    }

    void EnterAimMode()
    {
        isAiming = true;

        if (playerMovement != null)
        {
            playerMovement.movementSpeed = originalMoveSpeed * aimMoveSpeedMultiplier;
        }

        if (playerCam != null)
        {
           
            playerCam.multiplierX = originalSensitivityX * aimSensMultiplierX;
            playerCam.multiplierY = originalSensitivityY * aimSensMultiplierY;
            playerCam.UpdateGainMultiplier();
        }
        UpdateShoulderOffset();
        SetTargetShoulderOffset();
        SetAimDamping(true);

    }

    void ExitAimMode()
    {
        isAiming = false;

        if (playerMovement != null)
        {
            playerMovement.movementSpeed = originalMoveSpeed;
        }

        if (playerCam != null)
        {
            playerCam.multiplierX = originalSensitivityX;
            playerCam.multiplierY = originalSensitivityY;
            playerCam.UpdateGainMultiplier();
        }

        targetOffset = centerOffset;
        UpdateShoulderOffset();
        SetTargetShoulderOffset();
        SetAimDamping(false);
    }

    void UpdateShoulderOffset()
    {
        if (cameraTargetFollow == null) return;

        if (!isAiming)
        {
            cameraTargetFollow.SetSideOffset(0f);
            return;
        }

        float targetOffset = isRightShoulder ? rightShoulderOffset : leftShoulderOffset;
        cameraTargetFollow.SetSideOffset(targetOffset);

        if (weaponSideSwitcher != null)
        {
            weaponSideSwitcher.SetRightSide(isRightShoulder);
        }

    }
    void UpdateCameraFov()
    {
        if (vCam == null) return;

        float targetFov = isAiming ? aimFov : originalFov;

        vCam.Lens.FieldOfView = Mathf.Lerp(
            vCam.Lens.FieldOfView,
            targetFov,
            Time.deltaTime * fovLerpSpeed
        );
    }
    void SetTargetShoulderOffset()
    {
        if (cameraTargetFollow == null) return;

        if (!isAiming)
        {
            cameraTargetFollow.SetSideOffset(0f);
            return;
        }

        float sideOffset = isRightShoulder ? rightShoulderOffset : leftShoulderOffset;
        cameraTargetFollow.SetSideOffset(sideOffset);
    }
    void UpdateCameraOffset()
    {
        if (orbitalFollow == null) return;

        orbitalFollow.TargetOffset = Vector3.Lerp(
            orbitalFollow.TargetOffset,
            targetOffset,
            Time.deltaTime * offsetLerpSpeed
        );
    }
    void SetAimDamping(bool aiming)
    {
        if (orbitalFollow == null) return;

        var settings = orbitalFollow.TrackerSettings;

        if (aiming)
        {
            settings.PositionDamping = Vector3.zero;
            settings.RotationDamping = Vector3.zero;
            settings.QuaternionDamping = 0f;
        }
        else
        {
            settings.PositionDamping = originalPositionDamping;
            settings.RotationDamping = originalRotationDamping;
            settings.QuaternionDamping = originalQuaternionDamping;
        }

        orbitalFollow.TrackerSettings = settings;
    }
    public bool IsAiming()
    {
        return isAiming;
    }

    public bool IsRightShoulder()
    {
        return isRightShoulder;
    }
}