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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Shoot(fireProjectile);
        }

        if (Input.GetKeyDown(KeyCode.E))
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
