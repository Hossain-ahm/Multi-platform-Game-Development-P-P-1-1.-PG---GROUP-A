using System.Collections;
using System.Collections.Generic;
using Inventory_Scripts;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMana : MonoBehaviour
{
    [SerializeField] private float mana;
    [SerializeField] private float maxMana;
    [SerializeField] private Image manaBar;
    [SerializeField] private float manaRegen;
    // Start is called before the first frame update
    void Start()
    {
        maxMana = mana;
    }

    // Update is called once per frame
    void Update()
    {

        manaBar.fillAmount = Mathf.Clamp(mana / maxMana, 0, 1);
        if (mana < maxMana)
        {
            mana += manaRegen;
        }
    }
    public float GetMana()
    {
        return mana;
    }
    public void useMana(float amount)
    {
        mana -= amount;
    }
}