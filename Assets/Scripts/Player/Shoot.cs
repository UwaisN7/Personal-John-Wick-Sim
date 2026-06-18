using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerWeaponManager weaponManager;
    [SerializeField] private Camera mainCamera;

    [Header("Aiming")]
    [SerializeField] private float aimDistance = 100f;
    [SerializeField] private LayerMask aimLayers = ~0;

    private PlayerControls playerControls;

    void Awake()
    {
        playerControls = new PlayerControls();

        if (weaponManager == null)
        {
            weaponManager = GetComponent<PlayerWeaponManager>();
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
        bool shootInput = playerControls.Gameplay.Shoot.ReadValue<float>() > 0.1f;

        if (shootInput)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (weaponManager == null) return;

        Gun currentGun = weaponManager.GetCurrentGun();

        if (currentGun == null)
        {
            Debug.LogWarning("No current gun equipped.");
            return;
        }

        Vector3 aimPoint = GetAimPoint();

        currentGun.FireAtPoint(aimPoint);
    }

    Vector3 GetAimPoint()
    {
        if (mainCamera == null)
        {
            return transform.position + transform.forward * aimDistance;
        }

        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out RaycastHit hit, aimDistance, aimLayers))
        {
            return hit.point;
        }

        return ray.origin + ray.direction * aimDistance;
    }
}