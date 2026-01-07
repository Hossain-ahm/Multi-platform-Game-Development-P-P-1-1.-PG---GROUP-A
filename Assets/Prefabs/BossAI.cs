using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    [Header("Idle Hover")]
    public float hoverAmplitude = 1f;
    public float hoverSpeed = 1f;
    [Header("Chase & Attack")]
    public float chaseSpeed = 10f;          // Speed while chasing player
    public float attackRange = 20f;         // Distance to start shooting
    public GameObject projectilePrefab;     // Prefab for boss projectile
    public Transform firePoint;             // Assign 'BossFirePoint' here
    public float fireCooldown = 2f;         // Time between shots

    private Vector3 originPosition;
    private bool playerInArena = false;
    private Transform player;
    private float fireTimer = 0f;

    void Start()
    {
        originPosition = transform.position;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (!playerInArena)
        {
            IdleHover();
            if (playerInArena)
            {
                ChaseAndAttack();
            }
        }
    }

    void IdleHover()
    {
        Vector3 newPos = originPosition;
        newPos.y += Mathf.Sin(Time.time * hoverSpeed) * hoverAmplitude;
        transform.position = newPos;
    }

    // Called from BossArena
    public void PlayerEnteredArena()
    {
        playerInArena = true;
        Debug.Log("Player entered arena: Boss activated");
    }

    public void PlayerExitedArena()
    {
        playerInArena = false;
        Debug.Log("Player exited arena: Boss returning to idle");
    }

    void ChaseAndAttack()
    {
        if (player == null) return;

        // Move toward player in 3D
        Vector3 direction = player.position - transform.position;
        transform.position += direction.normalized * chaseSpeed * Time.deltaTime;

        // Face player
        if (direction.sqrMagnitude > 0f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                5f * Time.deltaTime
            );
        }

        // Shooting
        fireTimer += Time.deltaTime;
        if (direction.magnitude <= attackRange && fireTimer >= fireCooldown)
        {
            FireProjectile();
            fireTimer = 0f;
        }
    }

    void FireProjectile()
    {
        if (projectilePrefab == null || firePoint == null) return;

        Vector3 shootDirection = (player.position - firePoint.position).normalized;

        GameObject proj = Instantiate(
            projectilePrefab,
            firePoint.position,
            Quaternion.LookRotation(shootDirection)
        );

        // Assuming your projectile script has Init(Vector3 direction)
        PlayerProjectile projectile = proj.GetComponent<PlayerProjectile>();
        if (projectile != null)
        {
            projectile.Init(shootDirection);
        }
    }


}
