using UnityEngine;
using UnityEngine.Rendering;

public class BirdController : MonoBehaviour
{
    public float flapUpForce = 8f;
    public float flapForwardForce = 5f;
    public float defaultLiftForce = 4f;
    public float hoverLiftForce = 6f;
    public float turnSpeed = 60f;
    public float bankAngle = 30f;
    private float speedFactor = 0f;
    float boostDuration, boostTimer;
    [SerializeField] float rotationSpeed = 2f;
    [SerializeField] float pitchAmount = 30f;
    [SerializeField] float rollAmount = 45f;
    [SerializeField] Animator birdAnimator;

    private Rigidbody rb;
    bool flapQueued = false, diveUp = false, isBoosting = false, hasDove = false;
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
        //constant fwd and hover force
        rb.AddForce(transform.forward * 5f, ForceMode.Acceleration);
        rb.AddForce(Vector3.up * (hoverLiftForce), ForceMode.Acceleration);

        float forwardInput = Input.GetAxis("Vertical");
        if (forwardInput > 0f)
        {
            rb.AddForce(transform.forward * 10f, ForceMode.Acceleration);
            rb.AddForce(Vector3.down * (hoverLiftForce / 2), ForceMode.Acceleration);
            hasDove = true;
        }

        //DIVING UP ON KEY PRESSED
        if (Input.GetKey(KeyCode.S))
        {
            if (!isBoosting && !diveUp && hasDove)
            {
                diveUp = true;
                speedFactor = Mathf.Clamp(rb.velocity.magnitude, 0f, 100f);
                boostDuration = Mathf.Min(speedFactor / 10f, 4f);
                Debug.Log("UP" + boostDuration + " " + speedFactor);
                boostTimer = 0f;
                isBoosting = true; hasDove = false;
            }
        }
        else
        {
            diveUp = false;
            isBoosting = false;
            boostTimer = 0f;
        }
        //ACTUAL DIVEUP FORCES APPLIED
        if (diveUp && isBoosting)
        {
            boostTimer += Time.fixedDeltaTime;
            float lift = defaultLiftForce * (speedFactor / 5f);
            rb.AddForce(Vector3.up * lift, ForceMode.Acceleration);
            rb.AddForce(transform.forward * speedFactor / 25f, ForceMode.Acceleration);
            if (boostTimer >= boostDuration)
            {
                isBoosting = false;
                diveUp = false;
            }
        }

        //FLAP WINGS
        if (flapQueued)
        {
            rb.AddForce(Vector3.up * flapUpForce + transform.forward * flapForwardForce, ForceMode.Impulse);
            if (birdAnimator != null)
                birdAnimator.SetTrigger("flap");
            flapQueued = false;
        }

        //TURNING LOGIC
        float horizontal = Input.GetAxis("Horizontal");

        float targetHRoll = -horizontal * bankAngle;
        float targetVRoll = -horizontal * bankAngle;

        Vector3 currentEuler = transform.localEulerAngles;
        if (currentEuler.x > 180) currentEuler.x -= 360;
        if (currentEuler.y > 180) currentEuler.y -= 360;
        if (currentEuler.z > 180) currentEuler.z -= 360;

        float newHRoll = Mathf.Lerp(currentEuler.z, targetHRoll, Time.fixedDeltaTime * 3f);
        float newVRoll = Mathf.Lerp(currentEuler.x, targetVRoll, Time.fixedDeltaTime * 3f);

        float yawTurnSpeed = Mathf.Abs(newHRoll / bankAngle) * turnSpeed;
        float pitchTurnSpeed = Mathf.Abs(newVRoll / bankAngle) * turnSpeed;
        float yawDirection = -Mathf.Sign(newHRoll);
        float pitchDirection = -Mathf.Sign(newVRoll);
        float newYaw = currentEuler.y + yawDirection * yawTurnSpeed * Time.fixedDeltaTime;

        Quaternion targetRot = Quaternion.Euler(currentEuler.x, newYaw, newHRoll);
        transform.rotation = targetRot;

        //CHANGE VELOCITY TO MATCH ROTATION
        Vector3 currentVel = rb.velocity;
        float verticalVel = currentVel.y;
        float speed = new Vector3(currentVel.x, 0f, currentVel.z).magnitude;

        Vector3 desiredVel = transform.forward * speed;
        rb.velocity = Vector3.Lerp(rb.velocity, desiredVel, Time.fixedDeltaTime * 2f);
        rb.velocity = new Vector3(rb.velocity.x, verticalVel, rb.velocity.z);
    }



}
