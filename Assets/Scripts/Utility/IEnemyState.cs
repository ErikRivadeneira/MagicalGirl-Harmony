public interface IEnemyState
{
    void Enter(EnemyAI enemy);
    void UpdateState(EnemyAI enemy);
    void Exit(EnemyAI enemy);
}
