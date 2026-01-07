using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    /* ---------- CONFIG ---------- */
    [Header("Health")]
    [SerializeField] float maxHealth = 100f;
    [SerializeField] float currentHealth;

    [Header("UI")]
    [SerializeField] Image healthBar;

    /* ---------- INITIALIZATION ---------- */

    void Start()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        UpdateUI();
    }

    /* ---------- DAMAGE ---------- */

    public void TakeDamage(float amount)
    {
        if (currentHealth <= 0f) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        UpdateUI();

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    /* ---------- UI ---------- */

    void UpdateUI()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = currentHealth / maxHealth;
        }
    }

    /* ---------- DEATH ---------- */

    void Die()
    {
        Debug.Log("Player died");
        // Future: disable controls, play animation, trigger game over
    }

}
