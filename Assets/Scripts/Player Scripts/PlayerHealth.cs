using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] public float health;
    [SerializeField] private float maxHealth;
    [SerializeField] private Image healthBar;
    [SerializeField] private Animator damageFlash;
    [SerializeField] private PlayerHunger playerHunger;
    public bool alive { get; set; }
    public bool infDamage { get; set; }
    [SerializeField] private float damageAmount = 5f; // damage per tick
    [SerializeField] private float damageInterval = 1f; // seconds between damage

    private Coroutine damageCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        alive = true;
        maxHealth = health;
    }
    void Update()
    {
        healthBar.fillAmount = Mathf.Clamp(health / maxHealth, 0, 1);
        if (health <= 0 && alive)
        {
            alive = false;
            health = 0;
            GetComponent<BirdController>().Die();

            if (damageCoroutine != null)
                StopCoroutine(damageCoroutine);
            infDamage = false;
        }
        if (playerHunger.GetHunger() <= 0)
        {
            health -= 3;
        }
    }
    public void StartInfDamage()
    {
        // Make sure infDamage is true when starting
        infDamage = true;

        if (damageCoroutine == null)
            damageCoroutine = StartCoroutine(InfDamageCoroutine());
        else
        {
            StopCoroutine(damageCoroutine);
            damageCoroutine = StartCoroutine(InfDamageCoroutine());
        }
    }


    // Call this to stop infinite damage
    public void StopInfDamage()
    {
        if (damageCoroutine != null)
        {
            infDamage = false;
            StopCoroutine(damageCoroutine);
            damageCoroutine = null;
        }
    }

    private IEnumerator InfDamageCoroutine()
    {
        while (infDamage && alive)
        {
            TakeDamage(damageAmount);
            yield return new WaitForSeconds(damageInterval);
        }
        damageCoroutine = null; // reset coroutine reference
    }

    public void TakeDamage(float amount)
    {
        Debug.Log("DAMAGED FOR " + amount);
        damageFlash.SetTrigger("damage");
        health -= amount;
        if (health < 0) health = 0;
    }

}
