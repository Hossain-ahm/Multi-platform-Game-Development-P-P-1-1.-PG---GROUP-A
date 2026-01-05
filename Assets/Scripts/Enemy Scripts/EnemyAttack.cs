using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public EnemyStats stats;
    public GameObject projectilePrefab;
    public Transform firePoint;

    float cooldownTimer;

    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer <= 0)
        {
            Shoot();
            cooldownTimer = stats.attackCooldown;
        }
    }

    void Shoot()
    {
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        proj.GetComponent<Projectile>().Init(stats.damage, stats.projectileSpeed);
    }
}