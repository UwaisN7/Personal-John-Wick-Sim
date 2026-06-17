using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Data", menuName = "Weapons/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("Basic Info")]
    public string weaponName = "New Weapon";

    [Header("Damage")]
    public float damage = 25f;

    [Header("Firing")]
    public float fireRate = 0.25f;
    public float projectileSpeed = 80f;
    public float projectileLifetime = 3f;
    public float spreadAngle = 1f;

    [Header("Prefabs")]
    public GameObject projectilePrefab;

    [Header("Camera Feel")]
    public float cameraShakeForce = 0.5f;
    public float recoilAmount = 1f;
}
