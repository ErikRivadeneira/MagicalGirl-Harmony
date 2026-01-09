using UnityEngine;
using UnityEngine.AI;

public class SearchState : IEnemyState
{
    private float searchTimer;
    private Vector2 searchCenter;
    private Vector3 currentSearchTarget;

    private bool movingToTarget;
    private bool visitedAtLeastOnePoint;

    private const float ARRIVAL_THRESHOLD = 0.3f;
    private const float WAIT_AT_POINT = 2f;
    private float waitTimer;

    public SearchState() { }
    public SearchState(Vector2 noisePosition)
    {
        searchCenter = noisePosition;
    }

    public void Enter(EnemyAI enemy)
    {
        searchTimer = enemy.searchDuration;
        enemy.Agent.speed = enemy.speed;

        if (searchCenter == Vector2.zero)
            searchCenter = enemy.LastKnownPlayerPos;
        else
            enemy.LastKnownPlayerPos = searchCenter;

        PickNewSearchTarget(enemy);
        visitedAtLeastOnePoint = false;
    }

    public void UpdateState(EnemyAI enemy)
    {
        float dist = Vector2.Distance(enemy.transform.position, currentSearchTarget);

        if (dist > ARRIVAL_THRESHOLD)
        {
            enemy.MoveTo(currentSearchTarget, enemy.speed);
            enemy.FaceTarget(currentSearchTarget);
        }
        else
        {
            if (!visitedAtLeastOnePoint)
                visitedAtLeastOnePoint = true;

            waitTimer -= Time.deltaTime;
            float searchAngleOffset = Mathf.Sin(Time.time * 2f) * 45f;
            Vector2 dirToTarget = (searchCenter - (Vector2)enemy.transform.position).normalized;
            Vector2 searchDir = Quaternion.Euler(0, 0, searchAngleOffset) * dirToTarget;
            enemy.FaceTarget((Vector2)enemy.transform.position + searchDir);

            if (waitTimer <= 0f)
                PickNewSearchTarget(enemy);
        }

        searchTimer -= Time.deltaTime;

        if (enemy.CanSeePlayer())
        {
            enemy.NotifyPlayerDiscovery();
            enemy.ChangeState(new ChaseState());
            return;
        }

        if (searchTimer <= 0f && visitedAtLeastOnePoint)
        {
            enemy.ChangeState(new PatrolState());
        }
    }

    public void Exit(EnemyAI enemy) { }

    private void PickNewSearchTarget(EnemyAI enemy)
    {
        float localSearchRadius = Mathf.Min(enemy.hearingRadius * 0.5f, 3f);

        for (int i = 0; i < 5; i++)
        {
            Vector2 randomOffset = Random.insideUnitCircle * localSearchRadius;
            Vector2 candidate = searchCenter + randomOffset;

            if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
            {
                currentSearchTarget = hit.position;
                movingToTarget = true;
                waitTimer = WAIT_AT_POINT;
                return;
            }
        }

        currentSearchTarget = searchCenter;
        movingToTarget = true;
        waitTimer = WAIT_AT_POINT;
    }
}
