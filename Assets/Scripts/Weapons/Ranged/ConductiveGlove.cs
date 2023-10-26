using UnityEngine;

public class ConductiveGlove : Weapon
{
    // Conductive Glove specific properties
    public GameObject projectilePrefab; // The projectile it will shoot

    public override void PrimaryAttack()
    {
        // Logic to shoot the conductive projectile
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        // Add logic to move projectile
    }


}
