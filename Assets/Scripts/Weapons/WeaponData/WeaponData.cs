using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapons/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
   
    public int baseDamage;
    public int upgradedDamage;
    public int range;
    public int speed;

    
    
    // Other properties can be added
}
