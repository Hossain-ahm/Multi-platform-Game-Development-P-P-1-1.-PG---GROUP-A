using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempPlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("Optional Armor / Shield")]
    public float damageReduction = 0f; // 0.2 = 20% reduction
    public float shieldHealth = 0f;

    [Header("Debug")]
    public bool invincible = false;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (invincible) return;

        // Apply damage reduction
        damage *= (1f - damageReduction);

        // Shield absorbs first
        if (shieldHealth > 0)
        {
            float absorbed = Mathf.Min(shieldHealth, damage);
            shieldHealth -= absorbed;
            damage -= absorbed;
        }

        if (damage <= 0) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    void Die()
    {
        Debug.Log("Temp player died ????");
        // temporary death handling
    }
}