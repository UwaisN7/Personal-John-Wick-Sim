using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private float health = 100f;

    public void TakeDamage(float damage)
    {
        health -= damage;

        Debug.Log($"{gameObject.name} took {damage} damage. Health left: {health}");

        if (health <= 0f)
        {
            Destroy(gameObject);
        }
    }
}