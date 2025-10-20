using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHunger : MonoBehaviour
{
    public float hunger;
    public float maxHunger;
	public float hungerDecrease; // amount the hunger goes down
	public float hungerRate; // rate of the hunger decreases
	public float hungerTime; 
    public Image hungerBar;
    // Start is called before the first frame update
    void Start()
    {
        maxHunger = hunger;
		hungerTime = hungerRate;
    }

    // Update is called once per frame
    void Update()
    {
        hungerBar.fillAmount = Mathf.Clamp(hunger/maxHunger, 0, 1);
		hungerTime -= Time.deltaTime;
		if (!(hungerTime <= 0)) return;
		hungerTime = hungerRate;
		hunger -= hungerDecrease;
    }
}
