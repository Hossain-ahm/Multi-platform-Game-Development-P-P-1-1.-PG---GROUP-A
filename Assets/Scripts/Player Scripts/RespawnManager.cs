using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RespawnManager : MonoBehaviour
{
    [SerializeField] List<Transform> respawnPoints = new();
    [SerializeField] Transform birdTransform;
    [SerializeField] Rigidbody birdRB;
    [SerializeField] Animator birdAnim;

    public void Respawn()
    {
        StopAllCoroutines();
        StartCoroutine(RespawnRoutine());
    }
    IEnumerator RespawnRoutine()
    {
        Time.timeScale = 1.0f; // restore normal time immediately
        birdRB.gameObject.GetComponent<BirdController>().blockInput = true;
        FindObjectOfType<PlayerHealth>().alive = true;
        FindObjectOfType<PlayerHealth>().health = 100f;
        birdRB.velocity = Vector3.zero;

        // Use WaitForSecondsRealtime instead
        yield return new WaitForSecondsRealtime(0.1f);

        Transform closestPoint = null;
        float dist = Mathf.Infinity;
        for (int i = 0; i < respawnPoints.Count; i++)
        {
            float newDist = Vector3.Distance(birdTransform.position, respawnPoints[i].position);
            if (newDist < dist)
            {
                dist = newDist;
                closestPoint = respawnPoints[i];
            }
        }

        birdRB.velocity = Vector3.zero;
        birdTransform.position = closestPoint.position;

        yield return new WaitForSecondsRealtime(0.1f);

        birdRB.velocity = Vector3.zero;
        birdTransform.position = closestPoint.position;
        birdAnim.SetTrigger("idle");

        EventSystem.current.SetSelectedGameObject(null);
        birdRB.gameObject.GetComponent<BirdController>().blockInput = false;
    }

}
