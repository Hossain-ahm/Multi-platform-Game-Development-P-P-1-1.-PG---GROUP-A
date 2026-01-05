using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Stats")]
public class EnemyStats : ScriptableObject
{
    public float maxHealth;
    public float damage;
    public float moveSpeed;
    public float attackRange;
    public float attackCooldown;
    public float projectileSpeed;
}
