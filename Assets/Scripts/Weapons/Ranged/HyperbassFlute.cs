using UnityEngine;

public class HyperbassFlute : Weapon
{
    public Transform projectileSpawnPoint;
    private Animator animator;
    private void Awake()  // Changed to Awake from Start
    {
        animator = GetComponentInParent<Animator>();  // Changed to GetComponentInParent
    }
    public override void PrimaryAttack()
    {
        //Debug.Log("PrimaryAttack called on HyperbassFlute");

        if (weaponData.projectilePrefab != null && projectileSpawnPoint != null)
        {
            // Instantiate a new projectile at the spawn point
            GameObject projectile = Instantiate(weaponData.projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);

            // set other properties of the projectile here, like its damage value
            // Assuming your projectile has a script with a SetDamage method
            // projectile.GetComponent<ProjectileScript>().SetDamage(weaponData.baseDamage);

            // Apply force to the projectile to shoot it
            Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
            if (projectileRigidbody != null)
            {
                Vector3 shootDirection = projectileSpawnPoint.forward;
                projectileRigidbody.velocity = shootDirection * weaponData.projectileSpeed;
            }
            if (animator != null)
            {
                animator.SetTrigger("PrimaryAttack");
            }
            else
            {
                Debug.LogWarning("Animator component is missing, can't trigger animation.");
            }
        }
        else
        {
            Debug.LogWarning("Projectile prefab or spawn point is missing, can't shoot projectile.");
        }
    }
    
}
