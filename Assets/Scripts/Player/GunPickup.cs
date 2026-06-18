using UnityEngine;
//This script will be for picking up and dropping guns. It will check for nearby guns and allow the player to pick them up or drop their current gun.
//Guns are their own existing objects wuirh a Gun script attached to them. The player will have a PlayerWeaponManager script that will handle equipping and dropping guns.
//This script is literally pickup and drop
public class PlayerWeaponInteractor : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerWeaponManager weaponManager;

    [Header("Pickup")]
    [SerializeField] private float pickupRadius = 2f;
    [SerializeField] private LayerMask weaponLayer;

    private PlayerControls playerControls;
    private Gun nearbyGun;

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
        nearbyGun = FindNearestGun();

        if (playerControls.Gameplay.GunPickup.WasPressedThisFrame())
        {
            HandlePickUpOrDrop();
        }
    }

    void HandlePickUpOrDrop()
    {
        if (weaponManager == null) return;

        if (nearbyGun != null)
        {
            weaponManager.EquipGun(nearbyGun); 
            nearbyGun = null;
            return;
        }

        if (weaponManager.HasGun())
        {
            weaponManager.DropCurrentGun();
        }
    }

    Gun FindNearestGun()
    {
        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            pickupRadius,
            weaponLayer,
            QueryTriggerInteraction.Ignore
        );

        Gun closestGun = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider hit in hits)
        {
            Gun gun = hit.GetComponentInParent<Gun>();

            if (gun == null) continue;
            if (gun.IsHeld()) continue;

            float distance = Vector3.Distance(transform.position, gun.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestGun = gun;
            }
        }

        return closestGun;
    }

    public Gun GetNearbyGun()
    {
        return nearbyGun;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }
}