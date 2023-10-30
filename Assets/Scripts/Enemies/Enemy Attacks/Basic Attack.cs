using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class BasicAttack : MonoBehaviour
{
    private Enemy enemy;
    private Transform playerTransform;
    private IDamageable playerDamageable;
    public Canvas enemyCanvas; // Canvas variable

    void Start()
    {
        enemy = GetComponent<Enemy>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerDamageable = playerTransform.GetComponent<IDamageable>();
        //uiCanvas = GameObject.FindGameObjectWithTag("UI").GetComponent<Canvas>(); // Find canvas tagged "UI"
    }

    void Update()
    {
        if (playerTransform == null)
        {

            return;

        }
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer < enemy.Data.attackRange && Time.time > enemy.LastAttackTime + enemy.Data.attackDelay)
        {
            Attack();
            enemy.LastAttackTime = Time.time;
            Debug.Log("Enemy attacking");
        }
    }

    void Attack()
    {
        if (playerDamageable != null)
        {
            playerDamageable.TakeDamage(enemy.Data.attackDamage, enemyCanvas); // Passing uiCanvas
            // Trigger any attack animations or sounds here
        }
    }
}
