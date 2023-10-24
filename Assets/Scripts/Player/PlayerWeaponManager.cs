using UnityEngine;
using UnityEngine.WSA;

public class PlayerWeaponManager : MonoBehaviour
{
    public int activeWeaponSlot = 0;  // Index of the active weapon slot
    public Transform weaponHolder;    // Transform where weapons are held

    void Update()
    {
        // Listen for the swap action (assuming you have bound the swap action to the "SwapWeapon" button)
        if (Input.GetKeyDown(KeyCode.M))
        {
            SwapWeapon();
        }
    }

    public void SwapWeapon()
    {
        // Get a reference to the InventoryManager
        InventoryManager inventoryManager = InventoryManager.instance;

        int nextWeaponSlot = activeWeaponSlot;
        do
        {
            nextWeaponSlot = (nextWeaponSlot + 1) % inventoryManager.maxWeaponSlots;
        } while (inventoryManager.slots[nextWeaponSlot].Item == null && nextWeaponSlot != activeWeaponSlot);


        // Check if the next weapon slot has a weapon
        if (inventoryManager.slots[nextWeaponSlot].Item is WeaponInventoryItem nextWeapon)
        {
            // Update the active weapon slot
            activeWeaponSlot = nextWeaponSlot;

            // Get the weapon prefab from the next weapon
            GameObject nextWeaponPrefab = nextWeapon.weaponPrefab;

            // Null check before proceeding
            if (nextWeaponPrefab == null)
            {
                Debug.LogWarning("Next weapon prefab is null.");
                return;  // Exit the method early if nextWeaponPrefab is null
            }

            // Replace the weapon in the weapon holder
            foreach (Transform child in weaponHolder)
            {
                child.gameObject.SetActive(false);  // Deactivate the current weapon
            }

            // Check if the next weapon is already instantiated in the weapon holder
            Transform nextWeaponTransform = weaponHolder.Find(nextWeaponPrefab.name);
            if (nextWeaponTransform == null)
            {
                // Instantiate the next weapon in the weapon holder
                WeaponManager.instance.InstantiateNewWeapon(nextWeaponPrefab, weaponHolder);
            }
            else
            {
                // Activate the next weapon
                nextWeaponTransform.gameObject.SetActive(true);
            }

            Debug.Log("Switched to weapon: " + nextWeaponPrefab.name);
        }
        else
        {
            Debug.LogWarning("No weapon in slot " + nextWeaponSlot);
        }
    }


}
