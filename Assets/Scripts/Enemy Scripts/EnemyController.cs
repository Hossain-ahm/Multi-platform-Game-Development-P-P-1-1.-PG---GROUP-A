using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public EnemyStats stats;
    public Transform player;

    void Update()
    {
        if (!player) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > stats.attackRange)
        {
            MoveTowardsPlayer();
        }
        else
        {
            FacePlayer();
        }
    }

    void MoveTowardsPlayer()
    {
        Vector3 dir = (player.position - transform.position).normalized;
        transform.position += dir * stats.moveSpeed * Time.deltaTime;
    }

    void FacePlayer()
    {
        Vector3 lookDir = player.position - transform.position;
        lookDir.y = 0;
        transform.rotation = Quaternion.LookRotation(lookDir);
    }
}
