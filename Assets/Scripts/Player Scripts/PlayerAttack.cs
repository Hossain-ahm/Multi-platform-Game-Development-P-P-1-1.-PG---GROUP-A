using UnityEngine;

namespace Player_Scripts
{
    public class PlayerAttack:MonoBehaviour
    {
        [SerializeField] private PlayerInventory playerInventory;
        [SerializeField] private PlayerMana playerMana;
        [SerializeField] private GameObject sword;
        [SerializeField] private GameObject shield;
        [SerializeField] private GameObject magic_ball;

        private bool isAttacking = false;
        private bool isDefending = false;
        private bool isMagic = false;
        private float counter=0;
        private float attackTime = 1f;
        private float counter2 = 0;
        private float defenseTime = 1f;
        private float counter3 = 0;
        private float magicTime = 1f;
        
        private void Start()
        {
            sword.SetActive(false);
            shield.SetActive(false);
            magic_ball.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && playerInventory.isWeaponEquiped && !isAttacking)
            {
                sword.SetActive(true);
                isAttacking = true;
                counter += Time.deltaTime;
            }
            
            if (Input.GetMouseButtonDown(1) && playerInventory.isshieldEquiped && !isDefending)
            {
                shield.SetActive(true);
                isDefending= true;
                counter2 += Time.deltaTime;
            }

            if (Input.GetKeyDown(KeyCode.R) && !isMagic)
            {
                magic_ball.SetActive(true);
                playerMana.useMana(30);
                isMagic = true;
                counter3 += Time.deltaTime;
            }

            if (isAttacking)
            {
                counter += Time.deltaTime;
            }
            if (isDefending)
            {
                counter2 += Time.deltaTime;
            }
            if (isMagic)
            {
                counter3 += Time.deltaTime;
            }

            if (counter > attackTime)
            {
                counter = 0;
                isAttacking = false;
                sword.SetActive(false);
            }
            if (counter2 > defenseTime)
            {
                counter2 = 0;
                isDefending = false;
                shield.SetActive(false);
            }
            if (counter3 > magicTime)
            {
                counter3 = 0;
                isMagic= false;
                magic_ball.SetActive(false);
            }
        }
        
        
    }
}