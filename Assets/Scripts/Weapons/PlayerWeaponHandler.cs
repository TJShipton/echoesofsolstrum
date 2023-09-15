using UnityEngine;

public class PlayerWeaponHandler : MonoBehaviour
{
    public Transform weaponHolder; // Reference to where the weapon will be attached on the player

    public void EquipWeapon(GameObject weapon)
    {
        // Instantiate the weapon at the weaponHolder's position and rotation
        Instantiate(weapon, weaponHolder.position, weaponHolder.rotation, weaponHolder);
    }
}

