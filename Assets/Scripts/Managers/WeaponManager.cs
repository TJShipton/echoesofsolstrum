using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager instance; // Singleton instance

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
        Debug.Log("Trying to instantiate weapon from prefab: " + weaponPrefab.name);

        Weapon newWeapon = Instantiate(weaponPrefab.GetComponent<Weapon>(), holder);
        newWeapon.gameObject.SetActive(false);
        newWeapon.transform.SetParent(holder);

        newWeapon.transform.localPosition = newWeapon.localPosition;
        newWeapon.transform.localRotation = Quaternion.identity;
        newWeapon.transform.localEulerAngles = newWeapon.localOrientation;

        newWeapon.ApplyTierModifiers();  // Assuming this method exists in your Weapon class

        //holder.gameObject.SetActive(true);  // Ensure holder is active
        newWeapon.gameObject.SetActive(true);  // Activate newWeapon

        Debug.Log("Weapon " + newWeapon.name + " instantiated.");

        return newWeapon;
    }

}


