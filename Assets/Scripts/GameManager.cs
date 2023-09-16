using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<string> UnlockedWeapons = new List<string>(); // List of unlocked weapon names
    public List<GameObject> weaponPrefabs; // List of all weapon prefabs in the game
   
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            UnlockedWeapons.Add("Drumstick");
            UnlockedWeapons.Add("OBow");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UnlockWeapon(string weaponName)
    {
        if (!UnlockedWeapons.Contains(weaponName))
        {
            UnlockedWeapons.Add(weaponName);
        }
    }

    // This function returns a random weapon GameObject from the list of unlocked weapons.
    public GameObject GetRandomWeaponFromPool()
    {
        // Create a new list to store weapon prefabs that are unlocked.
        List<GameObject> unlockedWeaponPrefabs = new List<GameObject>();

        // Loop through each weapon prefab in the full list of weapon prefabs.
        foreach (GameObject weaponPrefab in weaponPrefabs)
        {
            // Try to get the Weapon component from the prefab.
            Weapon weapon = weaponPrefab.GetComponent<Weapon>();

            // Check if the Weapon component exists and if the weapon is unlocked.
            if (weapon != null && UnlockedWeapons.Contains(weapon.weaponName))
            {
                // Add this weapon prefab to the list of unlocked weapons.
                unlockedWeaponPrefabs.Add(weaponPrefab);
            }
        }

        // Check if there are any unlocked weapons.
        if (unlockedWeaponPrefabs.Count > 0)
        {
            // Pick a random index within the list of unlocked weapons.
            int randomIndex = Random.Range(0, unlockedWeaponPrefabs.Count);

            // Return a random unlocked weapon based on the index.
            return unlockedWeaponPrefabs[randomIndex];
        }

        // If no unlocked weapons are found, return null.
        return null;
    }



}
