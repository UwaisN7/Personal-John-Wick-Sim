using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private Gun currentGun;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private PlayerWeaponManager weaponManager;
    private PlayerControls playerControls;

    void Awake()
    {
        playerControls = new PlayerControls();


        if (weaponManager == null)
        {
            weaponManager = GetComponent<PlayerWeaponManager>();
        }

        if (cameraTransform == null && Camera.main != null)
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
        bool shootInput = playerControls.Gameplay.Shoot.ReadValue<float>() > 0.1f;

        if (shootInput)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (weaponManager == null) return;
        if (cameraTransform == null) return;

        Gun currentGun = weaponManager.GetCurrentGun();

        if (currentGun == null) return;

        currentGun.Fire(cameraTransform.forward);
    }
}