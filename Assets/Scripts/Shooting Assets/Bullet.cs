using UnityEngine;


// This is the projectile script, when it spawns
//It will move in the direction it was shot and destroy itself after a certain amount of time
public class Bullet : MonoBehaviour
{
    private Vector3 direction;
    private float speed;
    private float damage;

    public void LaunchBullet(Vector3 shootDirection, float projectileSpeed, float projectileDamage, float lifeTime)
    {
        direction = shootDirection.normalized;
        speed = projectileSpeed;
        damage = projectileDamage;
        // Destroy the bullet after a certain amount of time to prevent memory leaks
        Destroy(gameObject, lifeTime); 
    }
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
