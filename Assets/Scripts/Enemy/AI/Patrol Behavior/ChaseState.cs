using UnityEngine;

public class ChaseState : IEnemyState
{
    public void Enter(EnemyAI enemy)
    {
        enemy.Agent.speed = enemy.speed + enemy.speedIncrementForChase;
    }

    public void UpdateState(EnemyAI enemy)
    {
        if (enemy.Player == null) return;

        enemy.MoveTo(enemy.Player.position, enemy.Agent.speed);
        enemy.FaceTarget(enemy.Player.position);

        float dist = Vector2.Distance(enemy.transform.position, enemy.Player.position);

        if (!enemy.CanSeePlayer())
        {
            enemy.LastKnownPlayerPos = enemy.Player.position;
            enemy.ChangeState(new SearchState());
        }
        else if (dist <= enemy.attackRange)
        {
            enemy.ChangeState(new AttackState());
        }
    }

    public void Exit(EnemyAI enemy) { }
}
