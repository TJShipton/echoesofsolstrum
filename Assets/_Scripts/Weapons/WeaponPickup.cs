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
        // Get the singleton instance of InventoryManager
        WeaponInventoryManager inventoryManager = WeaponInventoryManager.instance;
        if (inventoryManager != null)
        {
            // Call InstantiateNewWeapon method from WeaponManager
            Weapon newWeapon = WeaponManager.instance.InstantiateNewWeapon(weaponPrefab.gameObject, inventoryManager.weaponHolder);
            if (newWeapon != null) // Check if new weapon was successfully instantiated
            {
                // Create a new WeaponInventoryItem for the picked weapon
                WeaponInventoryItem newWeaponItem = new WeaponInventoryItem(newWeapon.weaponName, newWeapon.gameObject);

                // Add the new weapon item to the inventory
                inventoryManager.AddWeapon(newWeapon); // Correctly pass a WeaponInventoryItem here
            }

            // Destroy the weapon pickup object
            Destroy(gameObject);
        }
    }




}



