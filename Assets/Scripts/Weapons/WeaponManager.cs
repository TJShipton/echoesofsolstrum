using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager instance; // Singleton instance

    public List<Weapon> availableWeapons; // List of weapons the player has
    public Weapon currentWeapon;          // Currently equipped weapon
    public Transform weaponHolder;

    public List<GameObject> unlockedWeapons;
   
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


    }

    public void UnlockWeapon(string weaponName)
    {
        if (weaponName == "OBow" || weaponName == "Drumstick")
        {
            return;
        }

        if (!weaponNameToPrefabMap.ContainsKey(weaponName))
        {
            GameObject newWeaponPrefab = Resources.Load<GameObject>("Prefabs/" + weaponName);

            if (newWeaponPrefab != null)
            {
                unlockedWeapons.Add(newWeaponPrefab);
                AddWeapon(newWeaponPrefab);
            }
        }
    }

    public void AddWeapon(GameObject newWeapon)
    {
        weaponNameToPrefabMap.Add(newWeapon.name, newWeapon);
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
    public void SwitchWeapon(Weapon newWeapon)
    {


        if (availableWeapons.Contains(newWeapon))
        {
            // Deactivate current weapon
            if (currentWeapon != null)
            {

                currentWeapon.gameObject.SetActive(false);
            }

            // Set and activate new weapon
            currentWeapon = newWeapon;

            currentWeapon.gameObject.SetActive(true);
        }

        WeaponManager.instance.UnlockWeapon(newWeapon.weaponName);
    }


}
//   keep the responsibilities clear
//  WeaponData should handle static data related to the weapon, Weapon should handle weapon logic,
//  and WeaponManager should handle player-level
