using UnityEngine;
using UnityEngine.AI;

public class FreezeProjectile : MonoBehaviour
{
    public float freezeDuration; // Duration of the freeze effect

    void OnCollisionEnter(Collision collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            ApplyFreeze(enemy);
        }
        Destroy(gameObject);
    }

    private void ApplyFreeze(Enemy enemy)
    {
        NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
        Animator enemyAnimator = enemy.GetComponent<Animator>();

        if (agent != null && enemyAnimator != null)
        {
            agent.isStopped = true; // Stop the enemy from moving
            enemyAnimator.SetBool("isTwerking", true); // Assuming 'isTwerking' is the animation trigger

            enemy.Invoke("Unfreeze", freezeDuration); // Schedule the unfreeze action
        }
        else
        {
            if (enemyAnimator == null)
            {
                Debug.LogError("Animator not found on enemy");
            }
        }
    }
}
