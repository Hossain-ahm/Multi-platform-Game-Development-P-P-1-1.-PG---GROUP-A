using UnityEngine;

namespace Player_Scripts
{
    public class PlayerAttack:MonoBehaviour
    {
        [SerializeField] private PlayerInventory playerInventory;
        [SerializeField] private GameObject sword;

        private bool isAttacking = false;
        private float counter=0;
        private float attackTime = 1f;
        private void Start()
        {
            sword.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && playerInventory.isWeaponEquiped && !isAttacking)
            {
                sword.SetActive(true);
                isAttacking = true;
                counter += Time.deltaTime;
            }

            if (isAttacking)
            {
                counter += Time.deltaTime;
            }

            if (counter > attackTime)
            {
                counter = 0;
                isAttacking = false;
                sword.SetActive(false);
            }
        }
        
    }
}