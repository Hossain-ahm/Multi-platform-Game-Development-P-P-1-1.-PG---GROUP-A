using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private float maxHealth;
    [SerializeField] private Image healthBar;
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = health;   
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = Mathf.Clamp(health/maxHealth, 0, 1);
    }
}
