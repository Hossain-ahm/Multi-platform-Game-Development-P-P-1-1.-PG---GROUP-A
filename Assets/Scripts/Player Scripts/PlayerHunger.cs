using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHunger : MonoBehaviour
{
    public float hunger;
    public float maxHunger;
    public Image hungerBar;
    // Start is called before the first frame update
    void Start()
    {
        maxHunger = hunger;
    }

    // Update is called once per frame
    void Update()
    {
        hungerBar.fillAmount = Mathf.Clamp(hunger/maxHunger, 0, 1);
    }
}
