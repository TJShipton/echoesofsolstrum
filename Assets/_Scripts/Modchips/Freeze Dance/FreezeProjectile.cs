using UnityEngine;
using UnityEngine.AI;

public class FreezeProjectile : MonoBehaviour
{

    public float freezeDuration; // Duration of the freeze effect

    public void Setup(float duration)
    {
        freezeDuration = duration;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if we've hit an enemy
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            ApplyFreeze(enemy);
        }

        // Destroy the projectile upon collision
        Destroy(gameObject);
    }

    private void ApplyFreeze(Enemy enemy)
    {
        NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.isStopped = true; // Stop the enemy from moving

            //SET ANIM TRIGGER HERE

            // Schedule the unfreeze action
            enemy.Invoke("Unfreeze", freezeDuration);
        }
    }
}