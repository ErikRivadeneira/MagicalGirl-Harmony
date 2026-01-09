using UnityEngine;
public class PatrolState : IEnemyState
{
    private int currentPatrolIndex;

    public void Enter(EnemyAI enemy)
    {
        enemy.Agent.speed = enemy.speed;
        currentPatrolIndex = 0;
    }

    public void UpdateState(EnemyAI enemy)
    {
        if (enemy.PatrolPoints.Length == 0) return;

        Transform target = enemy.PatrolPoints[currentPatrolIndex];
        enemy.MoveTo(target.position, enemy.speed);
        enemy.FaceTarget(target.position);

        if (!enemy.Agent.pathPending && enemy.Agent.remainingDistance <= enemy.Agent.stoppingDistance)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % enemy.PatrolPoints.Length;
        }

        if (enemy.CanSeePlayer())
        {
            enemy.NotifyPlayerDiscovery();
            enemy.ChangeState(new ChaseState());
        }
    }

    public void Exit(EnemyAI enemy) { }
}
