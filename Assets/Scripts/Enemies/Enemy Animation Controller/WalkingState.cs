using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingState : EnemyState
{
    public float walkSpeed;
    private Transform enemyTransform;
    private EnemyAnimationController animationController;

    public WalkingState(Enemy enemy, EnemyAnimationController animationController) : base(enemy)
    {
        this.animationController = animationController;
    }

    public override void EnterState()
    {
        animationController.SetWalking(true);
    }

    public override void UpdateState()
    {
        // Logic for walking. E.g. moving between patrol points.
        float moveDirection = Mathf.Sign(Mathf.Sin(Time.time)); // Oscillates between -1 and 1
        enemy.transform.position += new Vector3(moveDirection * enemy.Data.speed * Time.deltaTime, 0, 0);

        // Adjust direction based on moveDirection using localScale
        Vector3 localScale = enemy.transform.localScale;

        if (moveDirection > 0)
        {
            localScale.x = Mathf.Abs(localScale.x);  // make it positive
        }
        else
        {
            localScale.x = -Mathf.Abs(localScale.x);  // make it negative
        }

        enemy.transform.localScale = localScale;
    }




    public override void ExitState()
    {
        animationController.SetWalking(false);
    }
}
