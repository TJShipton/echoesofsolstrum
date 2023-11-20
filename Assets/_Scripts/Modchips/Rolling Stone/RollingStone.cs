using UnityEngine;

public class RollingStone : Modchip
{
    private Rigidbody playerRigidbody;
    public GameObject boulderPrefab;
    public GameObject boulderSpawnpoint;
    public float rollForce = 2.0f;
    private float lastUseTime;

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
                    RollBoulder(playerRigidbody); // Pass the Rigidbody to RollBoulder
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

    private void RollBoulder(Rigidbody playerRigidbody)
    {

        GameObject projectile = Instantiate(boulderPrefab, boulderSpawnpoint.transform.position, Quaternion.identity);
        if (projectile == null)
        {
            Debug.LogError("Failed to instantiate projectile.");
            return;
        }

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody not found on the instantiated projectile.");
            return;
        }

        Vector3 forceDirection = playerRigidbody.transform.forward;
        Vector3 totalForce = forceDirection.normalized * rollForce;
        rb.AddForce(totalForce, ForceMode.VelocityChange);

        BoulderProjectile projectileScript = projectile.GetComponent<BoulderProjectile>();
        if (projectileScript != null)
        {
            projectileScript.SetModchipData(modchipData);
        }
    }

}
