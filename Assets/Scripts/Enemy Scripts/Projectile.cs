using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float damage;
    float speed;

    public void Init(float dmg, float spd)
    {
        damage = dmg;
        speed = spd;
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<TempPlayerHealth>()?.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}