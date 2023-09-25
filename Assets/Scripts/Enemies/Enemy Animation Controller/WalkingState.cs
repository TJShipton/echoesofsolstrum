using UnityEngine;

public class WalkingState : EnemyState
{
    private EnemyAnimationController animationController;
    private Vector3 initialPosition;
    private float moveDirection = 1f;
    private bool isPaused = false; // Flag to track if the enemy is paused
    private float pauseTimer = 0f; // Timer to track pause duration

    public WalkingState(Enemy enemy, EnemyAnimationController animationController) : base(enemy)
    {
        this.animationController = animationController;
    }

    public override void EnterState()
    {
        animationController.SetWalking(true);
        initialPosition = enemy.transform.position;
    }

    public override void UpdateState()
    {
        // If the enemy is paused, countdown
        if (isPaused)
        {
            // Log that the enemy is paused
            Debug.Log("Enemy is paused.");

            // Stop the walking animation
            animationController.SetWalking(false);

            pauseTimer -= Time.deltaTime;
            if (pauseTimer <= 0)
            {
                // Unpause and change direction
                isPaused = false;
                moveDirection *= -1;
                initialPosition = enemy.transform.position;
                // Resume the walking animation
                animationController.SetWalking(true);
            }
            return; // Skip the rest of the update
        }

        // Patrol logic
        Vector3 currentPosition = enemy.transform.position;
        float distanceMoved = Mathf.Abs(currentPosition.x - initialPosition.x);

        if (distanceMoved >= enemy.Data.patrolDistance)
        {
            // Log that the enemy has reached the end of patrol
            Debug.Log("Reached the end of patrol, pausing.");

            // Pause the enemy and initialize pauseTimer
            isPaused = true;
            pauseTimer = enemy.Data.patrolPauseDuration;
            return; // Skip the rest of the update
        }

        Vector3 movement = new Vector3(moveDirection * enemy.Data.speed * Time.deltaTime, 0, 0);
        enemy.transform.position += movement;

        Vector3 localScale = enemy.transform.localScale;
        if (moveDirection > 0)
        {
            enemy.transform.eulerAngles = new Vector3(0, 90, 0);
        }
        else
        {
            enemy.transform.eulerAngles = new Vector3(0, -90, 0);
        }
    }

    public override void ExitState()
    {
        animationController.SetWalking(false);
    }
}
