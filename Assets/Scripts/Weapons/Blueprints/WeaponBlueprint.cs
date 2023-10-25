using UnityEngine;

[CreateAssetMenu(fileName = "New WeaponBlueprint", menuName = "Weapons/Weapon Blueprint")]
public class WeaponBlueprint : ScriptableObject
{
    public string weaponName; // The name of the weapon
    public GameObject weaponPrefab; // The prefab to instantiate when this weapon is crafted/collected
    public int damage; // Damage this weapon does
}


