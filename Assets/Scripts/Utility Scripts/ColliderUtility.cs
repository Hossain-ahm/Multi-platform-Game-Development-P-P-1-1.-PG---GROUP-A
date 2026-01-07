using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColliderUtility : MonoBehaviour
{
    [SerializeField] UnityEvent onTriggerEnter, onTriggerExit, onCollisionEnter, onCollisionExit;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.CompareTag("Player"))
            onTriggerEnter.Invoke();
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            onTriggerExit.Invoke();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            onCollisionEnter.Invoke();
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            onCollisionExit.Invoke();
    }
}
