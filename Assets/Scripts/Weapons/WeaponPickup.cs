using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public Weapon weaponPrefab; // Prefab of the weapon to be picked up

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") // Check if the collider is tagged as 'Player'
        {
            PickUp(other.gameObject); // Call the PickUp function
        }
    }

    void PickUp(GameObject player)
    {
        WeaponManager weaponManager = player.GetComponent<WeaponManager>();
        if (weaponManager != null)
        {
            Weapon newWeapon = Instantiate(weaponPrefab, weaponManager.weaponHolder);
            newWeapon.gameObject.SetActive(false);

            if (weaponManager.weaponHolder != null)
            {
                newWeapon.transform.SetParent(weaponManager.weaponHolder);

                // Set both local position and rotation according to the Weapon class
                newWeapon.transform.localPosition = newWeapon.localPosition;
                newWeapon.transform.localRotation = Quaternion.identity;
                newWeapon.transform.localEulerAngles = newWeapon.localOrientation;
            }

            weaponManager.availableWeapons.Add(newWeapon);
            weaponManager.SwitchWeapon(newWeapon);
        }

        Destroy(gameObject);
    }

}