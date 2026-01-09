using UnityEngine;

public class AttackState : IEnemyState
{
    public void Enter(EnemyAI enemy)
    {
        enemy.Agent.speed = enemy.speed;
        enemy.Agent.stoppingDistance = enemy.attackRange * 0.85f;
    }

    public void UpdateState(EnemyAI enemy)
    {
        if (enemy.Player == null) return;

        float dist = Vector2.Distance(enemy.transform.position, enemy.Player.position);

        if (dist > enemy.attackRange)
        {
            enemy.Agent.isStopped = false;
            enemy.ChangeState(new ChaseState());
            return;
        }

        if (enemy.CanAttack())
        {
            enemy.Shoot();
            enemy.RecordAttack();
        }
    }

    public void Exit(EnemyAI enemy) { }
}
