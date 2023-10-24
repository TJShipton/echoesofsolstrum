using UnityEngine;

public class HyperbassFlute : Weapon
{
    public Transform projectileSpawnPoint;

    void Update()
    {

        if (Input.GetButtonDown("Fire1"))
        {
            PrimaryAttack();
        }


    }

    public override void PrimaryAttack()
    {
        if (weaponData.projectilePrefab != null && projectileSpawnPoint != null)
        {
            // Instantiate a new projectile at the spawn point
            GameObject projectile = Instantiate(weaponData.projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);

            // You might want to set other properties of the projectile here, like its damage value
            // Assuming your projectile has a script with a SetDamage method
            // projectile.GetComponent<ProjectileScript>().SetDamage(weaponData.baseDamage);

            // Apply force to the projectile to shoot it
            Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
            if (projectileRigidbody != null)
            {
                Vector3 shootDirection = projectileSpawnPoint.forward;
                projectileRigidbody.velocity = shootDirection * weaponData.projectileSpeed;
            }
            else
            {
                Debug.LogWarning("Projectile lacks a Rigidbody component, cannot apply force to shoot it.");
            }
        }
        else
        {
            Debug.LogWarning("Projectile prefab or spawn point is missing, can't shoot projectile.");
        }
    }
}

