using System.Collections.Generic;
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



    }

    public override void PrimaryAttack()
    {
        if (weaponData.projectilePrefab != null && projectileSpawnPoint != null)
        {
            GameObject projectile = Instantiate(weaponData.projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
            Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
            ProjectileScript projectileScript = projectile.GetComponent<ProjectileScript>();  // Get the ProjectileScript component

            // Pass the equippedModifiers to the projectile
            if (projectileScript != null)
            {
                projectileScript.SetDamage(weaponData.baseDamage);
                projectileScript.equippedModifiers = new List<IWeaponModifier>(equippedModifiers); // Transfer the modifiers
            }


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


            if (animator != null)
            {
                animator.SetTrigger("HyperbassAttack");
            }

            weaponData.StartCooldown();
        }

    }

    void Update()
    {
        weaponData.UpdateCooldown(Time.deltaTime);

        if (!weaponData.IsOnCooldown() && gameObject.activeSelf)
        {
            gameObject.SetActive(false); // Deactivate the weapon when not on cooldown
        }
    }

}
