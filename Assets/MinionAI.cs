using UnityEngine;
using UnityEngine.AI;

public class MinionAI : MonoBehaviour
{
    /* ---------- PATROL ---------- */
    [Header("Patrol")]
    public Transform patrolA;
    public Transform patrolB;
    public float patrolSpeed = 3f;
    public float returnPointTolerance = 1.2f;

    /* ---------- DETECTION ---------- */
    [Header("Detection")]
    public float detectionRange = 20f;
    public float disengageRange = 35f;

    /* ---------- COMBAT ---------- */
    [Header("Combat")]
    public float chaseSpeed = 6f;
    public float attackRange = 10f;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireCooldown = 1.2f;
    public float firstShotDelay = 0.4f;

    /* ---------- PRIVATE ---------- */
    NavMeshAgent agent;
    Transform player;

    Vector3 homePosition;
    Transform currentPatrolTarget;

    float fireTimer;
    bool firstShotFired;
    bool returnDestinationSet;

    enum State { Patrol, Chase, Return }
    State currentState;

    /* ---------- UNITY ---------- */

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        homePosition = transform.position;
        currentPatrolTarget = patrolA;

        currentState = State.Patrol;
        agent.speed = patrolSpeed;
        agent.SetDestination(currentPatrolTarget.position);
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case State.Patrol:
                HandlePatrol(distanceToPlayer);
                break;

            case State.Chase:
                HandleChase(distanceToPlayer);
                break;

            case State.Return:
                HandleReturn();
                break;
        }
    }

    /* ---------- STATES ---------- */

    void HandlePatrol(float distanceToPlayer)
    {
        agent.speed = patrolSpeed;
        agent.isStopped = false;

        Patrol();

        if (distanceToPlayer <= detectionRange)
            currentState = State.Chase;
    }

    void HandleChase(float distanceToPlayer)
    {
        agent.speed = chaseSpeed;

        if (distanceToPlayer <= attackRange)
        {
            agent.isStopped = true;
            FacePlayer();
            HandleShooting();
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
            ResetFire();
        }

        if (distanceToPlayer > disengageRange)
        {
            ResetFire();
            returnDestinationSet = false;
            currentState = State.Return;
        }
    }

    void HandleReturn()
    {
        agent.speed = patrolSpeed;
        agent.isStopped = false;

        if (!returnDestinationSet)
        {
            agent.SetDestination(homePosition);
            returnDestinationSet = true;
        }

        if (!agent.pathPending && agent.remainingDistance <= returnPointTolerance)
        {
            currentPatrolTarget = patrolA;
            agent.SetDestination(currentPatrolTarget.position);
            currentState = State.Patrol;
        }
    }

    /* ---------- BEHAVIOURS ---------- */

    void Patrol()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            currentPatrolTarget = currentPatrolTarget == patrolA ? patrolB : patrolA;
            agent.SetDestination(currentPatrolTarget.position);
        }
    }

    void FacePlayer()
    {
        Vector3 dir = player.position - transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.01f) return;

        Quaternion targetRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 8f * Time.deltaTime);
    }

    void HandleShooting()
    {
        fireTimer += Time.deltaTime;

        if (!firstShotFired)
        {
            if (fireTimer >= firstShotDelay)
            {
                FireProjectile();
                firstShotFired = true;
                fireTimer = 0f;
            }
        }
        else if (fireTimer >= fireCooldown)
        {
            FireProjectile();
            fireTimer = 0f;
        }
    }

    void ResetFire()
    {
        fireTimer = 0f;
        firstShotFired = false;
    }

    void FireProjectile()
    {
        Vector3 direction = player.position - firePoint.position;

        GameObject proj = Instantiate(
            projectilePrefab,
            firePoint.position,
            Quaternion.LookRotation(direction)
        );

        proj.GetComponent<Projectile>().Init(direction);
    }

    /* ---------- DEBUG ---------- */

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, disengageRange);
    }
}
