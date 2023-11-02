using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public int damage;
    public Canvas enemyCanvas;
    public float homingSpeed = 5f;  // Speed at which the projectile homes in on the target
    public float detectionRadius = 10f;  // Radius within which to search for enemies

    public List<IWeaponModifier> equippedModifiers;

    private Transform target;  // The enemy to home in on

    private void Start()
    {
        if (enemyCanvas == null)
            enemyCanvas = GameManager.EnemyCanvas;

        FindClosestEnemy();
    }

    private void Update()
    {
        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, homingSpeed * Time.deltaTime);
        }
    }

    public void SetDamage(int damageAmount)
    {
        damage = damageAmount;
    }
    private void FindClosestEnemy()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
        float closestDistance = float.MaxValue;

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))  // Assumes enemies are tagged as "Enemy"
            {
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    target = hitCollider.transform;
                    //Debug.Log("Found enemy: " + hitCollider.name);  // Debug line
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        var hit = collision.gameObject.GetComponent<IDamageable>();
        if (hit != null)
        {
            hit.TakeDamage(damage, GameManager.EnemyCanvas);

            if (hit is Enemy enemy)
            {
                foreach (var mod in equippedModifiers)
                {
                    mod.ApplyEffect(enemy); // Casting IDamageable to Enemy
                }
            }
        }
    }
}

