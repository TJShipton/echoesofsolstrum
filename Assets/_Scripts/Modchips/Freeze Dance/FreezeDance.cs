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
    }


    public override void ModAttack()
    {
        // Dynamically find the player's Rigidbody when needed
        if (PlayerController.instance != null)
        {
            Rigidbody playerRigidbody = PlayerController.instance.GetComponent<Rigidbody>();
            if (playerRigidbody != null)
            {
                if (Time.time - lastUseTime >= modchipData.modCooldown)
                {
                    ThrowProjectile(playerRigidbody); // Pass the Rigidbody to RollBoulder
                    lastUseTime = Time.time; // Reset the timer
                }
            }
            else
            {
                Debug.LogError("playerRigidbody is null in ModAttack.");
            }
        }
        else
        {
            Debug.LogError("PlayerController instance not found in ModAttack.");
        }
    }

    private void ThrowProjectile(Rigidbody playerRigidbody)
    {
        if (projectilePrefab != null && FPSpawnpoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, FPSpawnpoint.transform.position, Quaternion.identity); // Use Quaternion.identity to ignore the spawn point's rotation
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Capture the player's current velocity
                Vector3 playerVelocity = playerRigidbody.velocity;

                // Calculate the force using the player's forward direction
                Vector3 forceDirection = PlayerController.instance.transform.forward; // Use the player's forward direction
                Vector3 totalForce = forceDirection.normalized * throwForce;

                // Add upward force for arc
                totalForce += Vector3.up * upwardForce;

                // Add the player's current horizontal velocity to the projectile
                Vector3 initialProjectileVelocity = totalForce + new Vector3(playerVelocity.x, 0, playerVelocity.z);
                rb.velocity = initialProjectileVelocity;
            }
        }
    }




}