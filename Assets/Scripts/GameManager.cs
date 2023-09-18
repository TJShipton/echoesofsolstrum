using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    public static GameManager instance;

    // Create a list for unlocked weapons
    public List<GameObject> unlockedWeapons;

    // Create a dictionary to map weapon names to prefabs for easier access
    private Dictionary<string, GameObject> weaponNameToPrefabMap;

    void Awake()
    {
        Debug.Log("GameManager Awake called");

        // Singleton setup
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Initialize the dictionary
        weaponNameToPrefabMap = new Dictionary<string, GameObject>();

        // Populate the dictionary using the unlockedWeapons list
        foreach (GameObject weapon in unlockedWeapons)
        {
            if (weapon != null)
            {
                weaponNameToPrefabMap[weapon.name] = weapon;
            }
        }
    }

    // Function to unlock a weapon by its name
    public void UnlockWeapon(string weaponName)
    {
        if (!weaponNameToPrefabMap.ContainsKey(weaponName))
        {
            // Load the weapon prefab based on its name
            GameObject newWeaponPrefab = Resources.Load<GameObject>("Prefabs/" + weaponName);

            if (newWeaponPrefab != null)
            {
                unlockedWeapons.Add(newWeaponPrefab);
                AddWeapon(newWeaponPrefab);
            }
            else
            {
                Debug.LogError("Weapon prefab not found: " + weaponName);
            }
        }
        else
        {
            Debug.LogWarning("Weapon already unlocked: " + weaponName);
        }
    }

    public void AddWeapon(GameObject newWeapon)
    {
        weaponNameToPrefabMap.Add(newWeapon.name, newWeapon);
    }

    // Function to get weapon prefab by its name
    public GameObject GetWeaponPrefabByName(string weaponName)
    {
        Debug.Log("Looking for Weapon Prefab: " + weaponName);
        GameObject weaponPrefab;
        if (weaponNameToPrefabMap.TryGetValue(weaponName, out weaponPrefab))
        {
            return weaponPrefab;
        }
        else
        {
            Debug.LogError("Weapon not found: " + weaponName);
            return null;
        }
    }
}
