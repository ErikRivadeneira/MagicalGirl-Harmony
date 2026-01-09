using System;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class EnemyAI : MonoBehaviour
{
    private IEnemyState currentState;

    [Header("References")]
    public Transform[] PatrolPoints;
    public Transform Player;
    public PlayerStealth playerStealth;
    public NavMeshAgent Agent;
    [SerializeField] private Sprite bulletSprite;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip shootClip;

    [Header("Movement")]
    public float speed = 2f;
    public float speedIncrementForChase = 3f;

    [Header("Detection")]
    // Vision
    public float visionRange = 6f;
    public float visionAngle = 45f;
    public float attackRange = 2f;
    public Vector2 LastKnownPlayerPos;
    public LayerMask visionMask;
    // Hearing
    public float hearingRadius = 10f;
    private Vector2 lastHeardPosition;
    // Indicators
    [SerializeField] private TextMeshProUGUI alertnessText;
    [SerializeField] private Canvas alertCanvas;
    private EAlertness alertLevel = EAlertness.Idle;

    [Header("Search")]
    public float searchDuration = 3f;

    [Header("Combat")]
    public float attackCooldown = 1f;
    public Transform shootPoint;
    public float bulletDamage;
    public float bulletSpeed;
    public float bulletReach;
    public float bulletSpread;
    private float lastAttackTime;

    public static event Action OnPlayerDiscovered;

    #region ENABLE-DISABLE Events
    private void OnEnable()
    {
        NoiseSystem.OnNoiseHeard += HandleNoiseHeard;
    }

    private void OnDisable()
    {
        NoiseSystem.OnNoiseHeard -= HandleNoiseHeard;
    }
    #endregion

    private void Awake()
    {
        if (!Agent)
        {
            Agent = GetComponent<NavMeshAgent>();
            Agent.updateRotation = false;
            Agent.updateUpAxis = false;
            Agent.baseOffset = 0f;
        }
    }

    private void Start()
    {
        ChangeState(new PatrolState());
    }

    private void Update()
    {
        currentState?.UpdateState(this);
    }
    private void LateUpdate()
    {
        if (Agent != null && Agent.enabled)
        {
            Vector3 pos = Agent.transform.position;
            pos.z = 0f;
            Agent.transform.position = pos;
            // Reset unwanted rotations (keep only Z rotation for 2D facing)
            Agent.transform.rotation = Quaternion.Euler(0f, 0f, Agent.transform.rotation.eulerAngles.z);
            // keep alert indicator upwards
            alertCanvas.transform.rotation = Quaternion.identity;
        }
    }

    public void ChangeState(IEnemyState newState)
    {
        currentState?.Exit(this);
        currentState = newState;
        currentState?.Enter(this);
        if (currentState is PatrolState)
            SetAlertLevel(EAlertness.Idle);
        else if (currentState is SearchState)
            SetAlertLevel(EAlertness.Suspicious);
        else if (currentState is ChaseState || currentState is AttackState)
            SetAlertLevel(EAlertness.Alert);
    }

    #region --- Helpers ---
    public void HandleNoiseHeard(Vector2 position, float volume)
    {
        hearingRadius = visionRange * 1.5f;
        float effectiveRange = hearingRadius * volume;

        float dist = Vector2.Distance(transform.position, position);
        if (dist <= effectiveRange)
        {
            lastHeardPosition = position;

            // Only search if not already in combat
            if (!(currentState is ChaseState) && !(currentState is AttackState))
            {
                ChangeState(new SearchState(lastHeardPosition));
            }
        }
    }

    public void NotifyPlayerDiscovery()
    {
        OnPlayerDiscovered?.Invoke();
    }

    public void MoveTo(Vector2 target, float moveSpeed)
    {
        Agent.speed = moveSpeed;
        Agent.SetDestination(target);
    }

    public void FaceTarget(Vector2 target)
    {
        Vector2 dir = (target - (Vector2)transform.position).normalized;
        if (dir.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    public bool CanSeePlayer()
    {
        if (Player == null) return false;

        Vector2 dirToPlayer = (Player.position - transform.position).normalized;
        float angle = Vector2.Angle(transform.right, dirToPlayer);

        float effectiveRange = visionRange;
        if (playerStealth != null)
            effectiveRange *= playerStealth.visibility;

        if (Vector2.Distance(transform.position, Player.position) <= effectiveRange &&
            angle < visionAngle / 2f)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dirToPlayer, effectiveRange, visionMask);
            return hit.collider != null && hit.collider.transform == Player;
        }

        return false;
    }

    public bool CanAttack()
    {
        return Time.time > (lastAttackTime + attackCooldown);
    }

    public void RecordAttack()
    {
        lastAttackTime = Time.time;
    }

    public void Shoot()
    {
        GameObject bullet = BulletPool.instance.GetPooledBullet();
        if (bullet == null) return;

        Vector2 dir = ((Vector2)Player.position - (Vector2)transform.position).normalized;
        Vector2 directionWithSpread = ApplySpread(dir, bulletSpread);

        bullet.transform.position = shootPoint.position;
        bullet.GetComponent<BulletControler>().SetBulletData(
            bulletSpeed, bulletReach, bulletDamage, directionWithSpread, bulletSprite
        );

        bullet.SetActive(true);
        source.PlayOneShot(shootClip);
    }

    private Vector2 ApplySpread(Vector2 baseDirection, float spreadAngleDegrees)
    {
        float offset = UnityEngine.Random.Range(-spreadAngleDegrees * 0.5f, spreadAngleDegrees * 0.5f);
        float radians = offset * Mathf.Deg2Rad;
        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);

        return new Vector2(
            baseDirection.x * cos - baseDirection.y * sin,
            baseDirection.x * sin + baseDirection.y * cos
        ).normalized;
    }

    void SetAlertLevel(EAlertness level)
    {
        alertLevel = level;
        switch(alertLevel)
        {
            case EAlertness.Idle:
                alertnessText.text = "...";
                alertnessText.color = Color.white;
                break;
            case EAlertness.Suspicious: 
                alertnessText.text = "?";
                alertnessText.color = Color.yellow;
                break;
            case EAlertness.Alert: 
                alertnessText.text = "!";
                alertnessText.color = Color.red;
                break;
        }
    }
    #endregion
    private void OnDrawGizmosSelected()
    {
        // --- Vision radius ---
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRange);

        // --- Hearing radius ---
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, hearingRadius);

        // --- Attack radius ---
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // --- Vision cone ---
        Vector3 forward = transform.right; // agent facing direction
        Quaternion leftRot = Quaternion.AngleAxis(-visionAngle / 2f, Vector3.forward);
        Quaternion rightRot = Quaternion.AngleAxis(visionAngle / 2f, Vector3.forward);

        Vector3 leftDir = leftRot * forward;
        Vector3 rightDir = rightRot * forward;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + leftDir * visionRange);
        Gizmos.DrawLine(transform.position, transform.position + rightDir * visionRange);

        // --- Last Known Player Position ---
        if (LastKnownPlayerPos != Vector2.zero)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(LastKnownPlayerPos, 0.2f);
            Gizmos.DrawLine(transform.position, LastKnownPlayerPos);
        }

        // --- Last Heard Position ---
        if (lastHeardPosition != Vector2.zero)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(lastHeardPosition, 0.2f);
            Gizmos.DrawLine(transform.position, lastHeardPosition);
        }
    }

}
