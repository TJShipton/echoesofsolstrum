using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public List<Weapon> availableWeapons; // List of weapons the player has
    public Weapon currentWeapon;          // Currently equipped weapon
    public Transform weaponHolder;
    public List<WeaponBlueprint> unlockedWeapons = new List<WeaponBlueprint>();
    
    
    
    public void SwitchWeapon(Weapon newWeapon)
    {
        Debug.Log("Switching to new weapon: " + newWeapon.weaponName);

        if (availableWeapons.Contains(newWeapon))
        {
            // Deactivate current weapon
            if (currentWeapon != null)
            {
                Debug.Log("Deactivating current weapon: " + currentWeapon.weaponName);
                currentWeapon.gameObject.SetActive(false);
            }

            // Set and activate new weapon
            currentWeapon = newWeapon;
            Debug.Log("Activating new weapon: " + currentWeapon.weaponName);
            currentWeapon.gameObject.SetActive(true);
        }

        GameManager.instance.UnlockWeapon(newWeapon.weaponName);
    }


}
//   keep the responsibilities clear
//  WeaponData should handle static data related to the weapon, Weapon should handle weapon logic,
//  and WeaponManager should handle player-levelusing System.Collections;
