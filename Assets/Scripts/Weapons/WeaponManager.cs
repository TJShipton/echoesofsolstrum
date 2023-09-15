using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public List<Weapon> availableWeapons; // List of weapons the player has
    public Weapon currentWeapon;          // Currently equipped weapon
    public Transform weaponHolder;

    public void SwitchWeapon(Weapon newWeapon)
    {
        if (availableWeapons.Contains(newWeapon))
        {
            // Deactivate current weapon
            if (currentWeapon != null) currentWeapon.gameObject.SetActive(false);

            // Set and activate new weapon
            currentWeapon = newWeapon;
            currentWeapon.gameObject.SetActive(true);
        }
    }

}
//  the key to managing systems in game development is to keep the responsibilities clear. Your
//  WeaponData should handle static data related to the weapon, Weapon should handle weapon logic,
//  and WeaponManager should handle player-level weapon management.