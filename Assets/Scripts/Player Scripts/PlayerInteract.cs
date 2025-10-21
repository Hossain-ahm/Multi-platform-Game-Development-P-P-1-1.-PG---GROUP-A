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
                if (col.TryGetComponent(out IInteractor interactor))
                {
                    interactor.Interact();
                }
            }
        }
    }

    public IInteractor GetInteractor()
    {
        float interactRange = 2f;
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
        foreach (Collider col in colliderArray)
        {
            if (col.TryGetComponent(out IInteractor interactor))
            {
                return interactor;
            }
        }
        return null;
    }
}
