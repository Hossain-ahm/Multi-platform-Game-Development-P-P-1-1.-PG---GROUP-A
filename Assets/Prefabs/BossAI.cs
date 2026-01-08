using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossAI : MonoBehaviour
{
    enum State { Idle, Chase, Return }
    State currentState = State.Idle;

    public Animator bossAnim;
    public Transform playerPlacement;
    private BirdController birdController;
    [Header("Idle Hover")]
    public float hoverAmplitude = 1f;
    public float hoverSpeed = 1f;

    [Header("Chase")]
    public float chaseSpeed = 10f;
    public float attackRange = 20f;

    [Header("Combat")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireCooldown = 2f;
    public GameObject bossUI;
    public Slider healthSlider;
    public TMP_Text bossName;
    public EnemyHealth bossHealth;

    float fireTimer;

    private Transform player;
    private Vector3 originPosition;
    private bool playerInArena;
    private Quaternion originRotation;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).transform;
        birdController = GameObject.FindGameObjectWithTag("Player").GetComponent<BirdController>();
        originPosition = transform.position;
        originRotation = transform.rotation;
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                HandleIdle();
                break;

            case State.Chase:
                HandleChase();
                break;

            case State.Return:
                HandleReturn();
                break;
        }
        if (playerInArena)
        {
            healthSlider.value = bossHealth.GetHealthNormalised();
        }
    }

    /* ---------- STATES ---------- */

    void HandleIdle()
    {
        HoverAtOrigin();

        if (playerInArena)
        {
            currentState = State.Chase;
        }
    }

    void HandleChase()
    {
        if (!playerInArena)
        {
            fireTimer = 0f;
            currentState = State.Return;
            return;
        }

        Vector3 dir = player.position - transform.position;
        float distance = dir.magnitude;

        FaceDirection(dir);

        if (distance > attackRange)
        {
            //transform.position += dir.normalized * chaseSpeed * Time.deltaTime;
            fireTimer = 0f;
        }
        else
        {
            HandleShooting(dir);
        }
    }

    void HandleReturn()
    {
        Vector3 dir = originPosition - transform.position;

        FaceDirection(dir);

        transform.position += dir.normalized * chaseSpeed * Time.deltaTime;

        if (dir.magnitude < 0.5f)
        {
            transform.position = originPosition;
            transform.rotation = originRotation;
            currentState = State.Idle;
        }
    }

    /* ---------- BEHAVIOURS ---------- */

    void HoverAtOrigin()
    {
        Vector3 pos = originPosition;
        pos.y += 10 + (Mathf.Sin(Time.time * hoverSpeed) * hoverAmplitude);
        transform.position = pos;
    }

    void FaceDirection(Vector3 dir)
    {
        if (dir.sqrMagnitude < 0.01f) return;

        Quaternion targetRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRot,
            5f * Time.deltaTime
        );
    }

    /* ---------- ARENA CALLBACKS ---------- */

    public void PlayerEnteredArena()
    {
        if (!playerInArena)
            StartCoroutine(BossEngagedRoutine());
    }
    IEnumerator BossEngagedRoutine()
    {
        bossAnim.SetTrigger("engage");
        birdController.gameObject.transform.position = playerPlacement.transform.position;
        birdController.blockInput = true;
        birdController.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        yield return new WaitForSecondsRealtime(6.0f);
        birdController.blockInput = false;
        playerInArena = true;
        bossUI.SetActive(true);
        bossName.text = "AIR BOSS";
        hoverAmplitude = 5f;
        yield break;

    }
    public void PlayerExitedArena()
    {
        //playerInArena = false;
    }

    void HandleShooting(Vector3 dir)
    {
        fireTimer += Time.deltaTime;

        if (fireTimer >= fireCooldown)
        {
            FireProjectile(dir);
            fireTimer = 0f;
        }
    }

    void FireProjectile(Vector3 dir)
    {
        if (projectilePrefab == null || firePoint == null) return;

        GameObject proj = Instantiate(
            projectilePrefab,
            firePoint.position,
            Quaternion.LookRotation(dir)
        );

        Projectile projectile = proj.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.Init(dir);
        }
    }

}
