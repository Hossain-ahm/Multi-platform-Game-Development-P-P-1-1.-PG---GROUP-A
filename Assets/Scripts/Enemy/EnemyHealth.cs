using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float maxHealth = 50f;
    private float currentHealth;
    [SerializeField] string playerPrefsKey;
    [SerializeField] int unlocksRegion;
    public UnityEvent onDeath;

    void Start()
    {
        if (playerPrefsKey.ToString().Length !> 0)
        {
            int defeated = PlayerPrefs.GetInt(playerPrefsKey, -1);
            if (defeated == 1)
            {
                Debug.Log("DEFEATED");
                Destroy(gameObject);
            }
        }

        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        Debug.Log(amount + "ENEMY DAMAGED");
        if (currentHealth <= 0f) return;

        currentHealth -= amount;

        if (currentHealth <= 0f)
        {
            currentHealth = 0f;
            Die();
        }
    }

    public float GetHealthNormalised()
    {
        return currentHealth / maxHealth;
    }
    private void Die()
    {
        if (playerPrefsKey != null || playerPrefsKey.ToString().Length! > 0)
        {
            PlayerPrefs.SetInt(playerPrefsKey, 1);
            PlayerPrefs.Save();
            FindObjectOfType<RegionLockManager>().GetComponent<RegionLockManager>().UnlockRegion(unlocksRegion);
            onDeath.Invoke();
        }
        Destroy(gameObject);
    }
    [ContextMenu("reset boss")]
    public void ResetEnemy()
    {
        PlayerPrefs.SetInt(playerPrefsKey, 0);
    }
}