using UnityEngine;

public class HyperbassFlute : Weapon
{
    public Transform projectileSpawnPoint;
    public Transform playerTransform;  // Reference to the player's transform
    public Canvas EnemyCanvas;

    
    public Animator animator;
    private Rigidbody playerRigidbody;  // Reference to the player's rigidbody
    

    private void Awake()
    {
        animator = GetComponentInParent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        
        if (playerTransform != null)
        {
            playerRigidbody = playerTransform.GetComponent<Rigidbody>();
        }
        else
        {
            Debug.LogError("Player object not found in the scene.");

        }

        // Access the GameManager to get the EnemyCanvas
        if (EnemyCanvas == null)
        {
            EnemyCanvas = GameManager.EnemyCanvas;
        }

    }

    public override void PrimaryAttack()
    {
        if (weaponData.projectilePrefab != null && projectileSpawnPoint != null)
        {
            GameObject projectile = Instantiate(weaponData.projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
            Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
            ProjectileScript projectileScript = projectile.GetComponent<ProjectileScript>();  // Get the ProjectileScript component

            if (projectileScript != null)
            {
                projectileScript.SetDamage(weaponData.baseDamage);   // Set the damage value
            }
            else
            {
                Debug.LogWarning("ProjectileScript component is missing on the projectile prefab, can't set damage value.");
            }

            if (projectileRigidbody != null)
            {
                Vector3 shootDirection = playerTransform.forward;  // Use player's forward direction
                Vector3 playerVelocity = playerRigidbody.velocity;  // Get the player's current velocity

                // Adjusting the projectile's velocity
                projectileRigidbody.velocity = shootDirection * (weaponData.projectileSpeed + playerVelocity.magnitude);
            }
            else
            {
                Debug.LogWarning("Rigidbody component is missing on the projectile prefab, can't set velocity.");
            }

            if (animator != null)
            {
                animator.SetTrigger("HyperbassAttack");
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
