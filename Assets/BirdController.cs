using UnityEngine;

public class BirdController : MonoBehaviour
{
    public float flapUpForce = 8f;     // vertical lift from a flap
    public float flapForwardForce = 5f; // extra forward thrust
    public float liftForce = 4f;        // gentle lift when gliding
    public float turnSpeed = 60f;    // degrees per second
    public float bankAngle = 30f;
    private Rigidbody rb;
    bool flapQueued = false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.drag = 0.5f;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            flapQueued = true;
    }

    void FixedUpdate()
    {
        // lift
        rb.AddForce(Vector3.up * liftForce, ForceMode.Acceleration);

        // Forward input (holding W)
        float forwardInput = Input.GetAxis("Vertical");
        if (forwardInput > 0f)
            rb.AddForce(transform.forward * 10f, ForceMode.Acceleration);
        else if (forwardInput == 0f)
            rb.AddForce(transform.forward * 2f, ForceMode.Acceleration);

        if (flapQueued)
        {
            rb.AddForce(Vector3.up * flapUpForce + transform.forward * flapForwardForce, ForceMode.Impulse);
            flapQueued = false;
        }

        // Turning logic 
        float horizontalInput = Input.GetAxis("Horizontal");

        if (Mathf.Abs(horizontalInput) > 0.01f)
        {
            float yawDelta = horizontalInput * turnSpeed * Time.fixedDeltaTime;
            Quaternion yawRotation = Quaternion.Euler(0f, yawDelta, 0f);
            rb.MoveRotation(rb.rotation * yawRotation);
        }

        // visual roll
        float targetBank = -horizontalInput * bankAngle;
        Vector3 euler = rb.rotation.eulerAngles;
        float newZ = Mathf.LerpAngle(euler.z, targetBank, Time.fixedDeltaTime * 3f);
        Quaternion finalRot = Quaternion.Euler(euler.x, euler.y, newZ);
        rb.MoveRotation(finalRot);

        // rotate fwd
        Vector3 currentVel = rb.velocity;
        float vertical = currentVel.y;
        float speed = new Vector3(currentVel.x, 0f, currentVel.z).magnitude;

        Vector3 desiredVel = rb.transform.forward * speed;
        Vector3 newVel = Vector3.Lerp(new Vector3(currentVel.x, 0f, currentVel.z), desiredVel, Time.fixedDeltaTime * 2f);
        rb.velocity = new Vector3(newVel.x, vertical, newVel.z);

    }
}
