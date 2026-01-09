using UnityEngine;

public class PlayerAttackProjectile : MonoBehaviour
{
    [Header("Fire Point")]
    [SerializeField] private Transform firePoint;

    [Header("Projectiles")]
    [SerializeField] private GameObject fireProjectile;
    [SerializeField] private GameObject frostProjectile;
    [SerializeField] private float manaCost;
    [SerializeField] private PlayerMana manaBar;
    [SerializeField] string frostKey, fireKey;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && PlayerPrefs.GetInt(fireKey, -1) == 1)
        {
            Shoot(fireProjectile);
        }

        if (Input.GetKeyDown(KeyCode.E) && PlayerPrefs.GetInt(frostKey, -1) == 1)
        {
            Shoot(frostProjectile);
        }
    }

    private void Shoot(GameObject projectilePrefab)
    {
        if (projectilePrefab == null || firePoint == null) return;
        if (manaBar.GetMana() < manaCost)
        {
            return;
        }
        manaBar.useMana(manaCost);
        GameObject proj = Instantiate(
            projectilePrefab,
            firePoint.position,
            firePoint.rotation
        );

        PlayerProjectile projectile = proj.GetComponent<PlayerProjectile>();
        if (projectile != null)
        {
            projectile.Init(firePoint.forward);
        }
    }
}
