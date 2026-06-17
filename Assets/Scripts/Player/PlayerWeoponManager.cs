using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    [SerializeField] private Gun startingGun;
    [SerializeField] private Transform weaponHolder;

    private Gun currentGun;

    void Start()
    {
        EquipGun(startingGun);
    }

    public void EquipGun(Gun newGun)
    {
        if (newGun == null) return;

        currentGun = newGun;

        currentGun.transform.SetParent(weaponHolder);
        currentGun.transform.localPosition = Vector3.zero;
        currentGun.transform.localRotation = Quaternion.identity;
    }

    public Gun GetCurrentGun()
    {
        return currentGun;
    }
}