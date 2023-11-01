using UnityEngine;
using UnityEngine.AI;

public class FreezeEffect : Effect
{
    private float freezeDuration = 3.0f; // 3 seconds
    private float timeFrozen;

    public override void StartEffect(Enemy enemy)
    {
        NavMeshAgent navMeshAgent = enemy.GetComponent<NavMeshAgent>();
        Animator animator  = enemy.Animator;
        
        if (navMeshAgent != null)
        {
            navMeshAgent.isStopped = true; // Stop the NavMeshAgent
        }

        if (animator != null)
        {
            animator.enabled = false; // Stop the animator
        }

        timeFrozen = 0; // Reset the freeze timer
    }

    public override void UpdateEffect(Enemy enemy)
    {
        timeFrozen += Time.deltaTime; // Update the freeze timer

        if (timeFrozen >= freezeDuration)
        {
            EndEffect(enemy); // End the effect after freezeDuration seconds
        }
    }

    public override void EndEffect(Enemy enemy)
    {
        NavMeshAgent navMeshAgent = enemy.GetComponent<NavMeshAgent>();
        Animator animator = enemy.Animator;

        if (navMeshAgent != null)
        {
            navMeshAgent.isStopped = false; // Resume the NavMeshAgent
        }

        if (animator != null)
        {
            animator.enabled = true; // Resume the animator
        }
    }
}
