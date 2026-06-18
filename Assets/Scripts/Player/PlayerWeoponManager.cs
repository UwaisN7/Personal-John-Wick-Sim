using UnityEngine;


public class PlayerWeaponManager : MonoBehaviour
{
    [Header("Weapon Holding")]
    [SerializeField] private Transform weaponHolder;

    [Header("Dropping")]
    [SerializeField] private Transform dropPoint;
    [SerializeField] private float dropForwardForce = 2f;
    [SerializeField] private float dropUpForce = 1f;

    private Gun currentGun;

    public void EquipGun(Gun gun)
    {
        if (gun == null)
        {
            Debug.LogWarning("Tried to equip null gun.");
            return;
        }

        if (weaponHolder == null)
        {
            Debug.LogWarning("No weapon holder assigned.");
            return;
        }

        if (currentGun != null)
        {
            DropCurrentGun();
        }

        currentGun = gun;
        currentGun.SetHeld(weaponHolder);

        Debug.Log("Equipped gun: " + currentGun.name);
    }

    public void DropCurrentGun()
    {
        if (currentGun == null) return;

        Vector3 dropPosition;
        Quaternion dropRotation;

        if (dropPoint != null)
        {
            dropPosition = dropPoint.position;
            dropRotation = dropPoint.rotation;
        }
        else
        {
            dropPosition = transform.position + transform.forward * 1f + Vector3.up * 1f;
            dropRotation = transform.rotation;
        }

        Vector3 dropForce = transform.forward * dropForwardForce + Vector3.up * dropUpForce;

        currentGun.SetDropped(dropPosition, dropRotation, dropForce);
        currentGun = null;
    }

    //Throw Gun logic here called from the shoot script because is Aiming is called there
    public Gun GetCurrentGun()
    {
        return currentGun;
    }

    public bool HasGun()
    {
        return currentGun != null;
    }

}