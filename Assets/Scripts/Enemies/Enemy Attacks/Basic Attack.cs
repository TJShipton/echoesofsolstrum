using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class BasicAttack : MonoBehaviour
{
    private Enemy enemy;
    private Transform playerTransform;
    private IDamageable playerDamageable;

    void Start()
    {
        enemy = GetComponent<Enemy>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerDamageable = playerTransform.GetComponent<IDamageable>();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer < enemy.Data.attackRange && Time.time > enemy.LastAttackTime + enemy.Data.attackDelay)
        {
            Attack();
            enemy.LastAttackTime = Time.time;
        }
    }

    void Attack()
    {
        if (playerDamageable != null)
        {
            playerDamageable.TakeDamage(enemy.Data.attackDamage);
            // Trigger any attack animations or sounds here
        }
    }
}
