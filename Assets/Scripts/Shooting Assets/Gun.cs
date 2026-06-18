using Unity.Cinemachine;
using UnityEngine;

public class Gun : MonoBehaviour


    //Generic Gun script data is held with scribtable object, this is basically the script that builds the guns 
{
    [Header("Weapon Data")]
    [SerializeField] private WeaponData weaponData;

    [Header("Gun Setup")]
    [SerializeField] private Transform firePoint;

    [Header("Cinemachine")]
    [SerializeField] private CinemachineImpulseSource impulseSource;

    [Header("Pickup / Drop")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float despawnTime = 20f;
    [SerializeField] private string heldLayerName = "IgnoreRaycast";
    [SerializeField] private string droppedLayerName = "Weapon";

    private Collider[] colliders;
    private float nextFireTime;
    private bool isHeld;
    private float despawnTimer;
    private bool despawnTimerRunning;

    void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }

        colliders = GetComponentsInChildren<Collider>();
    }

    void Update()
    {
        if (despawnTimerRunning)
        {
            despawnTimer -= Time.deltaTime;

            if (despawnTimer <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetHeld(Transform weaponHolder)
    {
        isHeld = true;
        despawnTimerRunning = false;

        transform.SetParent(weaponHolder,false);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.detectCollisions = false;
        }

        SetCollidersEnabled(false);
        SetLayerRecursively(gameObject, LayerMask.NameToLayer(heldLayerName));

        Debug.Log("Gun held: " + gameObject.name);
    }

    public void SetDropped(Vector3 dropPosition, Quaternion dropRotation, Vector3 dropForce)
    {
        isHeld = false;

        transform.SetParent(null);
        transform.position = dropPosition;
        transform.rotation = dropRotation;

        SetLayerRecursively(gameObject, LayerMask.NameToLayer(droppedLayerName));
        SetCollidersEnabled(true);

        if (rb != null)
        {
            rb.detectCollisions = true;
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.AddForce(dropForce, ForceMode.Impulse);
        }

        despawnTimer = despawnTime;
        despawnTimerRunning = true;

        Debug.Log("Gun dropped: " + gameObject.name);


        //This will be the area where guns become projectiles and instead of killing they are a direct stun and insta takedown kill 
    }

    void SetCollidersEnabled(bool enabled)
    {
        foreach (Collider col in colliders)
        {
            col.enabled = enabled;
        }
    }

    void SetLayerRecursively(GameObject obj, int layer)
    {
        if (layer < 0) return;

        obj.layer = layer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }

    public bool IsHeld()
    {
        return isHeld;
    }

    public void FireAtPoint(Vector3 targetPoint)
    {
        if (firePoint == null)
        {
            Debug.LogWarning("No firePoint assigned.");
            return;
        }

        Vector3 direction = targetPoint - firePoint.position;

        if (direction.sqrMagnitude < 0.01f)
        {
            direction = firePoint.forward;
        }

        Fire(direction.normalized);
    }

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

        if (weaponData.projectilePrefab == null)
        {
            Debug.LogWarning("No projectile prefab assigned.");
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
    //Hmmm debating on spread mayhaps 
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