using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Projectile : MonoBehaviour
{
    /* ---------- CONFIG ---------- */
    [Header("Projectile")]
    public float speed = 25f;
    public float lifeTime = 5f;
    public float damage = 10f;

    /* ---------- PRIVATE ---------- */
    Vector3 direction;

    /* ---------- INITIALIZATION ---------- */

    public void Init(Vector3 dir)
    {
        direction = dir.normalized;
        Destroy(gameObject, lifeTime);
    }

    /* ---------- MOVEMENT ---------- */

    void FixedUpdate()
    {
        transform.position += direction * speed * Time.fixedDeltaTime;
    }

    /* ---------- COLLISION ---------- */

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerHealth health = other.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}