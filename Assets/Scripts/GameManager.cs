using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int sol = 0;
    
    public List<GameObject> unlockedWeapons;
    public List<WeaponBlueprint> foundWeaponBlueprints = new List<WeaponBlueprint>();
  
    private Dictionary<string, GameObject> weaponNameToPrefabMap;

    void Awake()
    {
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

        Debug.Log("Do you want to equip " + blueprint.weaponName + "?");
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

    public void AddSol(int amount)
    {
        sol += amount;
        UIManager.instance.UpdateSolDisplay();  // Update the UI
        Debug.Log("Total sol: " + sol);
    }

}
