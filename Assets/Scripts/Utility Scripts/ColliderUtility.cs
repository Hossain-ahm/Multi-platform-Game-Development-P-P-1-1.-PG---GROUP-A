using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColliderUtility : MonoBehaviour
{
    [SerializeField] UnityEvent onTriggerEnter, onTriggerExit, onCollisionEnter, onCollisionExit;
    private void OnTriggerEnter(Collider other)
    {
        onTriggerEnter.Invoke();
    }
    private void OnTriggerExit(Collider other)
    {
        onTriggerExit.Invoke();
    }
    private void OnCollisionEnter(Collision collision)
    {
        onCollisionEnter.Invoke();
    }
    private void OnCollisionExit(Collision collision)
    {    
        onCollisionExit.Invoke();
    }
}
