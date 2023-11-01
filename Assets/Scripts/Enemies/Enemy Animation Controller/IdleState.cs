using UnityEngine;

public class IdleState : EnemyState
{
    public IdleState(Enemy enemy) : base(enemy) { }

    private float timeInIdle = 0f;
    public override void EnterState()
    {
        // Logic when entering the idle state
    }

    public override void UpdateState()
    {
        timeInIdle += Time.deltaTime;

        if (timeInIdle > 0f) // switch to walking after x seconds
        {
            enemy.SetState(new PatrolState(enemy, enemy.GetComponent<EnemyAnimationController>()));
        }
    }

    public override void ExitState()
    {
        // Logic when exiting the idle state
    }
}
