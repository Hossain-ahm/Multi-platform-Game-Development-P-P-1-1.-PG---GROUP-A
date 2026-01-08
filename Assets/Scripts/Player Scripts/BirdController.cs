using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

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

    [Header("Bird Audio")]
    [SerializeField] private AudioSource flapSource, diveSource;
    [SerializeField] private AudioClip[] flapClips;
    [SerializeField] private AudioClip[] diveClips;
    bool diveAudioPlaying = false;
    [SerializeField] private float fadeDuration = 0.5f;

    public bool blockInput { get; set; }
    enum BirdState
    {
        Idle,
        Hover,
        Walk,
        Dive,
        DiveUp
    }

    BirdState currentState;

    bool stopping, grounded;
    [SerializeField] Animator birdAnimator;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] ParticleSystem stoppingPS;
    [SerializeField] float groundDeceleration = 10f;
    [SerializeField] Slider staminaBar;
    [SerializeField] float passiveStamGain = 0.01f, lauchStamDecay = 0.1f, flapStamDecay = 0.25f;

    private Rigidbody rb;
    bool flapQueued = false, diveUp = false, isBoosting = false, hasDove = false;
    private bool walking;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.drag = 0.5f;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && staminaBar.value >= flapStamDecay && !blockInput)
        {
            grounded = false;
            stopping = false;
            flapQueued = true;
        }
    }

    void FixedUpdate()
    {
        //constant fwd and hover force
        if (!grounded && !stopping)
        {
            rb.AddForce(transform.forward * 15f, ForceMode.Acceleration);
            rb.AddForce(Vector3.up * (hoverLiftForce), ForceMode.Acceleration);
        }
        float forwardInput = Input.GetAxis("Vertical");
        bool isDiving = (!grounded && forwardInput > 0f) || (diveUp && isBoosting);
        if (isDiving && !diveAudioPlaying)
        {
            if (diveSource != null && diveClips.Length > 0)
                PlayRandomClip(diveClips, diveSource);
            diveAudioPlaying = true;
        }
        else if (!isDiving)
        {
            diveAudioPlaying = false;
            if (diveSource != null)
                diveSource.Stop();
        }

        if (forwardInput > 0f && !blockInput)
        {
            if (!grounded && staminaBar.value >= lauchStamDecay)
            {
                rb.AddForce(transform.forward * 10f, ForceMode.Acceleration);
                rb.AddForce(Vector3.down * (hoverLiftForce), ForceMode.Acceleration);
                SetState(BirdState.Dive);
                staminaBar.value -= lauchStamDecay;
                hasDove = true;
            }
            else
            {
                if (grounded && !stopping)
                {
                    Vector3 velocity = rb.velocity;

                    float walkSpeed = 3f;

                    Vector3 targetVelocity = transform.forward * walkSpeed;

                    rb.velocity = new Vector3(
                        targetVelocity.x,
                        velocity.y,
                        targetVelocity.z
                    );
                    SetState(BirdState.Walk);
                    staminaBar.value += passiveStamGain;
                }
            }
        }
        else if (forwardInput == 0f && !blockInput)
        {
            staminaBar.value += passiveStamGain;

            if (grounded && !stopping)
                SetState(BirdState.Idle);
            else if (!grounded)
                SetState(BirdState.Hover);
        }
        //DIVING UP ON KEY PRESSED
        if (Input.GetKey(KeyCode.S) && !blockInput)
        {
            if (!isBoosting && !diveUp && hasDove && !stopping && !grounded)
            {
                diveUp = true;
                speedFactor = Mathf.Clamp(rb.velocity.magnitude, 0f, 100f);
                boostDuration = Mathf.Min(speedFactor / 10f, 4f);
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
        if (diveUp && isBoosting && !stopping && !grounded && staminaBar.value >= lauchStamDecay)
        {
            staminaBar.value -= lauchStamDecay;
            StopCancelled();
            boostTimer += Time.fixedDeltaTime;
            float lift = defaultLiftForce * (speedFactor / 10f);
            rb.AddForce(Vector3.up * lift, ForceMode.Acceleration);
            rb.AddForce(transform.forward * speedFactor / 25f, ForceMode.Acceleration);
            SetState(BirdState.DiveUp);


            if (boostTimer >= boostDuration)
            {
                SetState(BirdState.Hover);
                isBoosting = false;
                diveUp = false;
            }
        }


        //FLAP WINGS
        if (flapQueued)
        {
            flapQueued = false;
            staminaBar.value -= flapStamDecay;
            rb.AddForce(Vector3.up * flapUpForce + transform.forward * flapForwardForce, ForceMode.Impulse);
            if (birdAnimator != null)
                birdAnimator.SetTrigger("flap");
            StopCancelled();
            if (flapSource != null && flapClips.Length > 0)
                PlayRandomClip(flapClips, flapSource);
        }

        //TURNING LOGIC
        float horizontal = Input.GetAxis("Horizontal") * (blockInput ? 0 : 1);

        float targetHRoll = -horizontal * (!grounded ? bankAngle : bankAngle / 2);

        Vector3 currentEuler = transform.localEulerAngles;
        if (currentEuler.x > 180) currentEuler.x -= 360;
        if (currentEuler.y > 180) currentEuler.y -= 360;
        if (currentEuler.z > 180) currentEuler.z -= 360;

        float newHRoll = Mathf.Lerp(currentEuler.z, targetHRoll, Time.fixedDeltaTime * 3f);

        float yawTurnSpeed = Mathf.Abs(newHRoll / bankAngle) * turnSpeed;
        float yawDirection = -Mathf.Sign(newHRoll);
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

        //STOPPING FUNCTIONS
        if (stopping)
        {
            Vector3 velocity = rb.velocity;

            // Only slow horizontal movement
            Vector3 horizontalVel = new Vector3(velocity.x, 0f, velocity.z);

            horizontalVel = Vector3.MoveTowards(
                horizontalVel,
                Vector3.zero,
                groundDeceleration * Time.fixedDeltaTime
            );

            rb.velocity = new Vector3(horizontalVel.x, velocity.y, horizontalVel.z);
            if (rb.velocity.magnitude <= 0.2f)
            {
                stopping = false;
                birdAnimator.SetBool("stopping", false);
                birdAnimator.SetTrigger("land");
                stoppingPS.Stop();
                grounded = true;
            }
        }
    }
    void SetState(BirdState newState)
    {
        if (currentState == newState) return;

        currentState = newState;

        switch (newState)
        {
            case BirdState.Idle:
                birdAnimator.SetTrigger("idle");
                break;

            case BirdState.Hover:
                birdAnimator.SetTrigger("hover");
                break;

            case BirdState.Walk:
                birdAnimator.SetTrigger("walking");
                break;
            case BirdState.Dive:
                birdAnimator.SetTrigger("diving");
                break;
            case BirdState.DiveUp:
                birdAnimator.SetTrigger("divingUp");
                break;
        }
    }
    public void SmiteDown()
    {
        rb.AddForce(Vector3.down * flapUpForce * 4, ForceMode.Impulse);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 6 && !stopping && !grounded)
        {
            birdAnimator.SetBool("stopping", true);
            stoppingPS.Play();
            stopping = true;
        }
    }

    void StopCancelled()
    {
        birdAnimator.SetBool("stopping", false);
        stoppingPS.Stop();
        stopping = false;
    }
    public void Die()
    {
        birdAnimator.SetTrigger("diving");
        Time.timeScale = 0.1f;
        blockInput = true;
        FindObjectOfType<MenuManager>().DeathScreen();
    }
    private void PlayRandomClip(AudioClip[] clips, AudioSource src)
    {
        if (clips == null || clips.Length == 0 || src == null) return;

        int index = Random.Range(0, clips.Length);
        src.PlayOneShot(clips[index]);
    }
}
