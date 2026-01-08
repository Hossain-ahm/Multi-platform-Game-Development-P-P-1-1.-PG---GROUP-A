using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordDamage : MonoBehaviour
{
    private int swordDmg;
    [SerializeField] private PlayerInventory playerInventory;
    void Start()
    {
        swordDmg = 0;
    }

    // Update is called once per frame
    void Update()
    {
        swordDmg = playerInventory.GetswordDmag();
    }
}
