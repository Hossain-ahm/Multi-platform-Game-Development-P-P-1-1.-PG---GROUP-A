using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            float interactRange = 2f;
            Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
            foreach (Collider col in colliderArray)
            {
                if (col.TryGetComponent(out FoodInteractor foodInteractor))
                {
                    foodInteractor.Interact();
                }
            }
        }
    }

    public bool GetfoodInteractor()
    {
        float interactRange = 2f;
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
        foreach (Collider col in colliderArray)
        {
            if (col.TryGetComponent(out FoodInteractor foodInteractor))
            {
                return true;
            }
        }
        return false;
    }
}
