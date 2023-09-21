using UnityEngine;

public class PlayerWeaponHandler : MonoBehaviour
{
    public Transform weaponHolder; // Reference to where the weapon will be attached on the player

    public void EquipWeapon(GameObject weapon)
    {
        // Instantiate the weapon at the weaponHolder's position and rotation
        GameObject weaponInstance = Instantiate(weapon, weaponHolder.position, weaponHolder.rotation, weaponHolder);
        Debug.Log("Equipped weapon name: " + weaponInstance.name);  // Log the name of the instantiated weapon

        // We'll use StartsWith in case Unity appends additional information to the name like "(Clone)"
        if (weaponInstance.name.StartsWith("OBow"))
        {
            Debug.Log("OBow identified. Adjusting orientation.");
            weaponInstance.transform.localRotation = Quaternion.Euler(0f, 85f, 0f);
        }
        else if (weaponInstance.name.Contains("Drumstick"))
        {
            // Drumstick is fine, no need to adjust
        }
    }
}

