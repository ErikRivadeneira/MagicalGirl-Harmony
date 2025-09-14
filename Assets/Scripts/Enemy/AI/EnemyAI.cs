using System;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering.Universal;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState { Patrol, Chase, Attack, Search }
    private EnemyState currentState;

    [Header("References")]
    public Transform[] patrolPoints;
    public Transform player;
    public LayerMask visionMask;
    [SerializeField] private Light2D light;
    [SerializeField] private Sprite bulletSprite;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip shootClip;

    [Header("Movement")]
    public float speed = 2f;
    private int currentPatrolIndex = 0;

    [Header("Detection")]
    public float visionRange = 6f;
    public float visionAngle = 45f;
    public float attackRange = 2f;
    public bool playerSpotted = false;
    public PlayerStealth playerStealth;
    

    [Header("Search")]
    public float searchDuration = 3f;
    private float searchTimer = 0f;
    private Vector2 lastKnownPlayerPos;

    [Header("Combat")]
    public float attackCooldown = 1f;
    public Transform shootPoint;
    public float bulletDamage;
    public float bulletSpeed;
    public float bulletReach;
    public float bulletSpread;
    private float lastAttackTime;

    public static event Action OnPlayerDiscovered;

    private void Start()
    {
        currentState = EnemyState.Patrol;
    }

    private void Update()
    {
        switch (currentState)
        {
            case EnemyState.Patrol:
                Patrol();
                LookForPlayer();
                break;

            case EnemyState.Chase:
                Chase();
                break;

            case EnemyState.Attack:
                Attack();
                break;

            case EnemyState.Search:
                Search();
                break;
        }

        playerSpotted = CanSeePlayer();
    }

    #region PATROL
    private void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        Transform targetPoint = patrolPoints[currentPatrolIndex];
        MoveTowards(targetPoint.position);
        FaceTarget(targetPoint.position);

        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }
    #endregion

    #region CHASE
    private void Chase()
    {
        if (player == null) return;

        MoveTowards(player.position);
        FaceTarget(player.position);

        float dist = Vector2.Distance(transform.position, player.position);

        if (!CanSeePlayer())
        {
            lastKnownPlayerPos = player.position;
            currentState = EnemyState.Search;
            searchTimer = searchDuration;
        }
        else if (dist <= attackRange)
            currentState = EnemyState.Attack;
    }
    #endregion

    #region ATTACK
    private void Attack()
    {
        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);

        if (dist > attackRange)
        {
            currentState = EnemyState.Chase;
            return;
        }

        if (Time.time > (lastAttackTime + attackCooldown))
        {
            Shoot();
            lastAttackTime = Time.time;
        }
    }
    #endregion

    #region SEARCH
    private void Search()
    {
        float dist = Vector2.Distance(transform.position, lastKnownPlayerPos);

        if (dist > 0.1f)
        {
            MoveTowards(lastKnownPlayerPos);
            FaceTarget(lastKnownPlayerPos);
        }
        else
        {
            float searchAngleOffset = Mathf.Sin(Time.time * 2f) * 45f;
            Vector2 dirToTarget = (lastKnownPlayerPos - (Vector2)transform.position).normalized;
            Vector2 searchDir = Quaternion.Euler(0, 0, searchAngleOffset) * dirToTarget;
            FaceTarget((Vector2)transform.position + searchDir);
        }

        searchTimer -= Time.deltaTime;

        if (CanSeePlayer())
        {
            currentState = EnemyState.Chase;
            return;
        }

        if (searchTimer <= 0f)
        {
            currentState = EnemyState.Patrol;
        }
    }
    #endregion

    #region HELPERS
    private void MoveTowards(Vector2 target)
    {
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    private void LookForPlayer()
    {
        if (CanSeePlayer())
        {
            OnPlayerDiscovered?.Invoke();
            currentState = EnemyState.Chase;
        }
    }

    private bool CanSeePlayer()
    {
        if (player == null) return false;

        Vector2 dirToPlayer = (player.position - transform.position).normalized;
        float angle = Vector2.Angle(transform.right, dirToPlayer);

        float effectiveRange = visionRange;
        float effectiveAngle = visionAngle;

        // Factor in player visibility
        if (playerStealth != null)
        {
            effectiveRange *= playerStealth.visibility; 
            light.pointLightOuterRadius = effectiveRange;
            // Optional scale angle:
            // effectiveAngle *= playerStealth.visibility;
        }

        if (Vector2.Distance(transform.position, player.position) <= effectiveRange &&
            angle < effectiveAngle / 2f)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dirToPlayer, effectiveRange, visionMask);
            if (hit.collider != null && hit.collider.transform == player)
            {
                return true;
            }
        }

        return false;
    }

    private void FaceTarget(Vector2 target)
    {
        Vector2 dir = (target - (Vector2)transform.position).normalized;
        if (dir.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void Shoot()
    {
        GameObject bullet = BulletPool.instance.GetPooledBullet();
        if (bullet == null) return;

        Vector2 dir = ((Vector2)player.position - (Vector2)transform.position).normalized;
        Vector2 directionWithSpread = ApplySpread(dir, bulletSpread);

        bullet.transform.position = shootPoint.position;
        bullet.GetComponent<BulletControler>().SetBulletData(bulletSpeed, bulletReach, bulletDamage, directionWithSpread, bulletSprite);
        bullet.SetActive(true);
        source.PlayOneShot(shootClip);
    }

    Vector2 ApplySpread(Vector2 baseDirection, float spreadAngleDegrees)
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
    #endregion

    private void OnDrawGizmos()
    {
        if (player == null) return;

        if (patrolPoints != null && patrolPoints.Length > 0)
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < patrolPoints.Length; i++)
            {
                if (patrolPoints[i] == null) continue;
                Gizmos.DrawSphere(patrolPoints[i].position, 0.2f);
                Transform next = patrolPoints[(i + 1) % patrolPoints.Length];
                if (next != null)
                    Gizmos.DrawLine(patrolPoints[i].position, next.position);
            }
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.yellow;
        int segments = 30;
        float halfAngle = visionAngle / 2f;
        float effectiveRange = visionRange;

        if (playerStealth != null)
            effectiveRange *= playerStealth.visibility;

        Vector3 lastPoint = transform.position;
        for (int i = 0; i <= segments; i++)
        {
            float angle = -halfAngle + (visionAngle / segments) * i;
            Vector2 dir = Quaternion.Euler(0, 0, angle) * transform.right;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, effectiveRange, visionMask);
            Vector3 endPoint = hit.collider ? (Vector3)hit.point : (Vector3)(transform.position + (Vector3)dir * effectiveRange);
            if (i > 0)
                Gizmos.DrawLine(lastPoint, endPoint);
            lastPoint = endPoint;
        }

        if (CanSeePlayer())
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(transform.position, player.position);
        }
    }
}
