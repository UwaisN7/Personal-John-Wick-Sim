using Unity.Cinemachine;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Weapon Data")]
    [SerializeField] private WeaponData weaponData;

    [Header("Gun Setup")]
    [SerializeField] private Transform firePoint;

    [Header("Cinemachine")]
    [SerializeField] private CinemachineImpulseSource impulseSource;

    private float nextFireTime;

    public void Fire(Vector3 direction)
    {
        if (weaponData == null)
        {
            Debug.LogWarning("No WeaponData assigned to gun.");
            return;
        }

        if (firePoint == null)
        {
            Debug.LogWarning("No firePoint assigned to gun.");
            return;
        }

        if (Time.time < nextFireTime)
        {
            return;
        }

        nextFireTime = Time.time + weaponData.fireRate;

        Vector3 finalDirection = ApplySpread(direction, weaponData.spreadAngle);

        GameObject bulletObject = Instantiate(
            weaponData.projectilePrefab,
            firePoint.position,
            Quaternion.LookRotation(finalDirection)
        );

        Bullet projectile = bulletObject.GetComponent<Bullet>();

        if (projectile != null)
        {
            projectile.LaunchBullet(
                finalDirection,
                weaponData.projectileSpeed,
                weaponData.damage,
                weaponData.projectileLifetime
            );
        }

        if (impulseSource != null)
        {
            impulseSource.GenerateImpulse(weaponData.cameraShakeForce);
        }
    }

    private Vector3 ApplySpread(Vector3 direction, float spreadAngle)
    {
        if (spreadAngle <= 0f)
        {
            return direction.normalized;
        }

        float randomX = Random.Range(-spreadAngle, spreadAngle);
        float randomY = Random.Range(-spreadAngle, spreadAngle);

        Quaternion spreadRotation = Quaternion.Euler(randomY, randomX, 0f);

        return spreadRotation * direction.normalized;
    }
}