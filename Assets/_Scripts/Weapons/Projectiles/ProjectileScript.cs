using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public int damage;
    public Canvas EnemyCanvas;
    public float homingSpeed = 5f;  // Speed at which the projectile homes in on the target
    public float detectionRadius = 10f;  // Radius within which to search for enemies

    public List<IWeaponModifier> equippedModifiers;

    private Transform target;  // The enemy to home in on

    private void Start()
    {
       

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
        // Try to get the IDamageable component from the collided object
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();

        // If the component exists, apply damage
        if (damageable != null)
        {
            // Apply the base damage of the projectile
            damageable.TakeDamage(damage, EnemyCanvas);

            // If the object is also an Enemy, apply the weapon's modifiers
            if (collision.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
            {
                foreach (var mod in equippedModifiers)
                {
                    // Apply each modifier's effect to the enemy
                    mod.ApplyEffect(enemy);
                }
            }

            // After applying damage and modifiers, destroy the projectile or disable it
            Destroy(gameObject); // Use this if you want to destroy the projectile
                                 // gameObject.SetActive(false); // Use this if you want to reuse the projectile later
        }
    }

}

