using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PlayerProjectile : MonoBehaviour
{
    /* ---------- CONFIG ---------- */
    [Header("Stats")]
    public float speed = 20f;
    public float damage = 10f;
    public float lifeTime = 5f;

    /* ---------- PRIVATE ---------- */
    Vector3 direction;

    /* ---------- INIT ---------- */

    public void Init(Vector3 dir)
    {
        direction = dir.normalized;
        Destroy(gameObject, lifeTime);
    }

    /* ---------- MOVE ---------- */

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    /* ---------- HIT ---------- */

    void OnTriggerEnter(Collider other)
    {
        EnemyHealth enemy = other.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
