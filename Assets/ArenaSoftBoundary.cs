using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ArenaSoftBoundary : MonoBehaviour
{
    [Header("Arena Settings")]
    public float arenaRadius = 20f;
    public float pushStrength = 6f;      // How hard the pushback is
    public float slideStrength = 4f;     // Controls sliding along the edge

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb == null) return;

        Vector3 center = transform.position;
        Vector3 playerPos = other.transform.position;

        // Ignore Y so jumping feels normal
        Vector3 flatOffset = new Vector3(
            playerPos.x - center.x,
            0f,
            playerPos.z - center.z
        );

        float distance = flatOffset.magnitude;

        if (distance <= arenaRadius) return;

        // Direction from center to player
        Vector3 outwardDir = flatOffset.normalized;

        // Push inward (opposite direction)
        Vector3 pushDir = -outwardDir;

        // Remove vertical velocity so we don’t mess with jumps
        Vector3 velocity = rb.velocity;
        velocity.y = rb.velocity.y;

        // Cancel outward movement
        float outwardSpeed = Vector3.Dot(velocity, outwardDir);
        if (outwardSpeed > 0f)
        {
            velocity -= outwardDir * outwardSpeed;
        }

        // Apply soft pushback
        velocity += pushDir * pushStrength;

        // Add sliding along the edge
        Vector3 tangent = Vector3.Cross(Vector3.up, outwardDir);
        velocity += tangent * slideStrength;

        rb.velocity = velocity;
    }
}