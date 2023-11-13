using UnityEngine;

public class FreezeDance : Modchip
{
    public GameObject projectilePrefab;
    public GameObject FPSpawnpoint; // Reference to the spawn point object
    public float throwForce = 40f;
    public float upwardForce = 5f; // Upward force to create an arc
    public float freezeDuration = 3f;
    private float lastUseTime;
    private Rigidbody playerRigidbody;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerRigidbody = player.GetComponent<Rigidbody>();
            //Debug.Log("Player RB found");
        }

    }


    public override void ModAttack()
    {
        if (Time.time - lastUseTime >= modchipData.modCooldown)
        {
            ThrowProjectile();
            lastUseTime = Time.time; // Reset the timer
        }

    }

    private void ThrowProjectile()
    {
        if (projectilePrefab != null && FPSpawnpoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, FPSpawnpoint.transform.position, FPSpawnpoint.transform.rotation);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Capture the player's current velocity
                Vector3 playerVelocity = playerRigidbody.velocity;

                // Fixed angle for the grenade arc, e.g., 45 degrees
                float launchAngle = 45f;
                // Convert launch angle to radians
                float launchAngleRad = launchAngle * Mathf.Deg2Rad;

                // Calculate the force using spherical coordinates
                Vector3 forceDirection = Quaternion.AngleAxis(-launchAngle, FPSpawnpoint.transform.right) * FPSpawnpoint.transform.forward;
                Vector3 totalForce = forceDirection.normalized * throwForce;

                // Add the player's current horizontal velocity to the projectile
                Vector3 initialProjectileVelocity = totalForce + new Vector3(playerVelocity.x, 0, playerVelocity.z);
                rb.velocity = initialProjectileVelocity;

                FreezeProjectile freezeProjectile = projectile.GetComponent<FreezeProjectile>();
                if (freezeProjectile != null)
                {
                    freezeProjectile.Setup(freezeDuration);
                }
            }
        }
    }



}