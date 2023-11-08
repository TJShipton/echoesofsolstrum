using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager instance; // Singleton instance
    private InventoryManager inventoryManager;

    public List<Weapon> availableWeapons; // List of weapons the player has
    public Weapon currentWeapon;
    public List<Weapon> weaponInventory;  // Existing inventory list

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


        inventoryManager = InventoryManager.instance;

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



    public Weapon InstantiateNewWeapon(GameObject weaponPrefab, Transform holder)
    {
        // Instantiate the new weapon
        Weapon newWeapon = Instantiate(weaponPrefab.GetComponent<Weapon>(), holder);



        newWeapon.transform.SetParent(holder);

        //Set weapon inactive (to be reactivated during attack)
        newWeapon.gameObject.SetActive(false);

        newWeapon.transform.localPosition = newWeapon.localPosition;
        newWeapon.transform.localRotation = Quaternion.identity;
        newWeapon.transform.localEulerAngles = newWeapon.localOrientation;

        // Apply tier modifiers and check
        newWeapon.ApplyTierModifiers();

        // Ensure holder and newWeapon are active
        holder.gameObject.SetActive(true);


        ApplyRandomModifiersToWeapon(newWeapon);

        return newWeapon;
    }


    private void ApplyRandomModifiersToWeapon(Weapon weapon)
    {
        if (weapon.weaponData.weaponTier == WeaponTier.Rare)
        {
            weapon.EquipRandomModifiers();
        }
    }

    public void UpdateWeapons()
    {
        if (inventoryManager != null)
        {
            Weapon newCurrentWeapon = inventoryManager.GetCurrentWeapon();
            weaponInventory = inventoryManager.GetAllWeapons();
            // Log the newCurrentWeapon and currentWeapon before they are updated

            // Deactivate the previous weapon and activate the new weapon
            if (currentWeapon != null && newCurrentWeapon != currentWeapon)
            {
                currentWeapon.gameObject.SetActive(false);  // Deactivate the previous weapon
            }
            if (newCurrentWeapon != null)
            {
                newCurrentWeapon.gameObject.SetActive(true);  // Activate the new weapon
            }

            currentWeapon = newCurrentWeapon;  // Update the current weapon reference

        }
        else
        {
            Debug.LogWarning("InventoryManager is not set on WeaponManager.");
        }

        // Log the current weapon in WeaponManager
        if (currentWeapon != null)
        {
            //Debug.Log("Current Weapon in WeaponManager: " + currentWeapon.name);
        }
    }

}


