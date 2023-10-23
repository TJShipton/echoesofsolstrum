using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager instance; // Singleton instance

    public List<Weapon> availableWeapons; // List of weapons the player has
    public Weapon currentWeapon;
    public List<Weapon> weaponInventory;  // Existing inventory list
    public int equippedWeaponIndex = 0;  // New variable to keep track of equipped weapon
    public Transform weaponHolder;

    public List<GameObject> unlockedWeapons;
    public ThreeDPrinter threeDPrinter;

    public List<WeaponBlueprint> foundWeaponBlueprints = new List<WeaponBlueprint>();

    private Dictionary<string, GameObject> weaponNameToPrefabMap;


    void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        weaponNameToPrefabMap = new Dictionary<string, GameObject>();

        foreach (GameObject weapon in unlockedWeapons)
        {
            if (weapon != null)
            {
                weaponNameToPrefabMap[weapon.name] = weapon;
            }
        }
    }

    public void UnlockWeaponFromBlueprint(WeaponBlueprint blueprint)
    {
        foundWeaponBlueprints.Add(blueprint);
        GameObject newWeaponPrefab = blueprint.weaponPrefab;

        if (newWeaponPrefab != null)
        {
            unlockedWeapons.Add(newWeaponPrefab);
            weaponNameToPrefabMap[newWeaponPrefab.name] = newWeaponPrefab;
        }

        // Clear lastRandomWeapons in ThreeDPrinter
        if (threeDPrinter != null)
        {
            threeDPrinter.ClearLastRandomWeapons();
        }
        else
        {
            // If threeDPrinter is null, find the ThreeDPrinter instance
            threeDPrinter = FindObjectOfType<ThreeDPrinter>();
            if (threeDPrinter != null)
            {
                threeDPrinter.ClearLastRandomWeapons();
            }
        }
    }


    

    public GameObject GetWeaponPrefabByName(string weaponName)
    {
        GameObject weaponPrefab;
        if (weaponNameToPrefabMap.TryGetValue(weaponName, out weaponPrefab))
        {
            return weaponPrefab;
        }
        else
        {
            return null;
        }
    }

    public List<GameObject> GetRandomUnlockedWeapons(int count)
    {
        if (unlockedWeapons.Count <= count)
        {
            return new List<GameObject>(unlockedWeapons);
        }

        List<GameObject> selectedWeapons = new List<GameObject>();
        List<GameObject> remainingWeapons = new List<GameObject>(unlockedWeapons);

        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, remainingWeapons.Count);
            selectedWeapons.Add(remainingWeapons[randomIndex]);
            remainingWeapons.RemoveAt(randomIndex);
        }

        return selectedWeapons;
    }

    public bool PickWeapon(string weaponName)
    {
        if (string.IsNullOrEmpty(weaponName))
        {
            return false;  // Return false if the weaponName is null or empty
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            return false;  // Return false if the player object is not found
        }

        WeaponManager weaponManager = player.GetComponent<WeaponManager>();
        InventoryManager inventoryManager = InventoryManager.instance;

        if (weaponManager == null || inventoryManager == null)
        {
            return false;  // Return false if weaponManager or inventoryManager is null
        }

        GameObject weaponPrefab = WeaponManager.instance.GetWeaponPrefabByName(weaponName);
        if (weaponPrefab == null)
        {
            return false;  // Return false if weaponPrefab is null
        }

        Weapon newWeapon = InstantiateNewWeapon(weaponPrefab, weaponManager.weaponHolder);  // Call to encapsulated method
        if (newWeapon == null)
        {
            return false;  // Return false if newWeapon instantiation failed
        }

        AddWeaponToAvailableWeapons(newWeapon);  // Call to encapsulated method
        AddWeaponToInventory(newWeapon);  // Call to encapsulated method

        weaponManager.AddWeapon(newWeapon);  // Switch to the new weapon

       // Debug.Log("Picked: " + weaponName);

        InventoryManager.instance.UpdateInventoryUI();

        return true;  // Return true to indicate success
    }

    // New encapsulated methods
    private Weapon InstantiateNewWeapon(GameObject weaponPrefab, Transform holder)
    {
        // Corrected debug line. Hardcoding the method name.
        Debug.Log("Trying to instantiate weapon from prefab: " + weaponPrefab.name);

        Weapon newWeapon = Instantiate(weaponPrefab.GetComponent<Weapon>(), holder);
        newWeapon.gameObject.SetActive(false);
        newWeapon.transform.SetParent(holder);

        newWeapon.transform.localPosition = newWeapon.localPosition;
        newWeapon.transform.localRotation = Quaternion.identity;
        newWeapon.transform.localEulerAngles = newWeapon.localOrientation;

        newWeapon.ApplyTierModifiers();  // Call to apply tier modifiers

        // Corrected debug line, using newWeapon.name to refer to the instantiated weapon.
        Debug.Log("Weapon " + newWeapon.name + " instantiated.");

        return newWeapon;
    }


    private void AddWeaponToAvailableWeapons(Weapon newWeapon)
    {
        availableWeapons.Add(newWeapon);
    }

    private void AddWeaponToInventory(Weapon newWeapon)
    {
        // Create a new InventoryItem for the picked weapon
        InventoryItem newWeaponItem = new WeaponInventoryItem(newWeapon.weaponName)
        {
            weaponPrefab = newWeapon.gameObject,
            // ... initialize other properties as needed ...
        };

        // Add the new weapon to the inventory
        InventoryManager.instance.AddItem(newWeaponItem);
    }



    public void AddWeapon(Weapon newWeapon)
    {
        Debug.Log("Trying to add weapon: " + newWeapon.weaponName); // Add Debug here
        if (availableWeapons.Contains(newWeapon))
        {
            Debug.Log("Weapon is in availableWeapons list."); // Add Debug here
            int weaponCount = InventoryManager.instance.slots.Count(slot => slot.Item is WeaponInventoryItem);

            if (weaponCount >= InventoryManager.instance.maxWeaponSlots && !InventoryContainsWeapon(newWeapon))
            {
                Debug.Log("Inventory is full. Swapping weapons."); // Add Debug here
                return;
            }

            DeactivateCurrentWeapon();
            currentWeapon = newWeapon;
            newWeapon.gameObject.SetActive(true);
            CheckAndAddWeaponToInventory(newWeapon);
        }
    }

    // New encapsulated methods

    private void DeactivateCurrentWeapon()
    {
        // Deactivate current weapon
        if (currentWeapon != null)
        {
            WeaponInventoryItem currentWeaponItem = new WeaponInventoryItem(currentWeapon.weaponName) { weaponPrefab = currentWeapon.gameObject };
            InventoryManager.instance.RemoveItem(currentWeaponItem);
            currentWeapon.gameObject.SetActive(false);
        }
    }

    private void CheckAndAddWeaponToInventory(Weapon newWeapon)
    {
        // Check if the new weapon is already in the inventory
        bool isInInventory = false;
        foreach (var slot in InventoryManager.instance.slots)
        {
            if (slot.Item is WeaponInventoryItem weaponItem && weaponItem.weaponPrefab == newWeapon.gameObject)
            {
                isInInventory = true;
                break;
            }
        }

        // Check the number of weapons currently in the inventory
        int currentWeaponCount = InventoryManager.instance.slots.Count(slot => slot.Item is WeaponInventoryItem);

        // If the weapon is not already in the inventory and there's room for more weapons, add it
        if (!isInInventory && currentWeaponCount < InventoryManager.instance.maxWeaponSlots)
        {
            InventoryManager.instance.AddItem(new WeaponInventoryItem(newWeapon.weaponName) { weaponPrefab = newWeapon.gameObject });

            //Debug.Log("Weapon added to inventory.");
        }
        else if (!isInInventory && currentWeaponCount >= InventoryManager.instance.maxWeaponSlots)
        {
            Debug.Log("Inventory is full. Cannot add new weapon.");
                        
        }
    }

    // Helper method to check if a weapon is already in the inventory
    public bool InventoryContainsWeapon(Weapon weapon)
    {
        foreach (var slot in InventoryManager.instance.slots)
        {
            if (slot.Item is WeaponInventoryItem weaponItem && weaponItem.weaponPrefab == weapon.gameObject)
            {
                return true;
            }
        }
        return false;
    }

    
  
}


