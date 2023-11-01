using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : EnemyState
{
    private EnemyAnimationController animationController;
    private NavMeshAgent navMeshAgent;
    private Vector3 initialPosition;
    private List<Vector3> waypoints;
    private int currentWaypointIndex = 0;
    private float pauseTimer = 0f;

    public PatrolState(Enemy enemy, EnemyAnimationController animationController) : base(enemy)
    {
        this.animationController = animationController;
        this.navMeshAgent = enemy.GetComponent<NavMeshAgent>();

        // Initialize waypoints (You can customize these)
        waypoints = new List<Vector3> {
            initialPosition + new Vector3(20, 0, 0),
            initialPosition + new Vector3(8, 0, 0)
        };
    }

    public override void EnterState()
    {
        animationController.SetWalking(true);
        initialPosition = enemy.transform.position;
        navMeshAgent.SetDestination(waypoints[currentWaypointIndex]);
    }

    public override void UpdateState()
    {
        // Check if we've reached the destination
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            // Stop the walking animation and NavMeshAgent
            animationController.SetWalking(false);
            navMeshAgent.isStopped = true;

            // Pause timer logic
            pauseTimer -= Time.deltaTime;
            if (pauseTimer <= 0)
            {
                // Change the waypoint
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;

                // Set the new destination and resume NavMeshAgent
                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(waypoints[currentWaypointIndex]);

                // Reset the pause timer
                pauseTimer = enemy.Data.patrolPauseDuration;

                // Resume the walking animation
                animationController.SetWalking(true);
            }
        }
    }


    public override void ExitState()
    {
        // Stop the NavMeshAgent from moving
        navMeshAgent.isStopped = true;

        // Stop the walking animation
        animationController.SetWalking(false);
    }
}
